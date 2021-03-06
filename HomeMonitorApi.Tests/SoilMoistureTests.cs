﻿using System;
using System.Linq;
using HomeMonitorApi.Controllers;
using HomeMonitorApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestExtensions;

namespace HomeMonitorApi.Tests
{
    [TestClass]
    public class SoilMoistureTests
    {
        private SoilMoistureController _controller;
        private HomeMonitorEntitiesStub _dummyData;

        [TestInitialize]
        public void Setup()
        {
            _dummyData = new HomeMonitorEntitiesStub();
            _controller = new SoilMoistureController(_dummyData);
        }

        [TestMethod]
        public void GetWithParametersShouldReturnLatestReadingForGivenSensorNumber()
        {
            var result = _controller.Get(2);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.MilliVolts, 900m);
        }

        [TestMethod]
        public void GetWithNoParametersShouldReturnLatestReadingsForAllSensors()
        {
            var result = _controller.Get().ToList();

            Assert.AreEqual(result.Count(), 6);
            Assert.AreEqual(result.Single(s => s.SensorNumber == 1).MilliVolts, 1000m);
            Assert.AreEqual(result.Single(s => s.SensorNumber == 2).MilliVolts, 900m);
        }

        [TestMethod]
        public void ShouldThrowNoSoilMoistureReadingsExceptionIfNoDataForGivenSensor()
        {
            _dummyData.SoilMoistureReadings.RemoveRange(null);

            ExceptionAssert.Throws<NoSoilMoistureReadingsException>(() => _controller.Get(1), "There are currently no soil moisture readings.");
        }

        [TestMethod]
        public void ShouldNotAddSoilMoistureReadingForInvalidSensorNumber()
        {
            var model = new SoilMoistureViewModel
            {
                MilliVolts = 987,
                SensorNumber = -34,
                Taken = DateTime.Parse("1/1/2014")
            };
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => _controller.Post(model));
        }

        [TestMethod]
        public void ShouldAddSoilMoistureReadingForValidSensorNumber()
        {
            var model = new SoilMoistureViewModel
            {
                MilliVolts = 987,
                SensorNumber = 1,
                Taken = DateTime.Parse("1/1/2014")
            };

            model = _controller.Post(model);

            Assert.IsTrue(model.Taken > DateTime.Parse("1/1/2014"));
            Assert.AreEqual(987, _controller.Get(1).MilliVolts);
        }
    }
}
