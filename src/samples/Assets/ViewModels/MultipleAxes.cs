using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;

namespace Assets.ViewModels
{
    public class MultipleAxes
    {
        public MultipleAxes()
        {
            // by default a cartesian chart axis contains only one element
            // and all the series are scaled in the default axis.
            // in this case we will only override the Y axis
            // the X axis will have the default value.
            var axis1 = new Axis();
            var axis2 = new Axis();

            // we create a collection to store our axes collection.
            YAxis = new ChartingCollection<Plane>();
            YAxis.Add(axis1);
            YAxis.Add(axis2);

            // we will bind in XAML Planes to CartesianChart.AxisY property

            // now we create some series.

            var series1 = new LineSeries<double>();
            series1.Add(10d);
            series1.Add(5d);
            series1.Add(3d);
            series1.Add(6d);

            var series2 = new LineSeries<double>();
            series2.Add(350d);
            series2.Add(650d);
            series2.Add(125d);
            series2.Add(156d);

            // we will scale series1 at axis1
            // axis1 is the first element in the Planes collection
            // then the index of axis1 in the Planes array is 0
            // so we will let the series know that is needs to be
            // scaled in Y at 0
            series1.ScalesYAt = 0;
            
            // axis2 index in the Planes collection is 1
            // we want the series2 to be scaled at axis2
            // then the series2 ScalesYAt property must match
            // the index of the axis2 instance in the Planes collection.
            series2.ScalesYAt = 1;

            // finally we add the series to our series collection.
            SeriesCollection = new ChartingCollection<Series>();
            SeriesCollection.Add(series1);
            SeriesCollection.Add(series2);

            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<Series> SeriesCollection { get; set; }
        public ChartingCollection<Plane> YAxis { get; set; }
    }
}