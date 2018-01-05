using System;
using System.Collections;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Data.Builders
{
    /// <summary>
    /// Point factory options class.
    /// </summary>
    public class DataFactoryArgs
    {
        /// <summary>
        /// Gets or sets the type of the collection items.
        /// </summary>
        /// <value>
        /// The type of the collection items.
        /// </value>
        public Type CollectionItemsType { get; set; }

        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public IList Collection { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public IChartSeries Series { get; set; }

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