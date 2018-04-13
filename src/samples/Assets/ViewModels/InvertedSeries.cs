using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class InvertedSeries
    {
        public InvertedSeries()
        {
            var lineSeries = new LineSeries<double>();
            lineSeries.Add(5d);
            lineSeries.Add(3d);
            lineSeries.Add(6d);
            lineSeries.Add(4d);

            var barSeries = new BarSeries<double>();
            barSeries.Add(7d);
            barSeries.Add(8d);
            barSeries.Add(2d);
            barSeries.Add(8d);
            barSeries.DefaultFillOpacity = .6f;

            SeriesCollection = new ChartingCollection<Series>();
            SeriesCollection.Add(lineSeries);
            SeriesCollection.Add(barSeries);
        }

        public ChartingCollection<Series> SeriesCollection { get; set; }
    }
}
