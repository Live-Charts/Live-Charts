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
using System.Windows.Controls;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The heat point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Wpf.Views.PointView{TModel, LWeightedCoordinate, HeatViewModel, IHeatSeries, Rectangle}" />
    public class HeatPointView<TModel>
        : PointView<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries, Rectangle>
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
                Shape.Fill = new SolidColorBrush(Color.FromArgb(vm.From.A, vm.From.R, vm.From.G, vm.From.B));
            }

            // map properties
            Canvas.SetLeft(Shape, vm.Rectangle.Left);
            Canvas.SetTop(Shape, vm.Rectangle.Top);
            Shape.Width = vm.Rectangle.Width;
            Shape.Height = vm.Rectangle.Height;
            // Shape.Stroke = point.Series.Stroke.AsWpf();
            // Shape.Fill = point.Series.Fill.AsWpf();
            //Shape.StrokeThickness = point.Series.StrokeThickness;
            Panel.SetZIndex(Shape, point.Series.ZIndex);
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

        /// <inheritdoc />
        protected override string GetLabelContent(
            Point<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> point)
        {
            var chart = point.Chart;
            var wAxis = chart.Dimensions[2][point.Series.ScalesAt[2]];

            return $"{wAxis.FormatValue(point.Coordinate[2][0])}";
        }

        protected override void PlaceLabel(Point<TModel, WeightedCoordinate, HeatViewModel, IHeatSeries> point, SizeF labelSize)
        {
            Canvas.SetTop(Label, point.ViewModel.Rectangle.Y);
            Canvas.SetLeft(Label, point.ViewModel.Rectangle.X);
        }

        protected override void OnDispose(IChartView chart)
        {
            chart.Content.RemoveChild(Shape);
            chart.Content.RemoveChild(Label);
        }
    }
}