using System.Collections.Generic;
using LiveCharts.Core.Collections;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// A <see cref="PlotableCollection{T}"/> of <see cref="DataSet"/>.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Collections.PlotableCollection{DataSet}" />
    public class SeriesCollection : PlotableCollection<DataSet>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesCollection"/> class.
        /// </summary>
        public SeriesCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesCollection"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public SeriesCollection(IEnumerable<DataSet> source)
            : base(source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesCollection"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public SeriesCollection(IList<DataSet> source)
            : base(source)
        {
        }
    }
}
