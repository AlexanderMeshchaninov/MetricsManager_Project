using System;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using MetricsManager.DAL;
using MetricsManager.Models;
using MetricsManager.Requests;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private readonly AgentsController controller;

        private readonly Mock<ILogger<AgentsController>> _logger;

        private readonly Mock<IAgentsRepository> _repository;

        public AgentsControllerUnitTests()
        {
            _logger = new Mock<ILogger<AgentsController>>();

            _repository = new Mock<IAgentsRepository>();

            controller = new AgentsController(_logger.Object, _repository.Object);
        }

        [Fact]
        public void RegisterAgent_ReturnOk()
        {
            //Mock Setup
            _repository.Setup(repo => repo
                .Create(new AgentInfo()
                {
                    AgentId = 1,
                    AgentAddress = "51684",
                })).Verifiable();
            
            //Arrange
            var request = new AgentInfoApiRequest()
            {
                AgentId = 1,
                AgentAddress = "51684",
            };

            //Act
            var result = controller.RegisterAgent(request);
            Assert.IsAssignableFrom<IActionResult>(result);

            //Assert
            _repository.Verify(repo => repo
                .Create(It.IsAny<AgentInfo>()));
        }

        [Fact]
        public void EnableAgentById_ReturnOk()
        {
            //Mock Setup
            _repository.Setup(repo => repo
                .Update(1, 1)).Verifiable();

            //Arrange
            var agentId = 1;

            //Act
            var result = controller.EnableAgentById(agentId);
            Assert.IsAssignableFrom<IActionResult>(result);

            //Assert
            _repository.Verify(repo => repo
                .Update(1, 1));
        }

        [Fact]
        public void DisableAgentById_ReturnOk()
        {
            //Mock Setup
            _repository.Setup(repo => repo
                .Update(1, 0)).Verifiable();

            //Arrange
            var agentId = 1;

            //Act
            var result = controller.DisableAgentById(agentId);
            Assert.IsAssignableFrom<IActionResult>(result);

            //Assert
            _repository.Verify(repo => repo
                .Update(1, 0));
        }
    }
}
