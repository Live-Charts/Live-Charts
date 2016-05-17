using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.PieChart
{
    /// <summary>
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class PieExample
    {
        public PieExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue(0) }
                },
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue(0) }
                },
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue(0) }
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                foreach (var observable in series.Values.Cast<ObservableValue>())
                {
                    observable.Value = r.Next(0, 10);
                }
            }
        }
    }
}
