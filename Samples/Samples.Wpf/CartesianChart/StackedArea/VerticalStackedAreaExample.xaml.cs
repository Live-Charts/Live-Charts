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

namespace Wpf.CartesianChart.StackedArea
{
    /// <summary>
    /// Interaction logic for VerticalStackedAreaExample.xaml
    /// </summary>
    public partial class VerticalStackedAreaExample : UserControl
    {

        public VerticalStackedAreaExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new VerticalStackedAreaSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(5),
                        new ObservableValue(3),
                        new ObservableValue(6),
                        new ObservableValue(2)
                    },
                    DataLabels = true
                },
                new VerticalStackedAreaSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(4),
                        new ObservableValue(1),
                        new ObservableValue(7),
                        new ObservableValue(9)
                    },
                    DataLabels = true
                },
                new VerticalStackedAreaSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(2),
                        new ObservableValue(8),
                        new ObservableValue(2),
                        new ObservableValue(9)
                    },
                    DataLabels = true
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public StackMode StackMode { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                foreach (var observable in series.Values.Cast<ObservableValue>())
                {
                    observable.Value = r.Next(1, 10);
                }
            }
        }

        private void ChangeStackModeOnClick(object sender, RoutedEventArgs e)
        {
            foreach (var series in SeriesCollection.Cast<VerticalStackedAreaSeries>())
            {
                series.StackMode = series.StackMode == StackMode.Percentage
                    ? StackMode.Values
                    : StackMode.Percentage;
            }
        }
    }
}
