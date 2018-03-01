using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;
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
    public class ColumnPointView<TModel, TPoint, TShape, TLabel>
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
            var @is = vm.ColumnInitialState;

            if (@is != RectangleF.Empty)
            {
                Shape = new TShape();
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, vm.Left);
                Canvas.SetTop(Shape, vm.Zero);
                Shape.Width = vm.Width;
                Shape.Height = 0;
            }

            var r = Shape as Rectangle;
            if (r != null)
            {
                var radius = vm.Width * .4;
                r.RadiusY = radius;
                r.RadiusX = radius;
            }

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();

            var speed = chart.AnimationsSpeed;

            Shape.Animate()
                .AtSpeed(speed)
                .Bounce(Canvas.LeftProperty, vm.Left, .1)
                .Bounce(FrameworkElement.WidthProperty, vm.Width, .1)
                .Bounce(Canvas.TopProperty, vm.Top)
                .Bounce(FrameworkElement.HeightProperty, vm.Height)
                .Begin();
            _point = point;
        }

        /// <inheritdoc />
        protected override void OnDrawLabel(TPoint point, Core.Drawing.Point location)
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