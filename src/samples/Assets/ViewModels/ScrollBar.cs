using System;
using System.Collections.ObjectModel;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace Assets.ViewModels
{
    public class ScrollBar
    {
        public ScrollBar()
        {
            var random = new Random();
            var values = new ObservableCollection<double>();
            var trend = 0;

            for (var i = 0; i < 50; i++)
            {
                values.Add(trend += random.Next(-5, 10));
            }

            ZoomedSeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = values
                }
            };

            ScrollBarSeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = values,
                    Geometry = Geometry.Empty
                }
            };

            X = new ObservableCollection<Plane>
            {
                new Axis
                {
                    MinValue = 0,
                    MaxValue = 5
                }
            };
            Y = new ObservableCollection<Plane>
            {
                new Axis
                {
                    MinValue = 0,
                    MaxValue = 20
                }
            };
        }

        public ObservableCollection<ISeries> ZoomedSeriesCollection { get; set; }
        public ObservableCollection<ISeries> ScrollBarSeriesCollection { get; set; }
        public ObservableCollection<Plane> X { get; set; }
        public ObservableCollection<Plane> Y { get; set; }
        public Axis ScrollableAxis { get; set; }
    }
}