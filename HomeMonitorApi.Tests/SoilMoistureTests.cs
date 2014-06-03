using System;
using System.Data.Entity;
using System.Linq;
using HomeMonitorApi.Controllers;
using HomeMonitorDataAccess;
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
    }
}
