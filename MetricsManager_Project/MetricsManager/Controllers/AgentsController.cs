using System;
using MetricsManager.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetricsManager.Models;
using MetricsManager.Requests;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/manager/")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;

        private readonly IAgentsRepository _agentsRepository;

        public AgentsController(ILogger<AgentsController> logger, IAgentsRepository agentsRepository)
        {
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into AgentsController");

            _agentsRepository = agentsRepository;
        }

        /// <summary>
        /// Контроллер получает адрес агента и его ID принимая на вход запрос ввиде серилизованного json
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /api/metrics/manager/register
        /// </remarks>
        /// 
        /// <param name="AgentId">"AgentId": "1"</param>
        /// <param name="AgentAddress">"AgentAddress": "http://localhost:51684"</param>
        /// 
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">Если передали не правильные параетры</response>
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfoApiRequest request)
        {
            _logger.LogInformation(1, $"This log from RegisterAgent " +
                                      $"- agentId:{request.AgentId}, agentAddress:{request.AgentAddress}");

            _agentsRepository.Create(new AgentInfo
            {
                AgentId = request.AgentId,

                AgentAddress = request.AgentAddress,

                Status = 0,
            });

            _logger.LogInformation("Agent has been registered");

            return Ok();
        }

        /// <summary>
        /// Контроллер получает ID агента, которого нужно "включить" принимая на вход цифры в int представлении
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// PUT /api/metrics/manager/enable
        /// </remarks>
        /// 
        /// <param name="agentId">"agentId": "1"</param>
        /// 
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">Если передали не правильные параетры</response>
        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation(1, $"This log from EnableAgentById - agentId:{agentId}");

            //Из-за того, что Sqlite не поддерживает тип bool использую "магические числа" :-)
            _agentsRepository.Update(agentId, 1);

            _logger.LogInformation($"Agent with {agentId} - enabled");

            return Ok();
        }

        /// <summary>
        /// Контроллер получает ID агента, которого нужно "выключить" принимая на вход цифры в int представлении
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// PUT /api/metrics/manager/disable
        /// </remarks>
        /// 
        /// <param name="agentId">"agentId": "1"</param>
        /// 
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">Если передали не правильные параетры</response>
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation(1, $"This log from DisableAgentById - agentId:{agentId}");

            _agentsRepository.Update(agentId);

            _logger.LogInformation($"Agent with {agentId} - disabled");

            return Ok();
        }
    }
}
