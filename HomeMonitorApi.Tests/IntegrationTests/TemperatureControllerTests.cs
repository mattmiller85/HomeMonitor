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
    public class TemperatureControllerTests
    {
        private const string TestBaseAddress = "http://localhost:60945/";
        //private const string TestBaseAddress = "http://homemonitorapi/";
        
        [TestMethod]
        public async Task ShouldAttemptToCallGetLatestWithNoParametersWhenIssuing_GET_To_api_Temperature()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(TestBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/Temperature");
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<TemperatureViewModel>(json);

                Assert.IsTrue(model.IsValid);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task ShouldAttemptToCallGetLatestWithIntParameterWhenIssuing_GET_To_api_Temperature_5()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(TestBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/Temperature/5");
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IEnumerable<TemperatureViewModel>>(json);

                Assert.IsTrue(model.Any());
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task ShouldAttemptToAddTemperatureIssuing_POST_To_api_Temperature()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(TestBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var itemToAdd = new TemperatureViewModel {TemperatureFarenheit = 55, Taken = DateTime.Parse("1/1/2014")};
                var response = await client.PostAsJsonAsync("api/Temperature", itemToAdd);
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<TemperatureViewModel>(json);

                Assert.IsTrue(model.Taken > DateTime.Parse("1/1/2014"));
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            }
        }
    }
}
