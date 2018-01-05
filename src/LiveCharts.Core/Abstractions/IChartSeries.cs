using System;
using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// A Chart series extraction.
    /// </summary>
    public interface IChartSeries : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="ChartPointTypes"/> value of the chart point.
        /// </summary>
        /// <value>
        /// The type of the chart point.
        /// </value>
        ChartPointTypes ChartPointType { get; }

        /// <summary>
        /// Gets the default fill opacity.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        double DefaultFillOpacity { get; }

        /// <summary>
        /// Gets the skip criteria.
        /// </summary>
        /// <value>
        /// The skip criteria.
        /// </value>
        SeriesSkipCriteria SkipCriteria { get; }

        /// <summary>
        /// Gets the tracking mode.
        /// </summary>
        /// <value>
        /// The tracking mode.
        /// </value>
        SeriesTrackingModes TrackingMode { get; }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <value>
        /// The type builder.
        /// </value>
        IChartingTypeBuilder TypeBuilder { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IChartSeries"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets the stroke color.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        Color Stroke { get; set; }

        /// <summary>
        /// Gets the fill color.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        Color Fill { get; set; }

        /// <summary>
        /// Gets or sets the scales at.
        /// </summary>
        /// <value>
        /// The scales at.
        /// </value>
        int[] ScaleAtByDimension { get; }

        /// <summary>
        /// Gets the data range.
        /// </summary>
        /// <value>
        /// The data range.
        /// </value>
        DimensionRange DataRange { get; }

        /// <summary>
        /// Gets the value point dictionary.
        /// </summary>
        /// <value>
        /// The value point dictionary.
        /// </value>
        Dictionary<ChartModel, Dictionary<object, ChartPoint>> ValuePointDictionary { get; }

        /// <summary>
        /// Calculates the all necessary data to plot the series.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        IEnumerable<ChartPoint> FetchData(ChartModel chart);

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="chart"></param>
        void UpdateView(ChartModel chart);
    }
}
