using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class BasicStackedBar
    {
        public BasicStackedBar()
        {
            // let's feed the series...
            
            var charlesSeries = new StackedBarSeries<double>();
            charlesSeries.Add(4d); // *
            charlesSeries.Add(8d);
            charlesSeries.Add(-3d);

            var fridaSeries = new StackedBarSeries<double>();
            fridaSeries.Add(5d); // *
            fridaSeries.Add(-3d);
            fridaSeries.Add(8d);

            var abrahamSeries = new StackedBarSeries<double>();
            abrahamSeries.Add(5d); // *
            abrahamSeries.Add(2d);
            abrahamSeries.Add(-7d);

            // The stack by default is done based on the index of each element 
            // so charles, frida and abraham at index = 0 share 4, 5 and 5 values (see * mark)
            // the total height of the bar when index = 0 will be 4 + 5 + 5 = 14

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<ISeries>();
            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(charlesSeries);
            SeriesCollection.Add(fridaSeries);
            SeriesCollection.Add(abrahamSeries);
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}