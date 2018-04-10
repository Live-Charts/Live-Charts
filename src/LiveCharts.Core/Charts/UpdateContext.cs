using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.DataSeries;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// The update arguments class.
    /// </summary>
    public class UpdateContext : IDisposable
    {
        private IEnumerable<Series> _series;
        private Dictionary<int, List<IBarSeries>> _barsCache = new Dictionary<int, List<IBarSeries>>();
        private double _maxPushOut = double.NaN;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateContext"/> class.
        /// </summary>
        /// <param name="series">The series.</param>
        public UpdateContext(IEnumerable<Series> series)
        {
            _series = series;
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
        }
    }
}
