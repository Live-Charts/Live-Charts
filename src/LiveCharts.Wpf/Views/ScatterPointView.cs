using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;

namespace LiveCharts.Wpf.Views
{
    public class ScatterPointView<TModel, TPoint, TLabel>
        : PointView<TModel, TPoint, Weighted2DPoint, ScatterViewModel, Path, TLabel>
        where TPoint : Point<TModel, Weighted2DPoint, ScatterViewModel>, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        protected override void OnDraw(TPoint point, TPoint previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNew = Shape == null;

            if (isNew)
            {
                Shape = new Path{Stretch = Stretch.Fill};
                chart.Content.AddChild(Shape);
                Canvas.SetLeft(Shape, vm.Location.X );
                Canvas.SetTop(Shape, vm.Location.Y);
                Shape.Width = 0;
                Shape.Height = 0;
            }

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();
            Shape.StrokeThickness = 3.5;
            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Data = Geometry.Parse(Core.Drawing.Svg.Geometry.Circle.Data); // Geometry.Parse(viewModel.Geometry.Data);

            var speed = chart.AnimationsSpeed;
            var r = vm.Diameter * .5;
            var b = vm.Diameter * .18;

            if (isNew)
            {
                Shape.Animate()
                    .AtSpeed(speed)
                    .InverseBounce(Canvas.LeftProperty, vm.Location.X - r, b * .5)
                    .InverseBounce(Canvas.TopProperty, vm.Location.Y - r, b * .5)
                    .Bounce(FrameworkElement.WidthProperty, vm.Diameter, b)
                    .Bounce(FrameworkElement.HeightProperty, vm.Diameter, b)
                    .Begin();
            }
            else
            {
                Shape.Animate()
                    .AtSpeed(speed)
                    .Property(Canvas.LeftProperty, vm.Location.X - r)
                    .Property(Canvas.TopProperty, vm.Location.Y - r)
                    .Begin();
            }
        }

        protected override void OnDispose(IChartView chart)
        {
            var animation = Shape.Animate()
                .AtSpeed(chart.AnimationsSpeed)
                .Property(FrameworkElement.HeightProperty, 0)
                .Property(FrameworkElement.WidthProperty, 0);

            animation.Then((sender, args) =>
            {
                chart.Content.RemoveChild(Shape);
                chart.Content.RemoveChild(Label);
                animation.Dispose();
                animation = null;
            }).Begin();
        }
    }
}