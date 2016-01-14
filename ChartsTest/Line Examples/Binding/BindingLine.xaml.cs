using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace ChartsTest.Line_Examples
{
    public partial class BindingLine
    {
        public BindingLineViewModel ViewModel { get; set; }
        public BindingLine()
        {
            InitializeComponent();
            ViewModel = new BindingLineViewModel
            {
                FirstSeries = new ObservableCollection<double> { 2, 4, double.NaN, 7, 8, 6, 2, 4, 2, 5 },
                SecondSeries = new ObservableCollection<double> { 7, 3, 4, 1, 5, 6, 8, 5, 1, 3 }
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

    public class BindingLineViewModel
    {
        private Random _random = new Random();
        private DispatcherTimer _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(500)};
        private bool _isWild;

        public BindingLineViewModel()
        {
            _timer.Tick += (sender, args) =>
            {
                var r = _random.NextDouble();
                FirstSeries.RemoveAt(0);
                FirstSeries.Add(r > 0.95 ? double.NaN : _random.Next(1, 10));
                SecondSeries.RemoveAt(0);
                SecondSeries.Add(r > 0.95 ? double.NaN : _random.Next(1, 10));
            };
        }

        public ObservableCollection<double> FirstSeries { get; set; }
        public ObservableCollection<double> SecondSeries { get; set; }

        public void AddValue()
        {
            FirstSeries.Add(_random.Next(1,10));
            SecondSeries.Add(_random.Next(1,10));
        }

        public void RemoveValue()
        {
            if (FirstSeries.Count < 3) return;
            FirstSeries.RemoveAt(0);
            SecondSeries.RemoveAt(0);
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
