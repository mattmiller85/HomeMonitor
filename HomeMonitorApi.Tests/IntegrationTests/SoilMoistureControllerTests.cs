using HomeMonitorApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HomeMonitorApi.Tests.IntegrationTests
{
    [TestClass]
    public class SoilMoistureControllerTests
    {
        //private const string TestBaseAddress = "http://localhost:60945/";
        private const string TestBaseAddress = "http://homemonitorapi/";

        [TestMethod]
        public async Task ShouldAttemptToCallGetLatestWithNoParametersWhenIssuing_GET_To_api_SoilMoisture_latest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(TestBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/SoilMoisture/latest");
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IEnumerable<SoilMoistureViewModel>>(json);

                Assert.IsTrue(model.Any());
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task ShouldAttemptToCallGetLatestWithIntParameterWhenIssuing_GET_To_api_Temperature_latest_1()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(TestBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/SoilMoisture/latest/1");
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<SoilMoistureViewModel>(json);

                Assert.IsTrue(model.IsValid);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task ShouldAttemptToAddTemperatureIssuing_POST_To_api_Temperature_addreading()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(TestBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var itemToAdd = new SoilMoistureViewModel { Taken = DateTime.Parse("1/1/2014"), MilliVolts = 400, SensorNumber = 1};
                var response = await client.PostAsJsonAsync("api/SoilMoisture/addreading", itemToAdd);
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<SoilMoistureViewModel>(json);

                Assert.IsTrue(model.Taken > DateTime.Parse("1/1/2014"));
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }
    }
}
