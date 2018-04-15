using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Animations;

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The heat point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TLabel">The type of the label.</typeparam>
    /// <seealso cref="LiveCharts.Wpf.Views.PointView{TModel, LWeightedCoordinate, HeatViewModel, IHeatSeries, Rectangle, TLabel}" />
    public class HeatPointView<TModel, TLabel>
        : PointView<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries, Rectangle, TLabel>
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {
        protected override void OnDraw(
            Point<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> point, 
            Point<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> previous)
        {
            var chart = point.Chart.View;
            var vm = point.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new Rectangle();
                chart.Content.AddChild(Shape);
                Shape.Fill = vm.From.AsWpf();
            }

            // map properties
            Canvas.SetLeft(Shape, vm.Rectangle.Left);
            Canvas.SetTop(Shape, vm.Rectangle.Top);
            Shape.Width = vm.Rectangle.Width;
            Shape.Height = vm.Rectangle.Height;
            //Shape.Stroke = point.Series.Stroke.AsWpf();
           // Shape.Fill = point.Series.Fill.AsWpf();
            //Shape.StrokeThickness = point.Series.StrokeThickness;
            Panel.SetZIndex(Shape, ((ICartesianSeries)point.Series).ZIndex);
            //if (point.Series.StrokeDashArray != null)
            //{
            //    Shape.StrokeDashArray = new DoubleCollection(point.Series.StrokeDashArray);
            //}

            // animate

            Shape.Fill.Animate()
                .AtSpeed(chart.AnimationsSpeed)
                .Property(SolidColorBrush.ColorProperty, Color.FromArgb(vm.To.A, vm.To.R, vm.To.G, vm.To.B))
                .Begin();
        }

        protected override void OnDispose(IChartView chart)
        {
            chart.Content.RemoveChild(Shape);
            chart.Content.RemoveChild(Label);
        }
    }
}