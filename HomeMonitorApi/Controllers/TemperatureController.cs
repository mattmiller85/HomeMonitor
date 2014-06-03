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
    public class TemperatureController : ApiController
    {
        private readonly IHomeMonitorDataContext _data;

        public TemperatureController()
        {
            _data = new HomeMonitorEntities();
        }

        public TemperatureController(IHomeMonitorDataContext data)
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
            return _data.Temperatures.OrderByDescending(t => t.Taken).Take(count).ToList().Select(TemperatureViewModel.FromData);
        }
            
        // POST: api/Temperature/addreading
        [HttpPost]
        [ActionName("addreading")]
        public TemperatureViewModel AddReading([FromBody]TemperatureViewModel value)
        {
            if (!value.IsValid)
                throw new ArgumentOutOfRangeException();
            value.Taken = DateTime.Now;
            _data.Temperatures.Add(value.ToData());
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
