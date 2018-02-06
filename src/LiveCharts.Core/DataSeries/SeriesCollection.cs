using System.Collections.Generic;
using LiveCharts.Core.Collections;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// An observable collection of <see cref="BaseSeries"/>.
    /// </summary>
    public class SeriesCollection : PlotableCollection<BaseSeries>
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
        public SeriesCollection(IEnumerable<BaseSeries> source)
            : base(source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesCollection"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public SeriesCollection(IList<BaseSeries> source)
            : base(source)
        {
        }
    }
}
