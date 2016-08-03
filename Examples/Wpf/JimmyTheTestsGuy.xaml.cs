using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Wpf.Annotations;

namespace Wpf
{
   
    //This is Jimmy, be rude with him.

    public partial class JimmyTheTestsGuy : INotifyPropertyChanged
    {
        public JimmyTheTestsGuy()
        {
            InitializeComponent();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ((TestVm) DataContext).Load(); 
        }
    }

    public class TestVm : INotifyPropertyChanged
    {
        private SeriesCollection _chartSeries;

        public TestVm()
        {
            ChartSeries = new SeriesCollection();
            Series = new ObservableCollection<Series>();
        }

        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        public SeriesCollection ChartSeries
        {
            get { return _chartSeries; }
            set
            {
                _chartSeries = value;
                OnPropertyChanged("ChartSeries");
            }
        }

        public ObservableCollection<Series> Series { get; set; } 

        public void Load()
        {
            //ChartSeries.Clear();
            //Series.Clear();

            var series = new SeriesCollection();

            var r = new Random();

            for (var i = 0; i < 3; i++)
            {
                var s = new LineSeries {Title =  "Series" + i, Values = new ChartValues<ObservableValue>()};
                for (var j = 0; j < 10; j++)
                {
                    s.Values.Add(new ObservableValue(r.Next(0, 10)));
                }
                series.Add(s);
                //series.Add(s);
            }

            ChartSeries = series;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class VisibilityToBooleanConverter : IValueConverter
    {
        public VisibilityToBooleanConverter()
        {
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility) value == Visibility.Visible ? true : false;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value == true ? Visibility.Visible : Visibility.Hidden;
        }
    }
}


