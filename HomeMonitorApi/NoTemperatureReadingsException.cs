using System;

namespace HomeMonitorApi
{
    public class NoTemperatureReadingsException : Exception
    {
        public override string Message
        {
            get { return "There are currently no temperature readings."; }
        }
    }

    public class NoSoilMoistureReadingsException : Exception
    {
        public override string Message
        {
            get { return "There are currently no soil moisture readings."; }
        }
    }
}