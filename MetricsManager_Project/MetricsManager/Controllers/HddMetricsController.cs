using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using MetricsManager.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/manager/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;

        private readonly IHddMetricsAgentsRepository _repository;

        private readonly IMapper _mapper;

        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsAgentsRepository repository, IMapper mapper)
        {
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into HddMetricsController");

            _repository = repository;

            _mapper = mapper;
        }

        /// <summary>
        /// Контроллер получает метрики Hdd (ранее полученные от Агента Метрик) из базы данных Менеджера принимая на вход ответ ввиде серилизованного json
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /api/metrics/manager/hdd/getmetricsfromagent
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
        public IActionResult GetMetricsFromAgent([FromBody] HddMetricsApiRequest request)
        {
            _logger.LogInformation(1, $"Starting new request to HddMetrics agent " +
                                      $"- fromTime:{request.FromTime}, toTime:{request.ToTime}");

            var metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);

            var response = new HddMetricsApiResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
