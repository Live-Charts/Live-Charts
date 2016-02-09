using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using LiveCharts;
using LiveCharts.Annotations;
using LiveCharts.CoreComponents;

namespace ChartsTest.HighPerformance
{
    /// <summary>
    /// Interaction logic for HighPerformanceLine.xaml
    /// </summary>
    public partial class HighPerformanceLine : INotifyPropertyChanged
    {
        private DateTime _time;
        private ZoomingOptions _zoomingMode;

        public HighPerformanceLine()
        {
            InitializeComponent();

            //First you need to install LiveCharts.Optimizations
            //from Nuget:
            //Install-Package LiveCharts.Optimizations

            //LiveCharts.Optimization contains a class called ChartOptimizations where you can find 
            //algorithms according to your chart type, they also have a good description, indicating 
            //which is the best according to your case, using a wrong algorithm could not display data
            //to an optimized quality.

            //var algorithm = ChartOptimizations.Lines.RegularX<double>()
            //    // low quality is the default, but it is really accurate, it could fail only for +-3 pixels
            //    .WithQuality(DataQuality.Low);

            //create a configuration in this case we will use X as a zero based index,
            //and Y as the stored value in Series.Values
            //we will also attach the algorithm we just selected.
            var config = new SeriesConfiguration<double>()
                .X((val, index) => index)
                .Y(val => val);
                //.HasHighPerformanceMethod(algorithm);

            //create a SeriesCollection with this configuration
            Series = new SeriesCollection(config);

            //create a new line series
            var line = new LineSeries {Values = new ChartValues<double>()};


            //add some random values to test
            var r = new Random();
            var trend = 0d;

            for (var i = 0; i < 1000000; i++)
            {
                if (i%1000 == 0) trend += r.Next(-500, 500);
                line.Values.Add(trend + r.Next(-10, 10));
            }

            Series.Add(line);

            //some format
            var now = DateTime.Now.ToOADate();
            XFormat = val => DateTime.FromOADate(now + val/100).ToShortDateString();
            YFormat = val => Math.Round(val) + " ms";

            //set zooming if needed.
            ZoomingMode = ZoomingOptions.XY;

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }
        public Func<double, string> XFormat { get; set; }
        public Func<double, string> YFormat { get; set; }

        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                OnPropertyChanged();
            }
        }

        private void HighPerformanceLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is only to force animation everytime you change the current view.
            Chart.Update();
        }

        private void Chart_OnPlot(Chart obj)
        {
            //MessageBox.Show((DateTime.Now - _time).TotalMilliseconds.ToString("N0"));
        }

        private void XyOnClick(object sender, RoutedEventArgs e)
        {
            ZoomingMode = ZoomingOptions.XY;
        }

        private void XOnClick(object sender, RoutedEventArgs e)
        {
            ZoomingMode = ZoomingOptions.X;
        }

        private void YOnClick(object sender, RoutedEventArgs e)
        {
            ZoomingMode = ZoomingOptions.Y;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
