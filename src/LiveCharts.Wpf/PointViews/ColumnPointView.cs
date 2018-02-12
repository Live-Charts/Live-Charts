using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// The column point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">the type of the chart point.</typeparam>
    /// <typeparam name="TShape">the type of the shape.</typeparam>
    /// <typeparam name="TLabel">the type of the label.</typeparam>
    /// <typeparam name="TCoordinate">the type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">the type of the view model.</typeparam>
    /// <seealso cref="PointView{TModel, Point,Point2D, ColumnViewModel, TShape, TLabel}" />
    public class ColumnPointView<TModel, TPoint, TCoordinate, TViewModel, TShape, TLabel>
        : PointView<TModel, TPoint, TCoordinate, TViewModel, TShape, TLabel>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : Point2D
        where TViewModel : ColumnViewModel
        where TShape : Shape, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        private TPoint _point;

        /// <inheritdoc />
        protected override void OnDraw(TPoint point, TPoint previous)
        {
            var chart = point.Chart.View;
            var viewModel = point.ViewModel;
            var isNew = Shape == null;

            if (isNew)
            {
                var wpfChart = (CartesianChart) chart;
                Shape = new TShape();
                wpfChart.DrawArea.Children.Add(Shape);
                Canvas.SetLeft(Shape, viewModel.Left);
                Canvas.SetTop(Shape, viewModel.Zero);
                Shape.Width = viewModel.Width;
                Shape.Height = 0;
            }

            var r = Shape as Rectangle;
            if (r != null)
            {
                var radius = viewModel.Width * .4;
                r.RadiusY = radius;
                r.RadiusX = radius;
            }

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();

            var speed = chart.AnimationsSpeed;

            var bounce = isNew ? 30 : 0;

            Shape.Animate()
                .AtSpeed(speed)
                .Property(Canvas.LeftProperty, viewModel.Left)
                .Property(Canvas.TopProperty, 
                    new AnimationFrame(0.9, viewModel.Top - bounce),
                    new AnimationFrame(1, viewModel.Top))
                .Property(FrameworkElement.WidthProperty, viewModel.Width)
                .Property(FrameworkElement.HeightProperty,
                    new AnimationFrame(0.9, viewModel.Height + bounce),
                    new AnimationFrame(1, viewModel.Height))
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
                var wpfChart = (CartesianChart) chart;
                Label = new TLabel();
                Label.Measure(point.PackAll());
                Canvas.SetLeft(Shape, Canvas.GetLeft(Shape));
                Canvas.SetTop(Shape, Canvas.GetTop(Shape));
                wpfChart.DrawArea.Children.Add((UIElement) Label);
            }

            var speed = chart.AnimationsSpeed;

            ((FrameworkElement) Label).BeginAnimation(
                Canvas.LeftProperty,
                new DoubleAnimation(location.X, speed));
            ((FrameworkElement) Label).BeginAnimation(
                Canvas.TopProperty,
                new DoubleAnimation(location.Y, speed));
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart)
        {
            var wpfChart = (CartesianChart) chart;

            var zero = chart.Model.ScaleToUi(0, chart.Dimensions[1][_point.Series.ScalesAt[1]]);

            Shape.Animate()
                .AtSpeed(chart.AnimationsSpeed)
                .Property(Canvas.TopProperty, zero)
                .Property(FrameworkElement.HeightProperty, 0)
                .Then((sender, args) =>
                {
                    wpfChart.DrawArea.Children.Remove(Shape);
                    wpfChart.DrawArea.Children.Remove((UIElement)Label);
                })
                .Begin();
        }
    }
}