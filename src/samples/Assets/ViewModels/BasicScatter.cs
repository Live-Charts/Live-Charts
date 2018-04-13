using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Drawing.Svg;

namespace Assets.ViewModels
{
    public class BasicScatter
    {
        public BasicScatter()
        {
            // populate the series.

            var year2017Series = new ScatterSeries<PointModel>();
            
            //                               ( x, y)
            year2017Series.Add(new PointModel(10, 6));
            year2017Series.Add(new PointModel(4, 3));
            year2017Series.Add(new PointModel(3, 9));
            year2017Series.Add(new PointModel(6, 2));

            var year2018Series = new ScatterSeries<PointModel>();

            //                               (x, y)
            year2018Series.Add(new PointModel(5, 8));
            year2018Series.Add(new PointModel(5, 4));
            year2018Series.Add(new PointModel(9, 6));
            year2018Series.Add(new PointModel(3, 1));

            // some custom style.
            year2018Series.Geometry = Geometry.Diamond;
            year2017Series.Geometry = Geometry.Circle;

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<Series>();
            // finally lets add the series to our series collection.
            SeriesCollection.Add(year2018Series);
            SeriesCollection.Add(year2017Series);
        }

        public ChartingCollection<Series> SeriesCollection { get; set; }
    }
}