using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Wpf.PointViews
{
    public class BezierPointView<TModel, TPoint, TCoordinate, TViewModel, TLabel>
        : PointView<TModel, TPoint, TCoordinate, TViewModel, Path, TLabel>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : Point2D
        where TViewModel : BezierViewModel
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        protected override void OnDraw(TPoint point, TPoint previous, IChartView chart, TViewModel viewModel)
        {
            var isNew = Shape == null;

            if (isNew)
            {
                var wpfChart = (CartesianChart)chart;
                Shape = new Path();
                wpfChart.DrawArea.Children.Add(Shape);
                Canvas.SetLeft(Shape, viewModel.Location.X);
                Canvas.SetTop(Shape, viewModel.Location.Y);
            }

            Shape.Stroke = point.Series.Stroke.AsSolidColorBrush();
            Shape.Fill = point.Series.Fill.AsSolidColorBrush();
            Shape.Data = Geometry.Parse(viewModel.Geometry.Data);
            Shape.Width = viewModel.GeometrySize;
            Shape.Height = viewModel.GeometrySize;
        }

        protected override void OnDrawLabel(TPoint point, Point location, IChartView chart)
        {
            base.OnDrawLabel(point, location, chart);
        }

        protected override void OnDispose(IChartView chart)
        {
            base.OnDispose(chart);
        }
    }
}