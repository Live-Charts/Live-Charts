using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Wpf.Annotations;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for ZoomingAndPanning.xaml
    /// </summary>
    public partial class ZoomingAndPanning : INotifyPropertyChanged
    {
        private ZoomingOptions _zoomingMode;

        public ZoomingAndPanning()
        {
            InitializeComponent();

            var gradientBrush = new LinearGradientBrush {StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)};
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(33, 148, 241), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = GetData(),
                    Fill = gradientBrush,
                    StrokeThickness = 1,
                    PointGeometrySize = 0
                }
            };

            ZoomingMode = ZoomingOptions.X;
            MaxValue = DateTime.Now.Ticks;
            MinValue = DateTime.Now.Ticks - TimeSpan.FromDays(1).Ticks;
            XFormatter = val => new DateTime((long) val).ToString("dd MMM");
            YFormatter = val => val.ToString("C");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                OnPropertyChanged();
            }
        }

        private double _maxValue;
        private double _minValue;

        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }
        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                OnPropertyChanged("MinValue");
            }
        }

        private void ToogleZoomingMode(object sender, RoutedEventArgs e)
        {

            //switch (ZoomingMode)
            //{
            //    case ZoomingOptions.None:
            //        ZoomingMode = ZoomingOptions.X;
            //        break;
            //    case ZoomingOptions.X:
            //        ZoomingMode = ZoomingOptions.Y;
            //        break;
            //    case ZoomingOptions.Y:
            //        ZoomingMode = ZoomingOptions.Xy;
            //        break;
            //    case ZoomingOptions.Xy:
            //        ZoomingMode = ZoomingOptions.None;
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
            this.SeriesCollection[0].Values = GetData();
        }

        private ChartValues<DateTimePoint> GetData()
        {
            var r = new Random();
            var trend = 100;
            var values = new ChartValues<DateTimePoint>();

            for (var i = 0; i < 100; i++)
            {
                var seed = r.NextDouble();
                if (seed > .8) trend += seed > .9 ? 50 : -50;
                values.Add(new DateTimePoint(DateTime.Now.AddDays(i), trend + r.Next(0, 10)));
            }
            MinValue = values[0].DateTime.Ticks;
            MaxValue = values[values.Count-1].DateTime.Ticks;
            return values;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ZoomingModeCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ZoomingOptions) value)
            {
                case ZoomingOptions.None:
                    return "None";
                case ZoomingOptions.X:
                    return "X";
                case ZoomingOptions.Y:
                    return "Y";
                case ZoomingOptions.Xy:
                    return "XY";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
