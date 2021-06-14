using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using AutoMapper;
using MetricsManager.Responses;
using MyNamespace;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<MetricsAgentClient> _logger;

        private readonly IMapper _mapper;

        public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger, IMapper mapper)
        {
            _httpClient = httpClient;

            _logger = logger;

            _mapper = mapper;
        }
        
        public CpuMetricsApiResponse GetCpuMetrics(CpuMetricsApiRequest request)
        {
            var requestClient = new MyNamespace.Client(request.ClientBaseAddress, _httpClient);

            try
            {
                CpuMetricsResponse response = requestClient
                    .ApiMetricsAgentCpuGetbytimeperiod(new CpuMetricGetByTimePeriodRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime
                    });
                
                return _mapper.Map<CpuMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public DotNetMetricsApiResponse GetDotNetMetrics(DotNetMetricsApiRequest request)
        {
            var requestClient = new MyNamespace.Client(request.ClientBaseAddress, _httpClient);

            try
            {
                DotNetMetricResponse response = requestClient
                    .ApiMetricsAgentDotnetGetbytimeperiod(new DotNetMetricGetByTimePeriodRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime
                    });

                return _mapper.Map<DotNetMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public HddMetricsApiResponse GetHddMetrics(HddMetricsApiRequest request)
        {
            var requestClient = new MyNamespace.Client(request.ClientBaseAddress, _httpClient);

            try
            {
                HddMetricResponse response = requestClient
                    .ApiMetricsAgentHddGetbytimeperiod(new HddMetricGetByTimePeriodRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime
                    });

                return _mapper.Map<HddMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public NetworkMetricsApiResponse GetNetworkMetrics(NetworkMetricsApiRequest request)
        {
            var requestClient = new MyNamespace.Client(request.ClientBaseAddress, _httpClient);

            try
            {
                NetworkMetricResponse response = requestClient
                    .ApiMetricsAgentNetworkGetbytimeperiod(new NetworkMetricGetByTimePeriodRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime
                    });

                return _mapper.Map<NetworkMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public RamMetricsApiResponse GetRamMetrics(RamMetricsApiRequest request)
        {
            var requestClient = new MyNamespace.Client(request.ClientBaseAddress, _httpClient);

            try
            {
                RamMetricsResponse response = requestClient
                    .ApiMetricsAgentRamGetbytimeperiod(new RamMetricGetByTimePeriodRequest()
                    {
                        FromTime = request.FromTime,

                        ToTime = request.ToTime
                    });

                return _mapper.Map<RamMetricsApiResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}