using MetricsManager.Controllers;
using System;
using System.Collections.Generic;
using AutoMapper;
using MetricsManager;
using MetricsManager.DAL;
using MetricsManager.Models;
using MetricsManager.Requests;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace MetricsManagerTests
{
    public class CpuControllerUnitTests
    {
        private readonly CpuMetricsController _controller;

        private readonly Mock<ILogger<CpuMetricsController>> _logger;

        private readonly Mock<ICpuMetricsAgentsRepository> _repository;

        public CpuControllerUnitTests()
        {
            var mapperConfiguration = new MapperConfiguration(mp => mp
                .AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();

            _logger = new Mock<ILogger<CpuMetricsController>>();

            _repository = new Mock<ICpuMetricsAgentsRepository>();

            _controller = new CpuMetricsController(_logger.Object, _repository.Object, mapper);
        }

        [Fact]
        public void GetMetricsFromAgent()
        {
            //Mock Setup
            _repository.Setup(repo => repo
                    .GetByTimePeriod(
                        It.IsAny<DateTimeOffset>(),
                        It.IsAny<DateTimeOffset>()))
                .Returns(new List<CpuMetrics>()
                {
                    new CpuMetrics()
                    {
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(10000),
                        Value = 100,
                    }
                }).Verifiable();

            //Arrange
            var request = new CpuMetricsApiRequest()
            {
                FromTime = DateTimeOffset.FromUnixTimeMilliseconds(1),
                ToTime = DateTimeOffset.FromUnixTimeMilliseconds(100),
            };

            //Act    
            var result = _controller.GetMetricsFromAgent(request);
            Assert.IsAssignableFrom<IActionResult>(result);

            //Assert
            _repository.Verify(repo => repo
                .GetByTimePeriod(
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>()
                ), Times.AtMostOnce());
        }
    }
}
