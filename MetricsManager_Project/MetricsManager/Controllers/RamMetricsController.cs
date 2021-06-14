using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/manager/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;

        private readonly IRamMetricsAgentsRepository _repository;

        private readonly IMapper _mapper;

        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsAgentsRepository repository, IMapper mapper)
        {
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into RamMetricsController");

            _repository = repository;

            _mapper = mapper;
        }

        /// <summary>
        /// Контроллер получает метрики Ram (ранее полученные от Агента Метрик) из базы данных Менеджера принимая на вход ответ ввиде серилизованного json
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /api/metrics/manager/ram/getmetricsfromagent
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
        public IActionResult GetMetricsFromAgent([FromBody] RamMetricsApiRequest request)
        {
            _logger.LogInformation(1, $"Starting new request to RamMetrics agent " +
                                      $"- fromTime:{request.FromTime}, toTime:{request.ToTime}");

            var metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);

            var response = new RamMetricsApiResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
