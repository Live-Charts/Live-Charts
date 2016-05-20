using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for MixingSeries.xaml
    /// </summary>
    public partial class MixingTypes : UserControl
    {
        public MixingTypes()
        {
            InitializeComponent();

            LineSeries = new LineSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(5),
                    new ObservableValue(7),
                    new ObservableValue(2),
                    new ObservableValue(3),
                },
                Fill = Brushes.Transparent
            };

            BubbleSeries = new BubbleSeries
            {
                Values = new ChartValues<BubblePoint>
                {
                    new BubblePoint(0, 2, 10),
                    new BubblePoint(1, 1, 2),
                    new BubblePoint(2, 3, 7),
                    new BubblePoint(3, 4, 9)
                }
            };

            ColumnSeries = new ColumnSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(5),
                    new ObservableValue(7),
                    new ObservableValue(2),
                    new ObservableValue(3),
                }
            };

            SeriesCollection = new SeriesCollection
            {
                LineSeries,
                BubbleSeries,
                ColumnSeries
            };

            DataContext = this;
        }

        public LiveCharts.Wpf.BubbleSeries BubbleSeries { get; set; }
        public LineSeries LineSeries { get; set; }
        public ColumnSeries ColumnSeries { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        private void OnLinkRequest(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var value in LineSeries.Values.Cast<ObservableValue>())
            {
                value.Value = r.Next(-20, 20);
            }
            foreach (var value in ColumnSeries.Values.Cast<ObservableValue>())
            {
                value.Value = r.Next(-20, 20);
            }
            var i = 0;
            foreach (var value in BubbleSeries.Values.Cast<BubblePoint>())
            {
                value.X = i;
                value.Y = r.Next(-20, 20);
                value.Weight = r.Next(-20, 20);
                i++;
            }
        }
    }
}
