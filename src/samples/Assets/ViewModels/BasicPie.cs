using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class BasicPie
    {
        public BasicPie()
        {
            var chromeSeries = new PieSeries<double>();
            chromeSeries.Add(12d);

            var fireFoxSeries = new PieSeries<double>();
            fireFoxSeries.Add(8d);

            var explorerSeries = new PieSeries<double>();
            explorerSeries.Add(6d);

            // some custom style..
            explorerSeries.CornerRadius = 6;
            explorerSeries.PushOut = 10;

            // create a collection to store our series.
            SeriesCollection = new ChartingCollection<Series>();
            // add the series to our collection
            SeriesCollection.Add(chromeSeries);
            SeriesCollection.Add(fireFoxSeries);
            SeriesCollection.Add(explorerSeries);
            // we bind the SeriesCollection property to the PieChart.Series property in XAML
        }

        public ChartingCollection<Series> SeriesCollection { get; set; }
    }
}