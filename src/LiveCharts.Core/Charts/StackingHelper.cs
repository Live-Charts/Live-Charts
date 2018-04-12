using System.Collections.Generic;
using LiveCharts.Core.Abstractions.DataSeries;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Stacking helper class.
    /// </summary>
    public class BarsGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarsGroup"/> class.
        /// </summary>
        public BarsGroup()
        {
            ByStackingIndexStack = new Dictionary<int, float[]>();
        }

        /// <summary>
        /// Gets or sets the by stacking index stack.
        /// </summary>
        /// <value>
        /// The by stacking index stack.
        /// </value>
        public Dictionary<int, float[]> ByStackingIndexStack { get; set; }

        /// <summary>
        /// Gets or sets the bar series group indexes.
        /// </summary>
        /// <value>
        /// The bar series group indexes.
        /// </value>
        public Dictionary<IBarSeries, int> BarSeriesGroupIndexes { get; set; }

        /// <summary>
        /// Gets or sets the bars count.
        /// </summary>
        /// <value>
        /// The bars count.
        /// </value>
        public int BarsCount { get; set; }
    }
}