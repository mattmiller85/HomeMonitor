using System.Data.Entity;
using HomeMonitorApi.Models;
using HomeMonitorDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HomeMonitorApi.Controllers
{
    [RoutePrefix("api/SoilMoisture")]
    public class SoilMoistureController : ApiController
    {
        private readonly IHomeMonitorDataContext _data;

        public SoilMoistureController()
        {
            _data = new HomeMonitorEntities();
        }

        public SoilMoistureController(IHomeMonitorDataContext data)
        {
            _data = data;
        }

        // GET: api/SoilMoisture/[sensorNumber]
        [Route("{sensorNumber:int}")]
        public SoilMoistureViewModel Get(int sensorNumber)
        {
            try
            {
                return SoilMoistureViewModel.FromData(
                    _data.SoilMoistureReadings
                        .Where(r => r.SensorNumber == sensorNumber)
                        .OrderByDescending(r => r.Taken)
                        .FirstOrDefault());
            }
            catch (NullReferenceException)
            {
                throw new NoSoilMoistureReadingsException();
            }
        }

        // GET: api/SoilMoisture
        [Route("")]
        public IEnumerable<SoilMoistureViewModel> Get()
        {
            var data = _data.SoilMoistureReadings
                .OrderByDescending(r => r.Taken)
                .GroupBy(r => r.SensorNumber)
                .Select(g => g.FirstOrDefault());
            return data.Select(SoilMoistureViewModel.FromData);
        }

        // POST: api/Temperature
        [Route("")]
        public SoilMoistureViewModel Post([FromBody]SoilMoistureViewModel value)
        {
            if(!value.IsValid)
                throw new ArgumentOutOfRangeException();
            value.Taken = DateTime.Now;
            _data.SoilMoistureReadings.Add(value.ToData());
            _data.SaveChanges();
            return value;
        }
    }
}
