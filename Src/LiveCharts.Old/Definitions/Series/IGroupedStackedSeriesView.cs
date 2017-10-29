using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCharts.Definitions.Series
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Definitions.Series.ISeriesView" />
    public interface IGroupedStackedSeriesView : ISeriesView
    {
        /// <summary>
        /// Gets or sets the column grouping.
        /// </summary>
        /// <value>
        /// The column grouping.
        /// </value>
        object Grouping { get; set; }
    }
}
