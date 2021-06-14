using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/agent/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;

        private readonly ICpuMetricsRepository _repository;

        private readonly IMapper _mapper;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, ICpuMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into CpuMetricsController");

            _repository = repository;

            _mapper = mapper;
        }

        /// <summary>
        /// Контроллер получает метрики Cpu принимая на вход запрос
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /api/metrics/agent/cpu/getbytimeperiod
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
        public CpuMetricsResponse GetByTimePeriod([FromBody] CpuMetricGetByTimePeriodRequest request)
        {
            _logger.LogInformation(1, 
                $"This log from Agent and GetByTimePeriod - fromTime:{request.FromTime}, toTime:{request.ToTime}");

            var metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);

            var response = new CpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            return response;
        }
    }
}
