using System.Collections.ObjectModel;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class InvertedSeries
    {
        public InvertedSeries()
        {
            var values1 = new ObservableCollection<double>();
            values1.Add(5d);
            values1.Add(3d);
            values1.Add(6d);
            values1.Add(4d);

            var values2 = new ObservableCollection<double>();
            values2.Add(7d);
            values2.Add(8d);
            values2.Add(2d);
            values2.Add(8d);

            SeriesCollection = new ChartingCollection<ISeries>();

            SeriesCollection.Add(
                new LineSeries<double>
                {
                    Values = values1
                });

            SeriesCollection.Add(
                new LineSeries<double>
                {
                    Values = values2,
                    DefaultFillOpacity = 0.3f
                });
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}
