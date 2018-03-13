using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;
using Orientation = LiveCharts.Core.Abstractions.Orientation;
using Point = LiveCharts.Core.Coordinates.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The column point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">the type of the chart point.</typeparam>
    /// <typeparam name="TShape">the type of the shape.</typeparam>
    /// <typeparam name="TLabel">the type of the label.</typeparam>
    /// <seealso cref="PointView{TModel, Point,Point2D, ColumnViewModel, TShape, TLabel}" />
    public class BarColumnPointView<TModel, TPoint, TShape, TLabel>
        : PointView<TModel, TPoint, Point, ColumnViewModel, TShape, TLabel>
        where TPoint : Point<TModel, Point, ColumnViewModel>, new()
        where TShape : Shape, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        private TPoint _point;

        /// <inheritdoc />
        protected override void OnDraw(TPoint point, TPoint previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new TShape();
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, vm.From.Left);
                Canvas.SetTop(Shape, vm.From.Top);
                Shape.Width = vm.From.Width;
                Shape.Height = vm.From.Height;
            }

            // map shape properties

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.StrokeThickness = point.Series.StrokeThickness;
            if (point.Series.StrokeDashArray != null)
            {
                Shape.StrokeDashArray = new DoubleCollection(point.Series.StrokeDashArray);
            }
            
            // special case

            var r = Shape as Rectangle;
            if (r != null)
            {
                var radius = (vm.Orientation == Orientation.Horizontal ? vm.To.Width : vm.To.Height) * .4;
                r.RadiusY = radius;
                r.RadiusX = radius;
            }

            // animate

            var speed = chart.AnimationsSpeed;

            var animation = Shape.Animate()
                .AtSpeed(speed)
                .Property(Canvas.LeftProperty, vm.To.Left)
                .Property(FrameworkElement.WidthProperty, vm.To.Width);

            if (isNewShape)
            {
                const int bounce = 8;
                animation
                    .InverseBounce(Canvas.TopProperty, vm.To.Top, bounce)
                    .Bounce(FrameworkElement.HeightProperty, vm.To.Height, bounce);
            }
            else
            {
                animation
                    .Property(Canvas.TopProperty, vm.To.Top)
                    .Property(FrameworkElement.HeightProperty, vm.To.Height);
            }

            animation.Begin();

            _point = point;
        }

        /// <inheritdoc />
        protected override void OnDrawLabel(TPoint point, PointF location)
        {
            var chart = point.Chart.View;
            var isNew = Label == null;

            if (isNew)
            {
                Label = new TLabel();
                Label.Measure(point.PackAll());
                Canvas.SetLeft(Shape, Canvas.GetLeft(Shape));
                Canvas.SetTop(Shape, Canvas.GetTop(Shape));
                chart.Content.AddChild(Label);
            }

            var speed = chart.AnimationsSpeed;

            Label.BeginAnimation(
                Canvas.LeftProperty,
                new DoubleAnimation(location.X, speed));
            Label.BeginAnimation(
                Canvas.TopProperty,
                new DoubleAnimation(location.Y, speed));
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart)
        {
            var zero = chart.Model.ScaleToUi(0, chart.Dimensions[1][_point.Series.ScalesAt[1]]);

            var animation = Shape.Animate()
                .AtSpeed(chart.AnimationsSpeed)
                .Property(Canvas.TopProperty, zero)
                .Property(FrameworkElement.HeightProperty, 0);

            animation.Then((sender, args) =>
            {
                chart.Content.RemoveChild(chart);
                chart.Content.RemoveChild(Label);
                animation.Dispose();
                animation = null;
            }).Begin();
        }
    }
}