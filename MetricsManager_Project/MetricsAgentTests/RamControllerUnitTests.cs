using MetricsAgent.Controllers;
using System;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Requests;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AutoMapper;
using MetricsAgent;

namespace MetricsAgentTests
{
    public class RamControllerUnitTests
    {
        private readonly RamMetricsController _controller;

        private readonly Mock<IRamMetricsRepository> _repository;

        private readonly Mock<ILogger<RamMetricsController>> _logger;

        private readonly IMapper _mapper;

        public RamControllerUnitTests()
        {
            var mapperConfiguration = new MapperConfiguration(mp => mp
                .AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();

            _repository = new Mock<IRamMetricsRepository>();

            _logger = new Mock<ILogger<RamMetricsController>>();

            _controller = new RamMetricsController(_logger.Object, _repository.Object, mapper);
        }

        [Fact]
        public void GetByTimePeriod_From_Controller()
        {
            //Mock Setup
            _repository.Setup(repo => repo
                    .GetByTimePeriod(
                        It.IsAny<DateTimeOffset>(),
                        It.IsAny<DateTimeOffset>()))
                .Returns(new List<RamMetrics>()
                {
                    new RamMetrics()
                    {
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(10000),
                        Value = 100,
                    }
                }).Verifiable();

            //Arrange
            var request = new RamMetricGetByTimePeriodRequest()
            {
                FromTime = DateTimeOffset.FromUnixTimeMilliseconds(1),
                ToTime = DateTimeOffset.FromUnixTimeMilliseconds(100),
            };

            //Act    
            var result = _controller.GetByTimePeriod(request);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(10000), result.Metrics.ToArray()[0].Time);
            Assert.Equal(100, result.Metrics.ToArray()[0].Value);

            //Assert
            _repository.Verify(repo => repo
                .GetByTimePeriod(
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>()
                ), Times.AtMostOnce());
        }
    }
}
