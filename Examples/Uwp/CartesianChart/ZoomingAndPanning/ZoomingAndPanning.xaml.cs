using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System;
using System.ComponentModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.ZoomingAndPanning
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ZoomingAndPanning : Page, INotifyPropertyChanged
    {
        private ZoomingOptions _zoomingMode;

        public ZoomingAndPanning()
        {
            InitializeComponent();

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop {Color = Color.FromArgb(255, 33, 148, 241), Offset = 0});
            gradientBrush.GradientStops.Add(new GradientStop {Color = Colors.Transparent, Offset = 1});

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

            XFormatter = val => new DateTime((long)val).ToString("dd MMM");
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

        private void ToogleZoomingMode(object sender, RoutedEventArgs e)
        {
            switch (ZoomingMode)
            {
                case ZoomingOptions.None:
                    ZoomingMode = ZoomingOptions.X;
                    break;
                case ZoomingOptions.X:
                    ZoomingMode = ZoomingOptions.Y;
                    break;
                case ZoomingOptions.Y:
                    ZoomingMode = ZoomingOptions.Xy;
                    break;
                case ZoomingOptions.Xy:
                    ZoomingMode = ZoomingOptions.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

            return values;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            //Use the axis MinValue/MaxValue properties to specify the values to display.
            //use double.Nan to clear it.

            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
            Y.MinValue = double.NaN;
            Y.MaxValue = double.NaN;
        }
    }

    public class ZoomingModeCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((ZoomingOptions)value)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
