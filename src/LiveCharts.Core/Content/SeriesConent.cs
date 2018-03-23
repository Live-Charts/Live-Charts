using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.DataSeries.Data;

namespace LiveCharts.Core.Content
{
    /// <summary>
    /// Series visual content.
    /// </summary>
    public class SeriesContent<TModel, TCoordinate, TViewModel, TPoint>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesContent{TModel, TCoordinate, TViewModel, TPoint}"/> class.
        /// </summary>
        public SeriesContent()
        {
            PointTracker = new Dictionary<object, TPoint>();
            Visuals = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the point tracker.
        /// </summary>
        /// <value>
        /// The point tracker.
        /// </value>
        public Dictionary<object, TPoint> PointTracker { get; set; }

        /// <summary>
        /// Gets or sets the extra.
        /// </summary>
        /// <value>
        /// The extra.
        /// </value>
        public Dictionary<string, object> Visuals { get; set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            PointTracker = null;
            Visuals = null;
        }
    }
}
