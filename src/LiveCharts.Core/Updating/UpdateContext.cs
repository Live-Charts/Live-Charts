﻿#region License
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
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.DataSeries;

#endregion

namespace LiveCharts.Core.Updating
{
    /// <summary>
    /// The update context class lives as the update is done once the chart is updated, all the resources consumed by 
    /// this class update should be released, it should also lazy load the resources as the series needs them.
    /// </summary>
    public class UpdateContext : IDisposable
    {
        private IEnumerable<ISeries> _series;
        private double _maxPushOut = double.NaN;
        private Dictionary<int, BarsGroup> _barsGroups;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateContext"/> class.
        /// </summary>
        /// <param name="series">The series.</param>
        public UpdateContext(IEnumerable<ISeries> series)
        {
            _series = series;
            _barsGroups = new Dictionary<int, BarsGroup>();
            PointLength = new[] {0f, 0f};
            PointMargin = 0d;
        }

        /// <summary>
        /// Gets or sets the ranges [i][j][k], where i is plane index (x, y), j is plane index (x1, x2 .. when multiple axes) and k 0: min, 1: max.
        /// </summary>
        /// <value>
        /// The ranges.
        /// </value>
        public float[][][] Ranges { get; internal set; }

        /// <summary>
        /// Gets the width of the point.
        /// </summary>
        /// <value>
        /// The width of the point.
        /// </value>
        public float[] PointLength { get; internal set; }

        /// <summary>
        /// Gets the point margin.
        /// </summary>
        /// <value>
        /// The point margin.
        /// </value>
        public double PointMargin { get; internal set; }

        /// <summary>
        /// Gets the stack.
        /// </summary>
        /// <param name="stackingIndex">The stacking index.</param>
        /// <param name="scalingKey">The item key.</param>
        /// <param name="isPositiveOrZero">if set to <c>true</c> [is positive].</param>
        /// <returns></returns>
        public float GetStack(int stackingIndex, int scalingKey, bool isPositiveOrZero)
        {
            int i = isPositiveOrZero ? 0 : 1;
            return _barsGroups[scalingKey].ByStackingIndexStack[stackingIndex][i];
        }

        /// <summary>
        /// Stacks the specified grouping index.
        /// </summary>
        /// <param name="stackingIndex">The stacking index.</param>
        /// <param name="scalingKey">The scaling index.</param>
        /// <param name="value">The value to stack.</param>
        /// <returns></returns>
        public StackResult Stack(int stackingIndex, int scalingKey, float value)
        {
            var bars = GetBarsFromCache(scalingKey);

            if (!bars.ByStackingIndexStack.TryGetValue(stackingIndex, out float[] stack))
            {
                stack = new float[2];
                bars.ByStackingIndexStack.Add(stackingIndex, stack);
            }

            int i = value >= 0 ? 0 : 1;

            float current = stack[i];
            float from = current;
            float to = stack[i] = current + value;

            return new StackResult
            {
                From = from,
                To = to
            };
        }

        /// <summary>
        /// Gets the bars count.
        /// </summary>
        /// <param name="scalingKey">Index of the scaling.</param>
        /// <returns></returns>
        public int GetBarsCount(int scalingKey)
        {
            return GetBarsFromCache(scalingKey, true).BarsCount;
        }

        /// <summary>
        /// Gets the index of the bar.
        /// </summary>
        /// <param name="scalingKey">Index of the scaling.</param>
        /// <param name="series">The series to calculate the index of.</param>
        /// <returns></returns>
        public int GetBarIndex(int scalingKey, IBarSeries series)
        {
            return GetBarsFromCache(scalingKey, true).BarSeriesGroupIndexes[series];
        }

        /// <summary>
        /// Gets the maximum push out.
        /// </summary>
        /// <returns></returns>
        public double GetMaxPushOut()
        {
            if (double.IsNaN(_maxPushOut))
            {
                _maxPushOut = _series.OfType<IPieSeries>()
                    .Select(series => series.PushOut)
                    .DefaultIfEmpty(0)
                    .Max();
            }
            return _maxPushOut;
        }

        private BarsGroup GetBarsFromCache(int scalingIndex, bool calculateSeries = false)
        {
            if (!_barsGroups.TryGetValue(scalingIndex, out var group))
            {
                group = new BarsGroup();
                _barsGroups.Add(scalingIndex, group);
            }

            if (calculateSeries && group.BarSeriesGroupIndexes == null)
            {
                Tuple<Dictionary<IBarSeries, int>, int> barsGroup = GroupBars(_series, scalingIndex);
                group.BarSeriesGroupIndexes = barsGroup.Item1;
                group.BarsCount = barsGroup.Item2;
            }

            return group;
        }

        private static Tuple<Dictionary<IBarSeries, int>, int> GroupBars(IEnumerable<ISeries> series, int scalingIndex)
        {
            Dictionary<IBarSeries, int> result = new Dictionary<IBarSeries, int>();
            Dictionary<int, int> keys = new Dictionary<int, int>();

            int barIndex = 0;

            foreach (var s in series)
            {
                if (!(s is IBarSeries barSeries) || barSeries.ScalesAt[1] != scalingIndex) continue;

                if (s.GroupingIndex == -1)
                {
                    result.Add(barSeries, barIndex);
                    barIndex++;
                }
                else
                {
                    if (!keys.TryGetValue(s.GroupingIndex, out int assignedIndex))
                    {
                        assignedIndex = barIndex;
                        keys.Add(s.GroupingIndex, assignedIndex);
                        barIndex++;
                    }

                    result.Add(barSeries, assignedIndex);
                }
            }

            return new Tuple<Dictionary<IBarSeries, int>, int>(result, barIndex);
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _series = null;
            _barsGroups = null;
            PointLength = null;
            Ranges = null;
        }
    }
}
