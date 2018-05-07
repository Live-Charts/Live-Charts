#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;
using Orientation = LiveCharts.Core.Drawing.Styles.Orientation;
using Rectangle = System.Windows.Shapes.Rectangle;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The column point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TShape">the type of the shape.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TSeries">The type fo the series.</typeparam>
    /// <seealso cref="PointView{TModel, Point,Point2D, ColumnViewModel, TShape}" />
    public class BarPointView<TModel, TCoordinate, TSeries, TShape>
        : PointView<TModel, TCoordinate, RectangleViewModel, TSeries, TShape>
        where TCoordinate : ICoordinate
        where TSeries : IStrokeSeries, ICartesianSeries
        where TShape : Shape, new()
    {
        private ChartPoint<TModel, TCoordinate, RectangleViewModel, TSeries> _chartPoint;
        private TimeLine _lastTimeLine;

        /// <inheritdoc />
        protected override void OnDraw(
            ChartPoint<TModel, TCoordinate, RectangleViewModel, TSeries> chartPoint, 
            ChartPoint<TModel, TCoordinate, RectangleViewModel, TSeries> previous,
            TimeLine timeLine)
        {
            var chart = chartPoint.Chart;
            var vm = chartPoint.ViewModel;
            var isNewShape = Shape == null;

            // initialize shape
            if (isNewShape)
            {
                Shape = new TShape();
                chart.Content.AddChild(Shape, true);
                Canvas.SetLeft(Shape, vm.From.Left);
                Canvas.SetTop(Shape, vm.From.Top);
                Shape.Width = vm.From.Width;
                Shape.Height = vm.From.Height;
            }

            // map properties
            Shape.Stroke = chartPoint.Series.Stroke.AsWpf();
            Shape.Fill = chartPoint.Series.Fill.AsWpf();
            Shape.StrokeThickness = chartPoint.Series.StrokeThickness;
            Panel.SetZIndex(Shape, ((ICartesianSeries) chartPoint.Series).ZIndex);
            if (chartPoint.Series.StrokeDashArray != null)
            {
                Shape.StrokeDashArray = new DoubleCollection(chartPoint.Series.StrokeDashArray);
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

            Shape
                .Animate(timeLine)
                .Property(Canvas.LeftProperty, Canvas.GetLeft(Shape), vm.To.Left)
                .Property(FrameworkElement.WidthProperty, Shape.Width, vm.To.Width)
                .Property(Canvas.TopProperty, Canvas.GetTop(Shape), vm.To.Top)
                .Property(
                    FrameworkElement.HeightProperty, !isNewShape ? Shape.Height : vm.From.Height, vm.To.Height)
                .Begin();

            _chartPoint = chartPoint;
            _lastTimeLine = timeLine;
        }

        /// <inheritdoc />
        protected override void PlaceLabel(
            ChartPoint<TModel, TCoordinate, RectangleViewModel, TSeries> chartPoint, 
            SizeF labelSize)
        {
            Canvas.SetTop(Label, chartPoint.ViewModel.To.Y);
            Canvas.SetLeft(Label, chartPoint.ViewModel.To.X);
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart, bool force)
        {
            if (force)
            {
                chart.Content.RemoveChild(Shape, true);
                chart.Content.RemoveChild(Label, true);
                _lastTimeLine = null;
                return;
            }

            // if not forced, animate the exit...

            var barSeries = (IBarSeries) _chartPoint.Series;
            var pivot = chart.Model.ScaleToUi(barSeries.Pivot, chart.Dimensions[1][barSeries.ScalesAt[1]]);

            var animation = Shape.Animate(_lastTimeLine)
                .Property(Canvas.TopProperty, Canvas.GetTop(Shape), pivot)
                .Property(FrameworkElement.HeightProperty, Shape.Height, 0);

            animation.Then((sender, args) =>
            {
                chart.Content?.RemoveChild(Shape, true);
                chart.Content?.RemoveChild(Label, true);
                animation.Dispose();
                animation = null;
                _lastTimeLine = null;
            }).Begin();
        }
    }
}