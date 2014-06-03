using HomeMonitorApi.Models;
using HomeMonitorDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HomeMonitorApi.Controllers
{
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

        // GET: api/SoilMoisture/latest/[sensorNumber]
        [HttpGet]
        [ActionName("latest")]
        public SoilMoistureViewModel GetLatest(int sensorNumber)
        {
            try
            {
                return SoilMoistureViewModel.FromData(
                    _data.SoilMoistureReadings
                        .Where(r => r.SensorNumber == sensorNumber)
                        .OrderByDescending(r => r.Taken)
                        .FirstOrDefault());
            }
            catch (InvalidOperationException)
            {
                throw new NoSoilMoistureReadingsException();
            }
        }

        // GET: api/SoilMoisture/latest
        [HttpGet]
        [ActionName("latest")]
        public IEnumerable<SoilMoistureViewModel> GetLatest()
        {
            var data = _data.SoilMoistureReadings
                .OrderByDescending(r => r.Taken)
                .GroupBy(r => r.SensorNumber)
                .Select(g => g.FirstOrDefault()).ToList();
            return data.Select(SoilMoistureViewModel.FromData);
        }

        // POST: api/Temperature
        [HttpPost]
        [ActionName("addreading")]
        public SoilMoistureViewModel AddReading([FromBody]SoilMoistureViewModel value)
        {
            if(!value.IsValid)
                throw new ArgumentOutOfRangeException();
            value.Taken = DateTime.Now;
            _data.SoilMoistureReadings.Add(value.ToData());
            _data.SaveChanges();
            return value;
        }

        // PUT: api/Temperature/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/Temperature/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
