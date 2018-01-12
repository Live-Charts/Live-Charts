using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Point factory options class.
    /// </summary>
    public class DataFactoryArgs<TModel, TCoordinate, TViewModel, TPoint>
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public IList<TModel> Collection { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public ISeries<TModel, TCoordinate, TViewModel, TPoint> Series { get; set; }

        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public ChartModel Chart { get; set; }

        /// <summary>
        /// Gets or sets the property changed event handler.
        /// </summary>
        /// <value>
        /// The property changed event handler.
        /// </value>
        public PropertyChangedEventHandler PropertyChangedEventHandler { get; set; }
    }
}