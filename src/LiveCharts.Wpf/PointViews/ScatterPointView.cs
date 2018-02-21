using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;

namespace LiveCharts.Wpf.PointViews
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
                var wpfChart = (CartesianChart) chart;
                Shape = new Path();
                wpfChart.DrawArea.Children.Add(Shape);
                Canvas.SetLeft(Shape, vm.Location.X);
                Canvas.SetTop(Shape, vm.Location.Y);
                Shape.Width = 0;
                Shape.Height = 0;
            }

            Shape.Stroke = point.Series.Stroke.AsWpf();
            Shape.Fill = point.Series.Fill.AsWpf();

            var speed = chart.AnimationsSpeed;
            var bounce = isNew ? vm.Radius * .3 : 0;

            Shape.Animate()
                .AtSpeed(speed)
                .Property(Canvas.LeftProperty)
        }

        protected override void OnDispose(IChartView chart)
        {
            base.OnDispose(chart);
        }
    }
}