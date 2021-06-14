using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for CpuChart.xaml
    /// </summary>
    public partial class CpuChart : UserControl, INotifyPropertyChanged
    {
        public CpuChart()
        {
            InitializeComponent();

            ColumnSeriesValues = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<int> { 10,20,30,40,50,60,70,80,90,100 }
                }
            };

            DataContext = this;
        }

        public SeriesCollection ColumnSeriesValues { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdateOnСlick(object sender, RoutedEventArgs e)
        {
            TimePowerChart.Update(true);
        }
    }
}
