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
        private DateTime _initialDateTime;

        private PeriodUnits _period = PeriodUnits.Days;
        private IAxisWindow _selectedWindow;

        public DateAxisExample()
        {
            InitializeComponent();

            var now = DateTime.UtcNow;
            InitialDateTime = new DateTime(now.Year, now.Month, now.Day);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,7, 8, 2, 0, 3, 5, 2, 4, 6, 4, 7, 3, 10, 4, 1, 2, 5, 8, 4, 6, 2, 4, 8, 7, 5, 4, 3, 2, 5, 6, 5, 3, 6, 4, 6, 3, 4, 1, 4, 2, 3, 2, 3, 5, 8, 6, 8, 4, 2, 4, 1, 2, 5, 6, 4, 6, 5, 2 ,7, 8, 2, 0, 3, 5, 2, 4, 6, 4, 7, 3, 10, 4, 1, 2, 5, 8, 4, 6, 2, 4, 8, 7, 5, 4, 3, 2, 5, 6, 5, 3, 6, 4, 6, 3, 4, 1, 4, 2, 3, 2, 3, 5, 8, 6, 8, 4, 2, 4, 1, 2, 5, 6  },
                },                
            };
       
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        public DateTime InitialDateTime
        {
            get { return _initialDateTime; }
            set
            {
                _initialDateTime = value;
                OnPropertyChanged("InitialDateTime");
            }
        }

        public PeriodUnits Period
        {
            get { return _period; }
            set
            {
                _period = value;
                OnPropertyChanged("Period");
            }
        }

        public IAxisWindow SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                _selectedWindow = value;
                OnPropertyChanged("SelectedWindow");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetDayPeriod(object sender, RoutedEventArgs e)
        {
            Period = PeriodUnits.Days;
        }

        private void SetHourPeriod(object sender, RoutedEventArgs e)
        {
            Period = PeriodUnits.Hours;
        }

        private void SetMinutePeriod(object sender, RoutedEventArgs e)
        {
            Period = PeriodUnits.Minutes;
        }

        private void SetSecondPeriod(object sender, RoutedEventArgs e)
        {
            Period = PeriodUnits.Seconds;
        }

        private void SetMilliSecondPeriod(object sender, RoutedEventArgs e)
        {
            Period = PeriodUnits.Milliseconds;
        }
    }
}
