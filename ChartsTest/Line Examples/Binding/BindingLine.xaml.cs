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

            ViewModel = new WeatherViewModel
            {
                Tokio = new ChartValues<double> {15, 25, 29, 32, 16, 10},
                NewYork = new ChartValues<double> {12, 10, 9, 8, 5, -10 }
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
                Tokio.Add(_random.Next(-10, 39));
                NewYork.RemoveAt(0);
                NewYork.Add(_random.Next(-10, 39));
            };
        }

        public ChartValues<double> Tokio { get; set; }
        public ChartValues<double> NewYork { get; set; }

        public void AddValue()
        {
            Tokio.Add(_random.Next(-10, 39));
            NewYork.Add(_random.Next(-10, 39));
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
