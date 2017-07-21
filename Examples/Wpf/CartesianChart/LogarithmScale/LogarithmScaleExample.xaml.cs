using System;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.LogarithmScale
{
    public partial class LogarithmScaleExample : UserControl
    {
        public LogarithmScaleExample()
        {
            InitializeComponent();

            Base = 10;

            var mapper = Mappers.Xy<ObservablePoint>()
                .X(point => Math.Log(point.X, Base)) //a 10 base log scale in the X axis
                .Y(point => point.Y);

            SeriesCollection =  new SeriesCollection(mapper)
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(1, 5),
                        new ObservablePoint(10, 6),
                        new ObservablePoint(100, 4),
                        new ObservablePoint(1000, 2),
                        new ObservablePoint(10000, 8),
                        new ObservablePoint(100000, 2),
                        new ObservablePoint(1000000, 9),
                        new ObservablePoint(10000000, 8)
                    }
                }
            };

            Formatter = value => Math.Pow(Base, value).ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        public double Base { get; set; }

    }
}
