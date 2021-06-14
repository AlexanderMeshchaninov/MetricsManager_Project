using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using MetricsAgent.DAL;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/agent/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;

        private readonly INetworkMetricsRepository _repository;

        private readonly IMapper _mapper;

        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into NetworkMetricsController");

            _repository = repository;

            _mapper = mapper;
        }

        /// <summary>
        /// Контроллер получает метрики Network принимая на вход запрос
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /api/metrics/agent/network/getbytimeperiod
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
        public NetworkMetricResponse GetByTimePeriod([FromBody] NetworkMetricGetByTimePeriodRequest request)
        {
            _logger.LogInformation(1, 
                $"This log from Agent and GetByTimePeriod - fromTime:{request.FromTime}, toTime:{request.ToTime}");

            var metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);

            var response = new NetworkMetricResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            return response;
        }
    }
}
