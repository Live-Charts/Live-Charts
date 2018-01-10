using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data.Points;
namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Point factory options class.
    /// </summary>
    public class DataFactoryArgs<TModel, TCoordinate, TViewModel, TChartPoint>
        where TChartPoint : ChartPoint<TModel, TCoordinate, TViewModel>, new()
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
        public IChartSeries<TModel, TCoordinate, TViewModel, TChartPoint> Series { get; set; }

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