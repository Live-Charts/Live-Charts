using System.Collections.ObjectModel;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class BasicPie
    {
        public BasicPie()
        {
            var chromeValues = new ObservableCollection<double>();
            chromeValues.Add(12d);

            var fireFoxValues = new ObservableCollection<double>();
            fireFoxValues.Add(8d);

            var explorerValues = new ObservableCollection<double>();
            explorerValues.Add(6d);

            // some custom style..
            //explorerSeries.CornerRadius = 6;
            //explorerSeries.PushOut = 10;

            // create a collection to store our series.
            SeriesCollection = new ChartingCollection<ISeries>();
            // add the series to our collection
            SeriesCollection.Add(
                new PieSeries<double>
                {
                    Values = chromeValues
                });
            SeriesCollection.Add(
                new PieSeries<double>
                {
                    Values = fireFoxValues
                });
            SeriesCollection.Add(
                new PieSeries<double>
                {
                    Values = explorerValues
                });
            // we bind the SeriesCollection property to the PieChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}