using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;
using LiveCharts.Wpf.Shapes;

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The pie point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TLabel">The type of the label.</typeparam>
    /// <seealso cref="LiveCharts.Wpf.Views.PointView{TModel, TPoint, PieCoordinate, PieViewModel, TShape, TLabel}" />
    public class PiePointView<TModel,  TLabel>
        : PointView<TModel, StackedPointCoordinate, PieViewModel, IPieSeries, Slice, TLabel>
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        /// <inheritdoc />
        protected override void OnDraw(
            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> point,
            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new Slice();
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, point.Chart.DrawAreaSize[0] / 2 - vm.To.OuterRadius);
                Canvas.SetTop(Shape, point.Chart.DrawAreaSize[1] / 2 - vm.To.OuterRadius);
                Shape.Rotation = 0d;
                Shape.Wedge = 0d;
                Shape.Width = vm.To.OuterRadius * 2;
                Shape.Height = vm.To.OuterRadius * 2;
            }

            // map properties
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.StrokeThickness = point.Series.StrokeThickness;
            if (point.Series.StrokeDashArray != null)
            {
                Shape.StrokeDashArray = new DoubleCollection(point.Series.StrokeDashArray);
            }
            Shape.InnerRadius = vm.To.InnerRadius;
            Shape.Radius = vm.To.OuterRadius;
            Shape.ForceAngle = true;
            Shape.CornerRadius = point.Series.CornerRadius;
            Shape.PushOut = point.Series.PushOut;

            // animate

            var animation = Shape.Animate().AtSpeed(chart.AnimationsSpeed);

            if (isNewShape)
            {
                Shape.Radius = vm.To.OuterRadius * .8;
                animation
                    .Property(Slice.RadiusProperty, vm.To.OuterRadius, 0)
                    .Property(Slice.RotationProperty, vm.To.Rotation)
                    .Bounce(Slice.WedgeProperty, 0, vm.To.Wedge);
            }
            else
            {
                animation
                    .Property(Slice.RotationProperty, vm.To.Rotation)
                    .Property(Slice.WedgeProperty, vm.To.Wedge);
            }

            animation.Begin();
        }

        /// <inheritdoc />
        protected override void OnDrawLabel(
            Point<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> point, 
            PointF location)
        {
            base.OnDrawLabel(point, location);
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart)
        {
            base.OnDispose(chart);
        }
    }
}