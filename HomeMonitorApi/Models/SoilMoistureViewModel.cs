using HomeMonitorDataAccess;
using System;

namespace HomeMonitorApi.Models
{
    public class SoilMoistureViewModel : IHomeMonitorApiModel<SoilMoistureReading>
    {
        public int SensorNumber { get; set; }
        public string SensorName { get; set; }
        public string SensorLocation { get; set; }
        public DateTime Taken { get; set; }
        public decimal MilliVolts { get; set; }

        public static SoilMoistureViewModel FromData(SoilMoistureReading reading)
        {
            return new SoilMoistureViewModel
            {
                Taken = reading.Taken,
                MilliVolts = reading.MilliVolts,
                SensorNumber = reading.SensorNumber,
                SensorName = reading.SoilMoistureSensor == null ? string.Empty : reading.SoilMoistureSensor.SensorName,
                SensorLocation = reading.SoilMoistureSensor == null ? string.Empty : reading.SoilMoistureSensor.SensorLocation
            };
        }

        public SoilMoistureReading ToData()
        {
            return new SoilMoistureReading
            {
                Taken = this.Taken,
                MilliVolts = this.MilliVolts,
                SensorNumber = this.SensorNumber
            };
        }

        public bool IsValid
        {
            get { return SensorNumber > 0; }
        }
    }
}