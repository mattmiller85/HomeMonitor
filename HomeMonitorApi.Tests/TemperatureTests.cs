using HomeMonitorApi.Controllers;
using HomeMonitorApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestExtensions;
using System;
using System.Linq;

namespace HomeMonitorApi.Tests
{
    [TestClass]
    public class TemperatureTests
    {
        private TemperatureController _controller;
        private HomeMonitorEntitiesStub _dummyData;

        [TestInitialize]
        public void Setup()
        {
            _dummyData = new HomeMonitorEntitiesStub();
            _controller = new TemperatureController(_dummyData);
        }

        [TestMethod]
        public void GetLatestWithNoParametersShouldReturnLatestTemperature()
        {
            Assert.AreEqual(_controller.GetLatest().Taken, DateTime.Parse("1/6/2014 10:20:41 AM"));
        }

        [TestMethod]
        public void GetLatestWithParameterShouldReturnThatNumberOfReadingsInDescendingOrder()
        {
            var readings = _controller.GetLatest(3).ToList();

            Assert.AreEqual(readings.Count(), 3);
            Assert.IsTrue(readings.First().Taken > readings.Skip(1).Take(1).First().Taken);
            Assert.IsTrue(readings.Skip(1).Take(1).First().Taken > readings.Skip(2).Take(1).First().Taken);
        }

        [TestMethod]
        public void ShouldThrowNoTemperatureReadingsExceptionIfNoData()
        {
            _dummyData.Temperatures.RemoveRange(null);

            ExceptionAssert.Throws<NoTemperatureReadingsException>(() => _controller.GetLatest(), "There are currently no temperature readings.");
        }

        [TestMethod]
        public void ShouldNotAddTemperatureReadingWhenCallingAddReadingWithInvalidLowTemperature()
        {
            var temp = new TemperatureViewModel { TemperatureFarenheit = -76 };

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => _controller.AddReading(temp));
        }

        [TestMethod]
        public void ShouldNotAddTemperatureReadingWhenCallingAddReadingWithInvalidHighTemperature()
        {
            var temp = new TemperatureViewModel { TemperatureFarenheit = 150 };

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => _controller.AddReading(temp));
        }

        [TestMethod]
        public void ShouldAddTemperatureReadingWhenCallingAddReading()
        {
            var temp = new TemperatureViewModel {Taken = DateTime.Parse("3/1/2014 09:56:37 AM"), TemperatureFarenheit = 63};

            _controller.AddReading(temp);

            var newReading = _dummyData.Temperatures.Last();
            Assert.IsTrue(newReading.TemperatureFarenheit == 63 && newReading.Taken != DateTime.Parse("3/1/2014 09:56:37 AM"));
        }
    }
}
