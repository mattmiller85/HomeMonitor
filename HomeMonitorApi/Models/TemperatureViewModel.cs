using HomeMonitorDataAccess;
using System;

namespace HomeMonitorApi.Models
{
    public class TemperatureViewModel : IHomeMonitorApiModel<Temperature>
    {
        public int TemperatureFarenheit { get; set; }
        public DateTime Taken { get; set; }

        public static TemperatureViewModel FromData(Temperature temp)
        {
            return new TemperatureViewModel {Taken = temp.Taken, TemperatureFarenheit = temp.TemperatureFarenheit};
        }

        public Temperature ToData()
        {
            return new Temperature { Taken = this.Taken, TemperatureFarenheit = this.TemperatureFarenheit };
        }

        public bool IsValid
        {
            get { return TemperatureFarenheit > -50 && TemperatureFarenheit < 120; }
        }
    }
}