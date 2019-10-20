using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.zzIssues
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Issue179 : UserControl
    {
        public Issue179()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(1),
                        new ObservableValue(6),
                        new ObservableValue(4),
                        new ObservableValue(7),
                        new ObservableValue(3)
                    }
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(8)
                    }
                }
            };

            Task.Factory.StartNew(() =>
            {
                var r = new Random();

                while (true)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (var series in Series)
                        {
                            series.Values.Add(new ObservableValue(r.Next(0, 10)));
                        }
                    }));

                    Thread.Sleep(1500);
                }
            });

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //ChartValues.Clear();
        }
    }
}
