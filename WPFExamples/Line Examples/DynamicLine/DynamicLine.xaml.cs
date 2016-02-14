using System;
using System.Windows;
using System.Windows.Threading;
using LiveCharts;

namespace ChartsTest.Line_Examples.DynamicLine
{
    /// <summary>
    /// Interaction logic for DynamicLine.xaml
    /// </summary>
    public partial class DynamicLine
    {
        private Random _r = new Random();
        private bool _isAlive;
        private readonly DispatcherTimer _timer;
        private DateTime _currentDate = DateTime.Now;

        public DynamicLine()
        {
            InitializeComponent();

            //In this case we will not only plot double values
            //to make it easier to handle "live-data" we are going to use WeatherViewModel class
            //we need to let LiveCharts know how to use this model

            //first we create a new configuration for WeatherViewModel
            var config = new SeriesConfiguration<WeatherViewModel>();

            //now we map X and Y
            //we will use Temperature as Y
            config.Y(model => model.Temperature);
            //and DateTime as X, we convert to OADate so we can plot it easly.
            config.X(model => model.DateTime.ToOADate());

            //now we create our series with this configuration
            Series = new SeriesCollection(config) {new LineSeries {Values = new ChartValues<WeatherViewModel>()}};

            //to display a custom label we will use a formatter,
            //formatters are just functions that take a double value as parameter
            //and return a string, in this case we will convert the OADate to DateTime
            //and then use a custom date format
            XFormatter = val => DateTime.FromOADate(val).ToString("hh:mm:ss tt");
            //now for Y we are rounding and adding ° for degrees
            YFormatter = val => Math.Round(val) + " °";

            //Don't forget DataContext so we can bind these properties.
            DataContext = this;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += TimerOnTick;
        }

        public SeriesCollection Series { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        private void LiveDateOnClick(object sender, RoutedEventArgs e)
        {
            if (_isAlive)
            {
                _isAlive = false;
                _timer.Stop();
            }
            else
            {
                _isAlive = true;
                _timer.Start();
            }
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _currentDate = DateTime.Now;

            foreach (var series in Series)
            {
                if (series.Values.Count > 10) series.Values.RemoveAt(0);
                series.Values.Add(new WeatherViewModel
                {
                    Temperature = _r.NextDouble()*30,
                    DateTime = _currentDate
                });
            }
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            Series.Add(new LineSeries {Values = new ChartValues<WeatherViewModel>()});
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            if (Series.Count > 0) Series.RemoveAt(0);
        }
    }
}
