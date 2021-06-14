using System;
using System.Windows;
using MetricsManagerClient.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger;

        private IServiceProvider _serviceProvider;

        private IGetAllCpuMetricsFromManager _allCpuMetrics;
        private IGetAllHddMetricsFromManager _allHddMetrics;
        private IGetAllRamMetricsFromManager _allRamMetrics;
        public MainWindow(ILogger<MainWindow> logger, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _logger = logger;

            _serviceProvider = serviceProvider;

            _allCpuMetrics = _serviceProvider.GetService<IGetAllCpuMetricsFromManager>();
            _allRamMetrics = _serviceProvider.GetService<IGetAllRamMetricsFromManager>();
            _allHddMetrics = _serviceProvider.GetService<IGetAllHddMetricsFromManager>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CpuChart.ColumnSeriesValues[0].Values.Clear();
            foreach (var cpuMetrics in _allCpuMetrics.ReadAllMetrics())
            {
                CpuChart.ColumnSeriesValues[0].Values.Add(cpuMetrics.Value);
            }
            _allCpuMetrics.DeleteMetrics();

            RamChart.ColumnSeriesValues[0].Values.Clear();
            foreach (var ramMetrics in _allRamMetrics.ReadAllMetrics())
            {
                RamChart.ColumnSeriesValues[0].Values.Add(ramMetrics.Value);
            }
            _allRamMetrics.DeleteMetrics();

            HddChart.ColumnSeriesValues[0].Values.Clear();
            foreach (var hddMetrics in _allHddMetrics.ReadAllMetrics())
            {
                HddChart.ColumnSeriesValues[0].Values.Add(hddMetrics.Value);
            }
            _allHddMetrics.DeleteMetrics();

            _logger.LogDebug("Metrics has been updated");
        }
    }
}