using System.Collections.ObjectModel;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class BasicBar
    {
        public BasicBar()
        {
            var values = new ObservableCollection<double>();

            // add some values...
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);

            var barSeries = new BarSeries<double>();

            barSeries.Values = values;

            // a custom fill and stroke, if we don't set these properties
            // LiveCharts will set them for us according to our theme.
            barSeries.StrokeThickness = 3f;
            barSeries.Stroke = LiveCharts.Core.Drawing.Brushes.Purple;
            barSeries.Fill = LiveCharts.Core.Drawing.Brushes.MediumPurple;

            // limit the column width to 65.
            barSeries.MaxColumnWidth = 65f;

            // do not display a label for every point
            barSeries.DataLabels = false;

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ObservableCollection<ISeries>();
            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(barSeries);
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }
    }
}