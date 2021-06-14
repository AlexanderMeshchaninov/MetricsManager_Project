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
    public class NetworkControllerUnitTests
    {
        private readonly NetworkMetricsController _controller;

        private readonly Mock<ILogger<NetworkMetricsController>> _logger;

        private readonly Mock<INetworkMetricsAgentsRepository> _repository;

        private readonly IMapper _mapper;

        public NetworkControllerUnitTests()
        {
            var mapperConfiguration = new MapperConfiguration(mp => mp
                .AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();

            _logger = new Mock<ILogger<NetworkMetricsController>>();

            _repository = new Mock<INetworkMetricsAgentsRepository>();

            _controller = new NetworkMetricsController(_logger.Object, _repository.Object, mapper);
        }

        [Fact]
        public void GetMetricsFromAgent()
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
            var request = new NetworkMetricsApiRequest()
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