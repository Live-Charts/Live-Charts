using System.Collections.ObjectModel;
using LiveCharts.Core.Abstractions.DataSeries;
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
            // feed the values

            var values2017 = new ObservableCollection<PointModel>();
            
            //                               ( x, y)
            values2017.Add(new PointModel(10, 6));
            values2017.Add(new PointModel(4, 3));
            values2017.Add(new PointModel(3, 9));
            values2017.Add(new PointModel(6, 2));

            var values2018 = new ObservableCollection<PointModel>();

            //                               (x, y)
            values2018.Add(new PointModel(5, 8));
            values2018.Add(new PointModel(5, 4));
            values2018.Add(new PointModel(9, 6));
            values2018.Add(new PointModel(3, 1));

            var series2018 = new ScatterSeries<PointModel> {Values = values2018};
            var series2017 = new ScatterSeries<PointModel> {Values = values2017};

            // some custom style.
            series2018.Geometry = Geometry.Diamond;
            series2017.Geometry = Geometry.Circle;

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<ISeries>();
            // finally lets add the series to our series collection.
            SeriesCollection.Add(series2018);
            SeriesCollection.Add(series2017);
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}