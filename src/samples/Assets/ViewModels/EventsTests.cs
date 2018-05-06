using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Assets.Commands;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Interaction.Points;

namespace Assets.ViewModels
{
    public class EventsTests
    {
        public EventsTests()
        {
            Log = new ObservableCollection<string>();

            var r = new Random();
            var trend = 0d;
            var generator = new double[20];

            SeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    // generate some random values...
                    Values = new ObservableCollection<double>(generator.Select(i => trend += r.Next(-5, 10)))
                }
            };

            UpdatePreviewCommand = new RelayCommand(chart =>
            {
                // notice you can access the chart that was updated
                // in this case, because this view model assembly
                // has no idea of the UI platform I am using
                // I will cast it to IChartView.
                var chartView = (IChartView) chart;
                var seriesCount = chartView.Series.Count();
                Log.Insert(0,$"[C] UpdatePreview fired");
            });

            UpdateFinishedCommand = new RelayCommand(chart =>
            {
                var chartView = (IChartView) chart;
                var firstSeriesValuesCount = ((ICollection) chartView.Series.First().Values).Count;
                Log.Insert(0,$"[C] Updated");
            });

            PointerDataDownCommand = new RelayCommand(points =>
            {
                var chartPoints = (IEnumerable<IChartPoint>) points;
                var point = chartPoints.FirstOrDefault();
                if (point == null) return;
                var pointCoordinate = (PointCoordinate)point.Coordinate;
                Log.Insert(0,$"[C] PointerDown @ {pointCoordinate.X}, {pointCoordinate.Y}");
            });

            PointerDataEnteredCommand = new RelayCommand(points =>
            {
                var chartPoints = (IEnumerable<IChartPoint>)points;
                var point = chartPoints.FirstOrDefault();
                if (point == null) return;
                var pointCoordinate = (PointCoordinate)point.Coordinate;
                Log.Insert(0,$"[C] PointerEntered @ {pointCoordinate.X}, {pointCoordinate.Y}");
            });

            PointerDataLeftCommand = new RelayCommand(points =>
            {
                var chartPoints = (IEnumerable<IChartPoint>) points;
                var point = chartPoints.FirstOrDefault();
                if (point == null) return;
                var pointCoordinate = (PointCoordinate) point.Coordinate;
                Log.Insert(0,$"[C] PointerLeft @ {pointCoordinate.X}, {pointCoordinate.Y}");
            });

            var x = new Axis();
            x.RangeChanged += (plane, min, max) =>
            {
                Log.Insert(0,
                    $"X changed, from {min:N}-{max:N} to {plane.ActualMinValue:N} - {plane.ActualMaxValue:N}");
            };
            XAxis = new ObservableCollection<Plane>
            {
                x
            };
        }

        public ObservableCollection<ISeries> SeriesCollection { get; }

        public ObservableCollection<Plane> XAxis { get; }

        public ICommand UpdatePreviewCommand { get; }

        public ICommand UpdateFinishedCommand { get; }

        public ICommand PointerDataDownCommand { get; }

        public ICommand PointerDataEnteredCommand { get; }

        public ICommand PointerDataLeftCommand { get; }

        public ObservableCollection<string> Log { get; set; }
    }
}