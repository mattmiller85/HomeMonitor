using HomeMonitorApi.Models;
using HomeMonitorDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HomeMonitorApi.Controllers
{
    [RoutePrefix("api/Temperature")]
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

        // GET: api/Temperature
        [Route("")]
        public TemperatureViewModel Get()
        {
            try
            {
                return Get(1).First();
            }
            catch (InvalidOperationException)
            {
                throw new NoTemperatureReadingsException();
            }
            
        }

        // GET: api/Temperature/[count]
        [Route("{count:int}")]
        public IEnumerable<TemperatureViewModel> Get(int count)
        {
            return _data.Temperatures.OrderByDescending(t => t.Taken).Take(count).ToList().Select(TemperatureViewModel.FromData);
        }
            
        // POST: api/Temperature
        [Route("")]
        public TemperatureViewModel Post([FromBody]TemperatureViewModel value)
        {
            if (!value.IsValid)
                throw new ArgumentOutOfRangeException();
            value.Taken = DateTime.Now;
            _data.Temperatures.Add(value.ToData());
            _data.SaveChanges();
            return value;
        }
    }
}
