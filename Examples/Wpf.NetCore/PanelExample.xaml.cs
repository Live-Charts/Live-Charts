using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Wpf.Annotations;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for PanelExample.xaml
    /// </summary>
    public partial class PanelExample : UserControl
    {
        public PanelExample()
        {
            InitializeComponent();
        }
    }

    public class DarkPanelControlVm : INotifyPropertyChanged
    {
        private double _angularGaugeValue;
        private DispatcherTimer _timer = new DispatcherTimer();

        public DarkPanelControlVm()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += TimerOnTick;
            _timer.Start();

            AngularGaugeValue = 50;
            Yes = new ObservableValue(10);
            No = new ObservableValue(6);
            Maybe = new ObservableValue(4);
            Line1 = new ChartValues<ObservableValue>
            {
                new ObservableValue(3),
                new ObservableValue(5),
                new ObservableValue(1),
                new ObservableValue(6),
                new ObservableValue(8),
                new ObservableValue(3),
                new ObservableValue(6),
                new ObservableValue(3)
            };
            PieSeries = new SeriesCollection
            {
                new PieSeries {Title = "Yes",Values = new ChartValues<ObservableValue> {Yes}},
                new PieSeries {Title = "No", Values = new ChartValues<ObservableValue> {Maybe}},
                new PieSeries {Title = "Maybe", Values = new ChartValues<ObservableValue> {Maybe}}
            };

            GeoValues = new Dictionary<string, double>();

            var r = new Random();
            GeoValues["MX"] = r.Next(0, 100);
            GeoValues["RU"] = r.Next(0, 100);
            GeoValues["CA"] = r.Next(0, 100);
            GeoValues["US"] = r.Next(0, 100);
            GeoValues["IN"] = r.Next(0, 100);
            GeoValues["CN"] = r.Next(0, 100);
            GeoValues["JP"] = r.Next(0, 100);
            GeoValues["BR"] = r.Next(0, 100);
            GeoValues["DE"] = r.Next(0, 100);
            GeoValues["FR"] = r.Next(0, 100);
            GeoValues["GB"] = r.Next(0, 100);

            DynamicValues = new ChartValues<ObservableValue>
            {
                new ObservableValue(1),
                new ObservableValue(5),
                new ObservableValue(4),
                new ObservableValue(7),
                new ObservableValue(4),
                new ObservableValue(8)
            };

            Formatter = x => x.ToString("P");
        }

        public ChartValues<ObservableValue> Line1 { get; set; }
        public ChartValues<ObservableValue> DynamicValues { get; set; }
        public SeriesCollection PieSeries { get; set; }
        public ObservableValue Yes { get; set; }
        public ObservableValue No { get; set; }
        public ObservableValue Maybe { get; set; }
        public Dictionary<string, double> GeoValues { get; set; }
        public Func<double, string> Formatter { get; set; }

        public double AngularGaugeValue
        {
            get { return _angularGaugeValue; }
            set
            {
                _angularGaugeValue = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var r  = new Random();

            AngularGaugeValue += r.NextDouble() > 0.5 ? r.NextDouble()*10 : -r.NextDouble()*10;
            Yes.Value += r.NextDouble() > 0.5 ? r.NextDouble() * 1 : -r.NextDouble() * 1;
            No.Value += r.NextDouble() > 0.5 ? r.NextDouble() * 1 : -r.NextDouble() * 1;
            Maybe.Value += r.NextDouble() > 0.5 ? r.NextDouble() * 1 : -r.NextDouble() * 1;

            if (r.NextDouble() > .7)
            {
                foreach (var observableValue in Line1)
                {
                    observableValue.Value = r.NextDouble()*10;
                }
            }

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                DynamicValues.Add(new ObservableValue(r.NextDouble()*10));
                DynamicValues.RemoveAt(0);
                Thread.Sleep(500);
                DynamicValues.Add(new ObservableValue(r.NextDouble()*10));
                DynamicValues.RemoveAt(0);
            });
        }

        protected virtual void OnPropertyChanged( string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
