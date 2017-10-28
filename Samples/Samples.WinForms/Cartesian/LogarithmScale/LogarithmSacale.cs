using System;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.LogarithmScale
{
    public partial class LogarithmSacale : Form
    {
        public LogarithmSacale()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection(Mappers.Xy<ObservablePoint>()
                .X(point => Math.Log10(point.X))
                .Y(point => point.Y))
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

            cartesianChart1.AxisX.Add(new LogarithmicAxis
            {
                LabelFormatter = value => Math.Pow(10, value).ToString("N"),
                Base = 10,
                Separator = new Separator
                {
                    Stroke = Brushes.LightGray
                }
            });

        }
    }
}
