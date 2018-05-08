using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Interaction.Points;

namespace Samples.Wpf.Views
{
    /// <summary>
    /// Interaction logic for EventsTests.xaml
    /// </summary>
    public partial class EventsTests : UserControl
    {
        public EventsTests()
        {
            InitializeComponent();
        }

        private void OnChartUpdatePreview(IChartView chart)
        {
            var context = (Assets.ViewModels.EventsTests) DataContext;
            context.Log.Insert(0,"UpdatePreview");
        }

        private void OnChartUpdated(IChartView chart)
        {
            var context = (Assets.ViewModels.EventsTests) DataContext;
            context.Log.Insert(0,"Updated");
        }

        private void Chart_OnChartPointPointerDown(
            IChartView chart, IEnumerable<IChartPoint> points, MouseButtonEventArgs args)
        {
            // the event is handled, so panning will be disabled while we click on a data point.
            args.Handled = true;

            var context = (Assets.ViewModels.EventsTests) DataContext;

            // points contains all the elements that were clicked by the users
            // in the case in our chart we do not have overlapped points
            // so we will only get one.

            var clickedPoint = points.FirstOrDefault();
            if (clickedPoint == null) return;

            // see coordinates docs to get more information about the following syntax
            // a coordinate can be accessed as follows Coordinate[dimension][ref]
            // where dimension is the plane dimension, normally 0 is X and 1 is Y
            // and ref is the value at the dimension, normally only 0 is used , but for example
            // in a case of a ranged coordinate, 0 will be the min value and 1 the maximum.

            context.Log.Insert(
                0,
                $"PointerDown @ {clickedPoint.Coordinate[0][0]}, {clickedPoint.Coordinate[1][0]}");

            // alternatively you can cast the coordinate if you know the type.
            // var pointCoordinate = (PointCoordinate) clickedPoint.Coordinate;
            // var x = pointCoordinate.X;
            // var y = pointCoordinate.Y;
            // context.Log.Add($"chart point clicked with coordinates {x}, {y}");
        }

        private void Chart_OnChartPointPointerEntered(
            IChartView chart, IEnumerable<IChartPoint> points, MouseEventArgs args)
        {
            var context = (Assets.ViewModels.EventsTests) DataContext;
            var point = points.FirstOrDefault();
            if (point == null) return;
            var pointCoordinate = (PointCoordinate) point.Coordinate;
            context.Log.Insert(0,$"PointerEntered @ {pointCoordinate.X}, {pointCoordinate.Y}");
        }

        private void Chart_OnChartPointPointerLeft(
            IChartView chart, IEnumerable<IChartPoint> points, MouseEventArgs args)
        {
            var context = (Assets.ViewModels.EventsTests) DataContext;
            var point = points.FirstOrDefault();
            if (point == null) return;
            var pointCoordinate = (PointCoordinate) point.Coordinate;
            context.Log.Insert(0,$"PointerLeft @ {pointCoordinate.X}, {pointCoordinate.Y}");
        }
    }
}
