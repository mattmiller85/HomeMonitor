using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using HomeMonitorApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeMonitorApi.Tests.IntegrationTests
{
    [TestClass]
    public class TemperatureControllerTests
    {
        [TestInitialize]
        public void Setup()
        {
            
        }

        [TestMethod]
        public async Task ShouldAttemptToCallGetLatestWithNoParametersWhenIssuing_GET_To_api_Temperature_latest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60945/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/Temperature/latest");
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<TemperatureViewModel>(json);

                Assert.IsTrue(model.IsValid);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task ShouldAttemptToCallGetLatestWithIntParameterWhenIssuing_GET_To_api_Temperature_latest_5()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60945/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/Temperature/latest/5");
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IEnumerable<TemperatureViewModel>>(json);

                Assert.IsTrue(model.Any());
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }
    }
}
