using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;

namespace UWP.CartesianChart.MaterialCards
{
    /// <summary>
    /// Interaction logic for MaterialCards.xaml
    /// </summary>
    public partial class MaterialCards : Page, INotifyPropertyChanged
    {
        private double _lastLecture;
        private double _trend;
        private DispatcherTimer _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(500)};

        public MaterialCards()
        {
            InitializeComponent();

            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8),
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8)
                    }
                }
            };

            _trend = 8;

            _timer.Tick += (sender, o) =>
            {
                var r = new Random();

                _trend += (r.NextDouble() > 0.3 ? 1 : -1) * r.Next(0, 5);
                LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                LastHourSeries[0].Values.RemoveAt(0);
                SetLecture();
            };
            _timer.Start();

            Vals = new ChartValues<double> { 5, 9, 8, 6, 1, 5, 7, 3, 6, 3 };
            Nan = double.NaN;

            DataContext = this;
        }

        public SeriesCollection LastHourSeries { get; set; }
        public ChartValues<double> Vals { get; set; }
        public double Nan { get; set; }

        public double LastLecture
        {
            get { return _lastLecture; }
            set
            {
                _lastLecture = value;
                OnPropertyChanged("LastLecture");
            }
        }

        private async void SetLecture()
        {
            var target = ((ChartValues<ObservableValue>) LastHourSeries[0].Values).Last().Value;
            var step = (target - _lastLecture)/4;

            await Task.Delay(100);
            LastLecture += step;

            LastLecture = target;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            TimePowerChart.Update(true);
        }
    }
}
