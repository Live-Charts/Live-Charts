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
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Animations;
using Slice = LiveCharts.Wpf.Shapes.Slice;
using TimeLine = LiveCharts.Core.Animations.TimeLine;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The pie point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="LiveCharts.Wpf.Views.PointView{TModel, TPoint, PieCoordinate, PieViewModel, TShape}" />
    public class PiePointView<TModel>
        : PointView<TModel, StackedPointCoordinate, PieViewModel, IPieSeries, Slice>
    {
        private TimeLine _lastTimeLine;

        /// <inheritdoc />
        protected override void OnDraw(
            ChartPoint<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> chartPoint,
            ChartPoint<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> previous,
            TimeLine timeLine)
        {
           
        }

        /// <inheritdoc />
        protected override void PlaceLabel(
            ChartPoint<TModel, StackedPointCoordinate, PieViewModel, IPieSeries> chartPoint,
            SizeF labelSize)
        {
            // this one could be a complex task...
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OnDispose(IChartView chart, bool force)
        {
            if (force)
            {
                chart.Canvas.DisposeChild(Shape, true);
                chart.Canvas.DisposeChild(Label, true);
                _lastTimeLine = null;
                return;
            }

            var sliceAnimation = Shape.Animate(_lastTimeLine)
                .Property(Slice.WedgeProperty, Shape.Wedge, 0);

            sliceAnimation
                .Then((sender, args) =>
                {
                    chart.Canvas?.DisposeChild(Shape, true);
                    chart.Canvas?.DisposeChild(Label, true);
                    sliceAnimation.Dispose();
                    sliceAnimation = null;
                    _lastTimeLine = null;
                });
        }
    }
}