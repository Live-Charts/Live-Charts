using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.zzIssues
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Issue179 : Page
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

            Task.Factory.StartNew(async () =>
            {
                var r = new Random();

                while (true)
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                     {
                         foreach (var series in Series)
                         {
                             series.Values.Add(new ObservableValue(r.Next(0, 10)));
                         }
                     });

                    await Task.Delay(1500);
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
