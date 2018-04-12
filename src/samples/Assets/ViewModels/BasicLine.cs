using System.Drawing;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Svg;

namespace Assets.ViewModels
{
    public class BasicLine
    {
        public BasicLine()
        {
            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<Series>();

            // create a new line series
            var lineSeries = new LineSeries<double>();

            // add some values to the series...
            lineSeries.Add(4);
            lineSeries.Add(-7);
            lineSeries.Add(-2);
            lineSeries.Add(9);

            // now lets customize it

            // a custom fill and stroke, if we don't set these properties
            // LiveCharts will set them for us according to our theme.
            lineSeries.Stroke = Color.Purple;
            lineSeries.Fill = Color.FromArgb(10, 80, 00, 80);

            // the beziers in the drawn line are calculated based on
            // the line smoothness property, the value goes from 
            // 0 (straight lines) to 1 (super curved lines)
            // use any value between 0 and 1 to play with the smoothness.
            lineSeries.LineSmoothness = 0.6f;

            // set a stroke dash array
            // 2 spaces for the stroked line, 2 spaces fot the white space
            lineSeries.StrokeDashArray = new double[] {2f, 2f};

            // do not display a label for every point
            lineSeries.DataLabels = false;

            // set a geometry and its size
            lineSeries.GeometrySize = 30;
            lineSeries.Geometry = Geometry.Diamond;

            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(lineSeries);
        }

        public ChartingCollection<Series> SeriesCollection { get; set; }
    }
}
