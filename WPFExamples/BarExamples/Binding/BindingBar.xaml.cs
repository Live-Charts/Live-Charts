using System;
using System.Windows;
using System.Windows.Threading;
using LiveCharts;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for BindingBar.xaml
    /// </summary>
    public partial class BindingBar
    {
        public BindingBar()
        {
            InitializeComponent();
            ViewModel = new BindedBarsViewModel
            {
                Series1 = new ChartValues<double> { 2, 3, 5, 7, 2, 3, 5, 7, 2, 3, 5, 7 },
                Series2 = new ChartValues<double> { 7, 3, 4, 1, 7, 3, 4, 1, 7, 3, 4, 1 }
            };
            DataContext = this;
        }

        public BindedBarsViewModel ViewModel { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Redraw();
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

    public class BindedBarsViewModel
    {
        private Random _random = new Random();
        //live charts requires at least 100 ms without changes to update the chart
        //livecharts waits for the data to stop changing within 100ms, and then draws all the changes
        //if you set this timer interval to less than 100ms chart should not update because it will be waiting
        //for data to stop changing, this is to prevent multiple chart redraw.
        private DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
        private bool _isWild;

        public BindedBarsViewModel()
        {
            _timer.Tick += (sender, args) =>
            {
                Series1.RemoveAt(0);
                Series1.Add(_random.Next(-50, 50));
                Series2.RemoveAt(0);
                Series2.Add(_random.Next(-10, 39));
            };
        }

        public ChartValues<double> Series1 { get; set; }
        public ChartValues<double> Series2 { get; set; }

        public void AddValue()
        {
            Series1.Add(_random.Next(-10, 39));
            Series2.Add(_random.Next(-10, 39));
        }

        public void RemoveValue()
        {
            if (Series1.Count < 3) return;
            Series1.RemoveAt(0);
            Series2.RemoveAt(0);
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
