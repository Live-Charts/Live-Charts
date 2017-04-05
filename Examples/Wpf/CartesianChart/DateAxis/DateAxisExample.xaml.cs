using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.DateAxis
{
    public partial class DateAxisExample : UserControl, INotifyPropertyChanged
    {
        private DateTime _referenceDateTime;

        private SeriesResolution _seriesResolution = SeriesResolution.Day;

        public DateAxisExample()
        {
            InitializeComponent();

            var now = DateTime.UtcNow;
            ReferenceDateTime = new DateTime(now.Year, now.Month, now.Day);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,7, 8, 2, 0, 3, 5, 2, 4, 6, 4, 7, 3, 10, 4, 1, 2, 5, 8, 4, 6, 2, 4, 8, 7, 5, 4, 3, 2, 5, 6, 5, 3, 6, 4, 6, 3, 4, 1, 4, 2, 3, 2, 3, 5, 8, 6, 8, 4, 2, 4, 1, 2, 5, 6, 4, 6, 5, 2 ,7, 8, 2, 0, 3, 5, 2, 4, 6, 4, 7, 3, 10, 4, 1, 2, 5, 8, 4, 6, 2, 4, 8, 7, 5, 4, 3, 2, 5, 6, 5, 3, 6, 4, 6, 3, 4, 1, 4, 2, 3, 2, 3, 5, 8, 6, 8, 4, 2, 4, 1, 2, 5, 6  },
                    PointGeometry = null
                },                
            };
       
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        public DateTime ReferenceDateTime
        {
            get { return _referenceDateTime; }
            set
            {
                _referenceDateTime = value;
                OnPropertyChanged("ReferenceDateTime");
            }
        }

        public SeriesResolution SeriesResolution
        {
            get { return _seriesResolution; }
            set
            {
                _seriesResolution = value;
                OnPropertyChanged("SeriesResolution");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetDayResolution(object sender, RoutedEventArgs e)
        {
            SeriesResolution = SeriesResolution.Day;
        }

        private void SetHourResolution(object sender, RoutedEventArgs e)
        {
            SeriesResolution = SeriesResolution.Hour;
        }

        private void SetMinuteResolution(object sender, RoutedEventArgs e)
        {
            SeriesResolution = SeriesResolution.Minute;
        }

        private void SetSecondResolution(object sender, RoutedEventArgs e)
        {
            SeriesResolution = SeriesResolution.Second;
        }
    }
}
