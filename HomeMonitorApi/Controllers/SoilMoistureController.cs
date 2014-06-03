using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http;
using HomeMonitorApi.Models;
using HomeMonitorDataAccess;

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

        // GET: api/Temperature/latest
        [HttpGet]
        [ActionName("latest")]
        public TemperatureViewModel GetLatest()
        {
            try
            {
                return GetLatest(1).First();
            }
            catch (InvalidOperationException)
            {
                throw new NoTemperatureReadingsException();
            }
            
        }

        // GET: api/Temperature/latest/[count]
        [HttpGet]
        [ActionName("latest")]
        public IEnumerable<TemperatureViewModel> GetLatest(int count)
        {
            return _data.Temperatures.OrderByDescending(t => t.Taken).Take(count).Select(t => TemperatureViewModel.FromData(t));
        }

        // POST: api/Temperature
        [HttpPost]
        [ActionName("addreading")]
        public void AddReading([FromBody]TemperatureViewModel value)
        {
            throw new NotImplementedException();
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
