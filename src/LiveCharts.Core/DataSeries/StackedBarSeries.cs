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
using LiveCharts.Core.Drawing.Shapes;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;
#if NET45 || NET46
using Font = LiveCharts.Core.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The stacked bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="CartesianStrokeSeries{TModel,TCoordinate, TPointShape}" />
    /// <seealso cref="IBarSeries" />
    public class StackedBarSeries<TModel> 
        : BaseBarSeries<TModel, StackedPointCoordinate>, IStackedBarSeries
    {
        private int _stackIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedBarSeries{TModel}"/> class.
        /// </summary>
        public StackedBarSeries()
        {
            DataLabelFormatter = coordinate => Format.AsMetricNumber(coordinate.Value);
            TooltipFormatter = coordinate =>
                $"{Format.AsMetricNumber(coordinate.Value)} / {Format.AsMetricNumber(coordinate.TotalStack)}";
            Charting.BuildFromTheme<IStackedBarSeries>(this);
            Charting.BuildFromTheme<ISeries<StackedPointCoordinate>>(this);
        }

        /// <inheritdoc />
        int ISeries.GroupingIndex => StackIndex;

        /// <inheritdoc />
        public int StackIndex
        {
            get => _stackIndex;
            set
            {
                _stackIndex = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        protected override RectangleViewModel BuildModel(
            ChartPoint<TModel, StackedPointCoordinate, IRectangle> current,
            UpdateContext context, 
            ChartModel chart,
            Plane directionAxis,
            Plane scaleAxis,
            float cw, 
            float columnStart, 
            float[] byBarOffset,
            float[] positionOffset,
            Orientation orientation,
            int h,
            int w)
        {
            float currentOffset = chart.ScaleToUi(current.Coordinate[0][0], directionAxis);
            float key = current.Coordinate.Key;
            float value = current.Coordinate.Value;

            float stack;

            unchecked
            {
                stack = context.GetStack((int) key, ScalesAt[1], value >= 0);
            }

            float[] columnCorner1 = new[]
            {
                currentOffset,
                chart.ScaleToUi(stack, scaleAxis)
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

            current.Coordinate.TotalStack = stack;

            if (current.Shape == null)
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
                return new RectangleViewModel(
                    RectangleF.Empty, 
                    initialRectangle, 
                    orientation);
            }

            // ToDo: optimize this rule???
            if (orientation == Orientation.Horizontal)
            {
                float y = location[h] + byBarOffset[1] + positionOffset[1];
                float l = columnCorner1[1] > columnCorner2[1] ? columnCorner1[1] : columnCorner2[1];

                return new RectangleViewModel(
                    new RectangleF(current.Shape.Left, current.Shape.Top, current.Shape.Width, current.Shape.Height), 
                    new RectangleF(
                        location[w] + byBarOffset[0] + positionOffset[0],
                        y + (l - y) * current.Coordinate.From / stack,
                        Math.Abs(difference[w]),
                        Math.Abs(difference[h]) * value / stack),
                    orientation);
            }
            else
            {
                float x = location[w] + byBarOffset[0] + positionOffset[0];
                float l = columnCorner1[1] > columnCorner2[1] ? columnCorner1[1] : columnCorner2[1];

                return new RectangleViewModel(
                    new RectangleF(current.Shape.Left, current.Shape.Top, current.Shape.Width, current.Shape.Height),
                    new RectangleF(
                        x + (l - x) * current.Coordinate.From / stack,
                        location[h] + byBarOffset[1] + positionOffset[1],
                        Math.Abs(difference[w]) * value / stack,
                        Math.Abs(difference[h])),
                    orientation);
            }
        }
    }
}