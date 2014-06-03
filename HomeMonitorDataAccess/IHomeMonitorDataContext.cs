using System.Data.Entity;

namespace HomeMonitorDataAccess
{
    public interface IHomeMonitorDataContext
    {
        DbSet<Temperature> Temperatures { get; set; }
        DbSet<SoilMoistureReading> SoilMoistureReadings { get; set; }
    }
}
