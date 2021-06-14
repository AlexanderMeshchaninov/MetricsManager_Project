using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using MetricsAgent.DAL;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/agent/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;

        private readonly IRamMetricsRepository _repository;

        private readonly IMapper _mapper;

        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into RamMetricsController");

            _repository = repository;

            _mapper = mapper;
        }

        /// <summary>
        /// Контроллер получает метрики Ram принимая на вход запрос
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /api/metrics/agent/ram/getbytimeperiod
        /// </remarks>
        /// 
        /// <param name="FromTime">"FromTime": "2021-01-01T21:00:00+00:00"</param>
        /// <param name="ToTime">"ToTime": "2021-05-19T21:00:00+00:00"</param>
        /// 
        /// <returns>Cписок метрик, которые были сохранены в заданном диапазоне времени ввиде response-ответа
        /// </returns>
        /// <response code="200">Если все хорошо</response>
        /// <response code="400">Если передали не правильные параметры</response>
        [HttpPost("getbytimeperiod")]
        public RamMetricsResponse GetByTimePeriod([FromBody] RamMetricGetByTimePeriodRequest request)
        {
            _logger.LogInformation(1, 
                $"This log from Agent and GetByTimePeriod - fromTime:{request.FromTime}, toTime:{request.ToTime}");

            var metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);

            var response = new RamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }

            return response;
        }
    }
}
