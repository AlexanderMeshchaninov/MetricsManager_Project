using System;
using MetricsManagerClient.DAL.Interface;
using System.Net.Http;
using AutoMapper;
using MetricsManagerGeneratedClient;
using Microsoft.Extensions.Logging;

namespace MetricsManagerClient.Client
{
    class ManagerClient : IClient
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<ManagerClient> _logger;

        private readonly IMapper _mapper;

        public ManagerClient(HttpClient httpClient, ILogger<ManagerClient> logger, IMapper mapper)
        {
            _httpClient = httpClient;

            _logger = logger;

            _mapper = mapper;
        }

        public Responses.CpuMetricsApiResponse GetAllCpuMetrics(Requests.CpuMetricsApiRequest request)
        {
            var requestClient = new ManagerGeneratedClient(request.ClientBaseAddress, _httpClient);

            try
            {
                var response = requestClient
                    .ApiMetricsManagerCpuGetmetricsfromagent(new CpuMetricsApiRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime,
                    });

                return _mapper.Map<Responses.CpuMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public Responses.HddMetricsApiResponse GetAllHddMetrics(Requests.HddMetricsApiRequest request)
        {
            var requestClient = new ManagerGeneratedClient(request.ClientBaseAddress, _httpClient);

            try
            {
                var response = requestClient
                    .ApiMetricsManagerHddGetmetricsfromagent(new HddMetricsApiRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime,
                    });

                return _mapper.Map<Responses.HddMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public Responses.RamMetricsApiResponse GetAllRamMetrics(Requests.RamMetricsApiRequest request)
        {
            var requestClient = new ManagerGeneratedClient(request.ClientBaseAddress, _httpClient);

            try
            {
                var response = requestClient
                    .ApiMetricsManagerRamGetmetricsfromagent(new RamMetricsApiRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime,
                    });

                return _mapper.Map<Responses.RamMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
