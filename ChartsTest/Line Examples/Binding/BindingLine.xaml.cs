using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using lvc;

namespace ChartsTest.Line_Examples
{
    public partial class BindingLine
    {
        public WeatherViewModel ViewModel { get; set; }
        public BindingLine()
        {
            InitializeComponent();

            Func<int, DateTime> buildADate = x =>
            {
                return DateTime.Now + TimeSpan.FromDays(x);
            };

            var tok = new LineSeries
            {
                Values = new ChartValues<WeatherDay>()
                    .AddRange(new[]
                    {
                        new WeatherDay {DateTime = buildADate(-5), Temperature = 15},
                        new WeatherDay {DateTime = buildADate(-4), Temperature = 18},
                        new WeatherDay {DateTime = buildADate(-3), Temperature = 20},
                        new WeatherDay {DateTime = buildADate(-2), Temperature = 25},
                        new WeatherDay {DateTime = buildADate(-1), Temperature = 22},
                        new WeatherDay {DateTime = buildADate(0), Temperature = 19}
                    })
            };

            var series = new SeriesCollection<WeatherDay>
            {
                new LineSeries(),
                new LineSeries(),
                tok
            };

            var tokio = new ChartValues<WeatherDay>()
                .AddRange(new[]             // Add some initial values
                {
                    new WeatherDay {DateTime = buildADate(-5), Temperature = 15},
                    new WeatherDay {DateTime = buildADate(-4), Temperature = 18},
                    new WeatherDay {DateTime = buildADate(-3), Temperature = 20},
                    new WeatherDay {DateTime = buildADate(-2), Temperature = 25},
                    new WeatherDay {DateTime = buildADate(-1), Temperature = 22},
                    new WeatherDay {DateTime = buildADate(0), Temperature = 19}
                });
            var newYork = new ChartValues<WeatherDay>()
                .AddRange(new[]
                {
                    new WeatherDay {DateTime = buildADate(-5), Temperature = 9},
                    new WeatherDay {DateTime = buildADate(-4), Temperature = 13},
                    new WeatherDay {DateTime = buildADate(-3), Temperature = 15},
                    new WeatherDay {DateTime = buildADate(-2), Temperature = 16},
                    new WeatherDay {DateTime = buildADate(-1), Temperature = 15},
                    new WeatherDay {DateTime = buildADate(0), Temperature = 13}
                });

            ViewModel = new WeatherViewModel
            {
                Tokio = tokio,
                NewYork = newYork
            };

            DataContext = this;
        }

        private void CleanLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }

        private void GoWildOnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.GoWild();
        }

        private void AddValueOnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.AddValue();
        }

        private void RemoveValueOnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveValue();
        }
    }

    public class WeatherDay
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
    }

    public class WeatherViewModel
    {
        private Random _random = new Random();
        //live charts requires at least 100 ms without changes to update the chart
        //livecharts waits for the data to stop changing within 100ms, and then draws all the changes
        //if you set this timer interval to less than 100ms chart should not update because it will be waiting
        //for data to stop changing, this is to prevent multiple chart redraw.
        private DispatcherTimer _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(150)};
        private bool _isWild;

        public WeatherViewModel()
        {
            _timer.Tick += (sender, args) =>
            {
                Tokio.RemoveAt(0);
                Tokio.Add(new WeatherDay
                {
                    DateTime = Tokio.Last().DateTime + TimeSpan.FromDays(1),
                    Temperature = _random.Next(-10, 39)
                });
                NewYork.RemoveAt(0);
                NewYork.Add(new WeatherDay
                {
                    DateTime = NewYork.Last().DateTime + TimeSpan.FromDays(1),
                    Temperature = _random.Next(-10, 39)
                });
            };
        }

        public ChartValues<WeatherDay> Tokio { get; set; }
        public ChartValues<WeatherDay> NewYork { get; set; }

        public void AddValue()
        {
            Tokio.Add(new WeatherDay
            {
                DateTime = Tokio.Last().DateTime + TimeSpan.FromDays(1),
                Temperature = _random.Next(-10, 39)
            });
            NewYork.Add(new WeatherDay
            {
                DateTime = Tokio.Last().DateTime + TimeSpan.FromDays(1),
                Temperature = _random.Next(-10, 39)
            });
        }

        public void RemoveValue()
        {
            if (Tokio.Count < 3) return;
            Tokio.RemoveAt(0);
            NewYork.RemoveAt(0);
        }

        public void GoWild()
        {
            if (!_isWild)
            {
                _timer.Start();
                _isWild = true;
            }
            else
            {
                _timer.Stop();
                _isWild = false;
            }
        }
    }
}
