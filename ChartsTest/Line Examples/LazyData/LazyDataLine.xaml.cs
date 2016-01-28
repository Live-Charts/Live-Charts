using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Annotations;
using LiveCharts.CoreComponents;

namespace ChartsTest.Line_Examples
{
    /// <summary>
    /// Interaction logic for LazyDataLine.xaml
    /// </summary>
    public partial class LazyDataLine
    {
        public LazyDataLine()
        {
            InitializeComponent();
            Charts = new ObservableCollection<ChartViewModel>();
            DataContext = this;

            Charts.Add(new ChartViewModel());
            Charts.Add(new ChartViewModel());
        }

        public ObservableCollection<ChartViewModel> Charts { get; set; }

        private void AddElementOnClick(object sender, RoutedEventArgs e)
        {
            Charts.Add(new ChartViewModel());
        }

        private void GetDataOnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var model = fe.DataContext as ChartViewModel;
            if (model == null) return;
            model.GetNewData();
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var model = fe.DataContext as ChartViewModel;
            if (model == null) return;
            model.AddSeries();
        }

        private void RemoveLastSeriesOnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var model = fe.DataContext as ChartViewModel;
            if (model == null) return;
            model.RemoveSeries();
        }

        private void RealTimeOnClick(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var model = fe.DataContext as ChartViewModel;
            if (model == null) return;
            model.GoWild();
        }
    }


    public class ChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Series> _series;
        private string _name;
        private static Random _random = new Random();
        //live charts requires at least 100 ms without changes to update the chart
        //livecharts waits for the data to stop changing within 100ms, and then draws all the changes
        //if you set this timer interval to less than 100ms chart should not update because it will be waiting
        //for data to stop changing, this is to prevent multiple chart redraw.
        private DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) };
        private bool _isWild;

        public ChartViewModel()
        {
            Labels = new List<string>();
            Series = new SeriesCollection().Setup(new SeriesConfiguration<double>().Y(y => y));
            _timer.Tick += (sender, args) =>
            {
                foreach (var series in Series)
                {
                    series.Values.RemoveAt(0);
                    series.Values.Add((double) _random.Next(-10, 10));
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //Charts Update When you add/remove a serie from Series collection or when you add/remove a value inside each serie.
        public ObservableCollection<Series> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        //labels do not fire any update, it is not necesary to use Observable collection.
        public List<string> Labels { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public void GetNewData()
        {
            Name = GetRandomName();
            Labels.Clear();
            var randomStartDay = _random.Next(1, 10);
            for (int i = 0; i < 10; i++) Labels.Add("Day " + (randomStartDay +i));
            foreach (var series in Series.ToArray()) Series.Remove(series);
            Series.Add(new LineSeries
            {
                Title = "Serie 1",
                Values = BuildRandomValues(),
                PointRadius = 0
            });
            Series.Add(new LineSeries
            {
                Title = "Serie 2",
                Values = BuildRandomValues(),
                PointRadius = 0
            });
        }

        public void AddSeries()
        {
            Series.Add(new LineSeries
            {
                Title = "Series " + Series.Count + 1,
                Values = BuildRandomValues(),
                PointRadius = 0
            });
        }

        public void RemoveSeries()
        {
            if (Series.Count < 2) return;
            Series.RemoveAt(0);
        }

        public void GoWild()
        {
            if (Series.Count < 1) return;

            if (_isWild)
            {
                _isWild = false;
                _timer.Stop();
            }
            else
            {
                _isWild = true;
                _timer.Start();
            }
        }

        private static ChartValues<double> BuildRandomValues()
        {
            var serie = new ChartValues<double>();
            for (int i = 0; i < 10; i++) serie.Add(_random.Next(-10,10));
            return serie;
        }

        private static string GetRandomName()
        {
            var names = new[] {"Pablo", "Vicent", "Andy", "Salvador", "Leonardo", "Jackson", "Claude", "Miguel", "Henri", "Frida", "Donatello" };
            return names[_random.Next(0, names.Length - 1)];
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
