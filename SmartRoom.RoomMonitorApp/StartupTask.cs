using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Sensors.Dht;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SmartRoom.RoomMonitorApp
{
    public sealed class StartupTask : IBackgroundTask
    {
        private GpioPin _temperaturePin;

        private Dht11 _dht11;
        private Timer _timer;


        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            InitHardware();
        }

        private void InitHardware()
        {
            _temperaturePin = GpioController.GetDefault().OpenPin(4, GpioSharingMode.Exclusive);

            _dht11 = new Dht11(_temperaturePin, GpioPinDriveMode.Input);

            _timer = new Timer(Timer_Tick, null, 0, 10000);
        }

        private async void Timer_Tick(object sender)
        {
            try
            {
                DhtReading reading = new DhtReading();
                reading = await _dht11.GetReadingAsync(3).AsTask();

                if (reading.IsValid)
                {
                    Console.WriteLine(reading.Temperature);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
