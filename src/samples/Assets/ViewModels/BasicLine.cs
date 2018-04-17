using System.Collections.ObjectModel;
using System.Drawing;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Svg;

namespace Assets.ViewModels
{
    public class BasicLine
    {
        public BasicLine()
        {
            var values = new ObservableCollection<double>();

            // add some values...
            values.Add(4);
            values.Add(9);
            values.Add(2);
            values.Add(6);

            // now lets customize it
            var lineSeries = new LineSeries<double>();
            lineSeries.Values = values;

            // a custom fill and stroke, if we don't set these properties
            // LiveCharts will set them for us according to our theme.
            lineSeries.Stroke = LiveCharts.Core.Drawing.Brushes.Purple;
            lineSeries.Fill = new LiveCharts.Core.Drawing.SolidColorBrush(Color.FromArgb(10, 80, 00, 80));

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

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<ISeries>();
            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(lineSeries);
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}
