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
using LiveCharts.Charts;
using LiveCharts.Coordinates;
using LiveCharts.Dimensions;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Drawing.Styles;
using LiveCharts.Interaction;
using LiveCharts.Interaction.Points;
using LiveCharts.Updating;
#if NET45 || NET46
using Font = LiveCharts.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.DataSeries
{
    /// <summary>
    /// The stacked bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="BaseBarSeries{TModel,TCoordinate}" />
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
            Global.Settings.BuildFromTheme<IStackedBarSeries>(this);
            Global.Settings.BuildFromTheme<ISeries<StackedPointCoordinate>>(this);
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
                OnPropertyChanged(nameof(StackIndex));
            }
        }

        /// <inheritdoc />
        internal override RectangleViewModel BuildModel(
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
                    ? new RectangleD(
                        columnStart,
                        location[h] + byBarOffset[1] + positionOffset[1],
                        0,
                        Math.Abs(difference[h]))
                    : new RectangleD(
                        location[w] + byBarOffset[0] + positionOffset[0],
                        columnStart,
                        Math.Abs(difference[w]),
                        0);
                return new RectangleViewModel(
                    RectangleD.Empty, 
                    initialRectangle, 
                    orientation);
            }

            // ToDo: optimize this rule???
            if (orientation == Orientation.Horizontal)
            {
                float y = location[h] + byBarOffset[1] + positionOffset[1];
                float l = columnCorner1[1] > columnCorner2[1] ? columnCorner1[1] : columnCorner2[1];

                return new RectangleViewModel(
                    new RectangleD(current.Shape.Left, current.Shape.Top, current.Shape.Width, current.Shape.Height), 
                    new RectangleD(
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
                    new RectangleD(current.Shape.Left, current.Shape.Top, current.Shape.Width, current.Shape.Height),
                    new RectangleD(
                        x + (l - x) * current.Coordinate.From / stack,
                        location[h] + byBarOffset[1] + positionOffset[1],
                        Math.Abs(difference[w]) * value / stack,
                        Math.Abs(difference[h])),
                    orientation);
            }
        }
    }
}