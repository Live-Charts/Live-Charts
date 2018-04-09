using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;
using LiveCharts.Wpf.Shapes;

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The pie point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">The type of the point.</typeparam>
    /// <typeparam name="TLabel">The type of the label.</typeparam>
    /// <seealso cref="LiveCharts.Wpf.Views.PointView{TModel, TPoint, PieCoordinate, PieViewModel, TShape, TLabel}" />
    public class PiePointView<TModel, TPoint, TLabel>
        : PointView<TModel, TPoint, StackedCoordinate, PieViewModel, Slice, TLabel>
        where TPoint : Point<TModel, StackedCoordinate, PieViewModel>, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        /// <inheritdoc />
        protected override void OnDraw(TPoint point, TPoint previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new Slice();
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, 0);
                Canvas.SetTop(Shape,0);
                Shape.Rotation = 0d;
                Shape.Wedge = 0d;
                Shape.Width = point.Chart.View.ControlSize[0];
                Shape.Height = point.Chart.View.ControlSize[1];
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
            Shape.CornerRadius = ((IPieSeries) point.Series).CornerRadius;
            Shape.PushOut = ((IPieSeries) point.Series).PushOut;

            // animate

            var animation = Shape.Animate().AtSpeed(chart.AnimationsSpeed);

            if (isNewShape)
            {
                animation
                    .Bounce(Slice.RotationProperty, vm.To.Rotation)
                    .Property(Slice.WedgeProperty, vm.To.Wedge);
            }
            else
            {
                animation
                    .Bounce(Slice.RotationProperty, vm.To.Rotation)
                    .Property(Slice.WedgeProperty, vm.To.Wedge);
            }

            animation.Begin();
        }

        /// <inheritdoc />
        protected override void OnDrawLabel(TPoint point, PointF location)
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