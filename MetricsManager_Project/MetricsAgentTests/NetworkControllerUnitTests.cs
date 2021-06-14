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
    public class NetworkControllerUnitTests
    {
        private readonly NetworkMetricsController _controller;

        private readonly Mock<INetworkMetricsRepository> _repository;

        private readonly Mock<ILogger<NetworkMetricsController>> _logger;

        private readonly IMapper _mapper;

        public NetworkControllerUnitTests()
        {
            var mapperConfiguration = new MapperConfiguration(mp => mp
                .AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();

            _repository = new Mock<INetworkMetricsRepository>();

            _logger = new Mock<ILogger<NetworkMetricsController>>();

            _controller = new NetworkMetricsController(_logger.Object, _repository.Object, mapper);
        }

        [Fact]
        public void GetByTimePeriod_From_Controller()
        {
            //Mock Setup
            _repository.Setup(repo => repo
                    .GetByTimePeriod(
                        It.IsAny<DateTimeOffset>(),
                        It.IsAny<DateTimeOffset>()))
                .Returns(new List<NetworkMetrics>()
                {
                    new NetworkMetrics()
                    {
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(10000),
                        Value = 100,
                    }
                }).Verifiable();

            //Arrange
            var request = new NetworkMetricGetByTimePeriodRequest()
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
