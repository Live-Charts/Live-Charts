using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Financial
{
    /// <summary>
    /// Interaction logic for CandleStickExample.xaml
    /// </summary>
    public partial class CandleStickExample : UserControl, INotifyPropertyChanged
    {
        private string[] _labels;

        public CandleStickExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new CandleSeries
                {
                    Values = new ChartValues<OhlcPoint>
                    {
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33),
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33),
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33),
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33),
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33)
                    }
                }
            };

            //based on https://github.com/beto-rodriguez/Live-Charts/issues/166 
            //The Ohcl point X property is zero based indexed.
            //this means the first point is 0, second 1, third 2.... and so on
            //then you can use the Axis.Labels properties to map the chart X with a label in the array.
            //for more info see (mapped labels section) 
            //http://lvcharts.net/#/examples/v1/labels-wpf?path=WPF-Components-Labels

            Labels = new[]
            {
                DateTime.Now.ToString("dd MMM"),
                DateTime.Now.AddDays(1).ToString("dd MMM"),
                DateTime.Now.AddDays(2).ToString("dd MMM"),
                DateTime.Now.AddDays(3).ToString("dd MMM"),
                DateTime.Now.AddDays(4).ToString("dd MMM"),
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged("Labels");
            }
        }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var point in SeriesCollection[0].Values.Cast<OhlcPoint>())
            {
                point.Open = r.Next((int)point.Low, (int)point.High);
                point.Close = r.Next((int)point.Low, (int)point.High);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
