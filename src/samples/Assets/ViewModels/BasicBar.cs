using System.Drawing;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class BasicBar
    {
        public BasicBar()
        {
            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<Series>();

            var barSeries = new BarSeries<double>();

            // add some values to the series...
            barSeries.Add(-4);
            barSeries.Add(-7);
            barSeries.Add(-2);
            barSeries.Add(9);

            // a custom fill and stroke, if we don't set these properties
            // LiveCharts will set them for us according to our theme.
            barSeries.StrokeThickness = 3f;
            barSeries.Stroke = Color.Purple;
            barSeries.Fill = Color.MediumPurple;

            // limit the column width to 65.
            barSeries.MaxColumnWidth = 65f;

            // do not display a label for every point
            barSeries.DataLabels = false;

            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(barSeries);
        }

        public ChartingCollection<Series> SeriesCollection { get; set; }
    }
}