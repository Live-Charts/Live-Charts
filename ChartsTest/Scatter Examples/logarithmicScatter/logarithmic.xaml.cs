using System;
using System.Drawing;
using LiveCharts;

namespace ChartsTest.Scatter_Examples.logarithmicScatter
{
    /// <summary>
    /// Interaction logic for Logaritmic.xaml
    /// </summary>
    public partial class Logarithmic
    {
        public Logarithmic()
        {
            InitializeComponent();

            var map = new SeriesConfiguration<Point>()
                .X(point => Math.Log(point.X, 10)) // Map X value to Log(X)
                .Y(point => point.Y);              // Use Y without Log

            Series = new SeriesCollection(map) 
            {
                new ScatterSeries
                {
                    Values = new[]
                    {
                        new Point(1, 100), new Point(10, 100), new Point(1000, 300), new Point(10000, 400),
                        new Point(100000, 500), new Point(1000000, 600)
                    }.AsChartValues()
                }
            };

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }
    }
}
