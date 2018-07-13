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

using System;
using System.Drawing;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The bar series class.
    /// </summary>The column series class.
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class BarSeries<TModel> : BaseBarSeries<TModel, PointCoordinate, IBarSeries>
    {
        private ISeriesViewProvider<TModel, PointCoordinate, RectangleViewModel, IBarSeries> _provider;

        /// <inheritdoc />
        public BarSeries()
        {
            DataLabelFormatter = coordinate => Format.AsMetricNumber(coordinate.Y);
            TooltipFormatter = DataLabelFormatter;
            Charting.BuildFromTheme<IBarSeries>(this);
            Charting.BuildFromTheme<ISeries<PointCoordinate>>(this);
        }

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, PointCoordinate, RectangleViewModel, IBarSeries>
            DefaultViewProvider => _provider ??
                                   (_provider = Charting.Settings.UiProvider.BarViewProvider<TModel, PointCoordinate, IBarSeries>());

        /// <inheritdoc />
        protected override void BuildModel(
            ChartPoint<TModel, PointCoordinate, RectangleViewModel, IBarSeries> current, UpdateContext context, ChartModel chart, 
            Plane directionAxis, Plane scaleAxis, float cw, float columnStart, float[] byBarOffset, float[] positionOffset, 
            Orientation orientation, int h, int w)
        {
            float currentOffset = chart.ScaleToUi(current.Coordinate[0][0], directionAxis);

            float[] columnCorner1 = new[]
            {
                currentOffset,
                chart.ScaleToUi(current.Coordinate[1][0], scaleAxis)
            };

            float[] columnCorner2 = new[]
            {
                currentOffset + cw,
                columnStart
            };

            float[] difference = Perform.SubstractEach2D(columnCorner1, columnCorner2);

            float[] location = new[]
            {
                currentOffset,
                columnStart + (columnCorner1[1] < columnStart ? difference[1] : 0f)
            };

            if (current.View.VisualElement == null)
            {
                var initialRectangle = chart.InvertXy
                    ? new RectangleF(
                        columnStart,
                        location[h] + byBarOffset[1] + positionOffset[1],
                        0f,
                        Math.Abs(difference[h]))
                    : new RectangleF(
                        location[w] + byBarOffset[0] + positionOffset[0],
                        columnStart,
                        Math.Abs(difference[w]),
                        0f);
                current.ViewModel = new RectangleViewModel(RectangleF.Empty, initialRectangle, orientation);
            }

            current.ViewModel = new RectangleViewModel(
                current.ViewModel.To,
                new RectangleF(
                    location[w] + byBarOffset[0] + positionOffset[0],
                    location[h] + byBarOffset[1] + positionOffset[1],
                    Math.Abs(difference[w]),
                    Math.Abs(difference[h])),
                orientation);
        }
    }
}
