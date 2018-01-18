using System;
using System.Collections.Generic;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart animations speed changed handler.
    /// </summary>
    /// <param name="newValue">The new value.</param>
    public delegate void ChartUpdaterfrequencyChangedHandler(TimeSpan newValue);

    /// <summary>
    /// Defines a property instance change handler.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="propertyName">The property name.</param>
    public delegate void PropertyInstanceChangedHandler(object instance, string propertyName);

    /// <summary>
    /// Defines a chart view
    /// </summary>
    public interface IChartView
    {
        /// <summary>
        /// Occurs when the charts is initialized.
        /// </summary>
        event Action ChartViewLoaded;

        /// <summary>
        /// Occurs when the reference of a property related to LiveCharts API changes.
        /// </summary>
        event PropertyInstanceChangedHandler DataInstanceChanged;

        /// <summary>
        /// Occurs when [updater frequency changed].
        /// </summary>
        event ChartUpdaterfrequencyChangedHandler UpdaterFrequencyChanged;

        /// <summary>
        /// Gets the chart model.
        /// </summary>
        /// <value>
        /// The chart model.
        /// </value>
        ChartModel Model { get; }

        /// <summary>
        /// Gets the size of the control.
        /// </summary>
        /// <value>
        /// The size of the control.
        /// </value>
        Size ControlSize { get; }

        /// <summary>
        /// Gets the size of the draw margin.
        /// </summary>
        /// <value>
        /// The size of the draw margin.
        /// </value>
        Margin DrawMargin { get; }

        /// <summary>
        /// Gets the plane sets.
        /// </summary>
        /// <value>
        /// The plane sets.
        /// </value>
        IList<IList<Plane>> Dimensions { get; }

        /// <summary>
        /// Gets the visible series in the chart, this property must be thread-safe.
        /// </summary>
        /// <value>
        /// The actual series.
        /// </value>
        IEnumerable<Series.Series> Series { get; }

        /// <summary>
        /// Gets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        TimeSpan AnimationsSpeed { get; }

        /// <summary>
        /// Gets the legend.
        /// </summary>
        /// <value>
        /// The legend.
        /// </value>
        ILegend Legend { get; }

        /// <summary>
        /// Gets the legend position.
        /// </summary>
        /// <value>
        /// The legend position.
        /// </value>
        LegendPositions LegendPosition { get; }

        /// <summary>
        /// Updates the draw margin.
        /// </summary>
        /// <param name="model">The model.</param>
        void UpdateDrawArea(Rectangle model);
    }
}
