using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/manager/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;

        private readonly ICpuMetricsAgentsRepository _repository;

        private readonly IMapper _mapper;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsAgentsRepository repository, IMapper mapper)
        {
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into CpuMetricsController");

            _repository = repository;

            _mapper = mapper;
        }

        /// <summary>
        /// Контроллер получает метрики Cpu (ранее полученные от Агента Метрик) из базы данных Менеджера принимая на вход ответ ввиде серилизованного json
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /api/metrics/manager/cpu/getmetricsfromagent
        /// </remarks>
        /// 
        /// <param name="FromTime">"FromTime": "2021-01-01T21:00:00+00:00"</param>
        /// <param name="ToTime">"ToTime": "2021-05-19T21:00:00+00:00"</param>
        /// 
        /// <returns>Cписок метрик, которые были сохранены в заданном диапазоне времени ввиде response-ответа в формате json
        /// </returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">Если передали не правильные параметры</response>
        [HttpPost("getmetricsfromagent")]
        public IActionResult GetMetricsFromAgent([FromBody] CpuMetricsApiRequest request)
        {
            _logger.LogInformation(1, $"Starting new request to CpuMetrics agent " +
                                      $"- fromTime:{request.FromTime}, toTime:{request.ToTime}");

            var metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);

            var response = new CpuMetricsApiResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
