using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.DataSeries;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// The update context class lives as the update is done once the chart is updated, all the resources consumed by 
    /// this class update should be released, it should also lazy load the resources as the series needs.
    /// </summary>
    public class UpdateContext : IDisposable
    {
        private IEnumerable<Series> _series;
        private Dictionary<int, List<IBarSeries>> _barsCache = new Dictionary<int, List<IBarSeries>>();
        private double _maxPushOut = double.NaN;
        private Dictionary<int, Dictionary<int, float[]>> _stacking;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateContext"/> class.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="chartDimensions">The chart supported dimensions.</param>
        public UpdateContext(IEnumerable<Series> series, int chartDimensions)
        {
            _series = series;
            _stacking = new Dictionary<int, Dictionary<int, float[]>>();
            var dims = new List<float[]>();
            for (var i = 0; i < chartDimensions; i++)
            {
                dims.Add(new[] {float.MaxValue, float.MinValue});
            }
            RangeByDimension = dims.ToArray();
        }

        /// <summary>
        /// Gets the ranges by dimension.
        /// </summary>
        /// <value>
        /// The ranges by dimension.
        /// </value>
        public float[][] RangeByDimension { get; private set; }

        /// <summary>
        /// Gets the stack.
        /// </summary>
        /// <param name="groupingIndex">Index of the grouping.</param>
        /// <param name="itemKey">The item key.</param>
        /// <param name="isPositive">if set to <c>true</c> [is positive].</param>
        /// <returns></returns>
        public float GetStack(int groupingIndex, int itemKey, bool isPositive)
        {
            var i = isPositive ? 0 : 1;
            return _stacking[groupingIndex][itemKey][i];
        }

        /// <summary>
        /// Stacks the specified grouping index.
        /// </summary>
        /// <param name="groupingIndex">Index of the grouping.</param>
        /// <param name="itemKey">The item key.</param>
        /// <param name="value">The value to stack.</param>
        /// <returns></returns>
        public StackResult Stack(int groupingIndex, int itemKey, float value)
        {
            if (!_stacking.TryGetValue(groupingIndex, out var byGroupingIndexDictionary))
            {
                byGroupingIndexDictionary = new Dictionary<int, float[]>();
                _stacking.Add(groupingIndex, byGroupingIndexDictionary);
            }

            if (!byGroupingIndexDictionary.TryGetValue(itemKey, out var group))
            {
                group = new[] {0f, 0f};
                byGroupingIndexDictionary.Add(itemKey, group);
            }

            var i = value >= 0 ? 0 : 1;

            var from = group[i];
            group[i] += value;
            var to = group[i];

            return new StackResult
            {
                From = from,
                To = to
            };
        }

        /// <summary>
        /// Gets the bars count.
        /// </summary>
        /// <param name="scalingIndex">Index of the scaling.</param>
        /// <returns></returns>
        public int GetBarsCount(int scalingIndex)
        {
            return GetBarsFromCache(scalingIndex).Count;
        }

        /// <summary>
        /// Gets the index of the bar.
        /// </summary>
        /// <param name="scalingIndex">Index of the scaling.</param>
        /// <param name="series">The series.</param>
        /// <returns></returns>
        public int GetBarIndex(int scalingIndex, IBarSeries series)
        {
            return GetBarsFromCache(scalingIndex).IndexOf(series);
        }

        /// <summary>
        /// Gets the maximum push out.
        /// </summary>
        /// <returns></returns>
        public double GetMaxPushOut()
        {
            if (double.IsNaN(_maxPushOut))
            {
                _maxPushOut = _series.OfType<IPieSeries>().Select(series => series.PushOut).DefaultIfEmpty(0).Max();
            }
            return _maxPushOut;
        }

        private List<IBarSeries> GetBarsFromCache(int key)
        {
            if (_barsCache.TryGetValue(key, out var barsSharedOnPlane))
            {
                return barsSharedOnPlane;
            }

            barsSharedOnPlane = _series.OfType<IBarSeries>().Where(x => x.ScalesAt[1] == key).ToList();
            _barsCache.Add(key, barsSharedOnPlane);

            return barsSharedOnPlane;
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _series = null;
            _barsCache = null;
            _stacking = null;
            RangeByDimension = null;
        }
    }
}
