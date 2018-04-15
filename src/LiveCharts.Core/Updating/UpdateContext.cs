using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.Abstractions.DataSeries;

namespace LiveCharts.Core.Updating
{
    /// <summary>
    /// The update context class lives as the update is done once the chart is updated, all the resources consumed by 
    /// this class update should be released, it should also lazy load the resources as the series needs.
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
        }

        /// <summary>
        /// Gets or sets the ranges [i][j][k], where i is plane index (x, y), j is plane index (x1, x2 .. when multiple axes) and k 0: min, 1: max.
        /// </summary>
        /// <value>
        /// The ranges.
        /// </value>
        public float[][][] Ranges { get; set; }

        /// <summary>
        /// Gets the stack.
        /// </summary>
        /// <param name="stackingIndex">The stacking index.</param>
        /// <param name="scalingKey">The item key.</param>
        /// <param name="isPositiveOrZero">if set to <c>true</c> [is positive].</param>
        /// <returns></returns>
        public float GetStack(int stackingIndex, int scalingKey, bool isPositiveOrZero)
        {
            var i = isPositiveOrZero ? 0 : 1;
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

            if (!bars.ByStackingIndexStack.TryGetValue(stackingIndex, out var stack))
            {
                stack = new float[2];
                bars.ByStackingIndexStack.Add(stackingIndex, stack);
            }

            var i = value >= 0 ? 0 : 1;

            var current = stack[i];
            var from = current;
            var to = stack[i] = current + value;

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
                var barsGroup = GroupBars(_series, scalingIndex);
                group.BarSeriesGroupIndexes = barsGroup.Item1;
                group.BarsCount = barsGroup.Item2;
            }

            return group;
        }

        private static Tuple<Dictionary<IBarSeries, int>, int> GroupBars(IEnumerable<ISeries> series, int scalingIndex)
        {
            var result = new Dictionary<IBarSeries, int>();
            var keys = new Dictionary<int, int>();

            var barIndex = 0;

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
                    if (!keys.TryGetValue(s.GroupingIndex, out var assignedIndex))
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
            Ranges = null;
        }
    }
}
