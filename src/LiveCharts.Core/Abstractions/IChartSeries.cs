using System;
using System.Collections.Generic;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Chart series extraction.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IChartSeries : IDisposable
    {
        /// <summary>
        /// Gets the key, the unique name of this series.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        string Key { get; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        Geometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IChartSeries"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        double StrokeThickness { get; set; }

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
        /// Gets the charts that are using this series.
        /// </summary>
        /// <value>
        /// The used by.
        /// </value>
        IEnumerable<ChartModel> UsedBy { get; }

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="chart"></param>
        void UpdateView(ChartModel chart);

        /// <summary>
        /// Fetches the data.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void Fetch(ChartModel chart);
    }

    /// <summary>
    /// A Chart series extraction.
    /// </summary>
    public interface IChartSeries<in TModel, out TCoordinate, out TViewModel, TChartPoint> : IChartSeries
        where TChartPoint : ChartPoint<TModel, TCoordinate, TViewModel>, new()
    {
        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        Func<TModel, TCoordinate> Mapper { get; }
        
        /// <summary>
        /// Gets or sets the reference tracker.
        /// </summary>
        /// <value>
        /// The reference tracker.
        /// </value>
        IList<TChartPoint> ValueTracker { get; set; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        IEnumerable<TChartPoint> Points { get; }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <value>
        /// The type builder.
        /// </value>
        Func<TModel, TViewModel> PointBuilder { get; }
    }
}
