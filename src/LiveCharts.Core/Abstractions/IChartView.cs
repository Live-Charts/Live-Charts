using System;
using System.Collections.Generic;
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
        /// Gets the chart model.
        /// </summary>
        /// <value>
        /// The chart model.
        /// </value>
        ChartModel ChartModel { get; }

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
        Margin DrawMargin { get; set; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        IList<IList<Plane>> AxisArrayByDimension { get; }

        /// <summary>
        /// Gets the visible series in the chart, this property must be thread-safe.
        /// </summary>
        /// <value>
        /// The actual series.
        /// </value>
        IList<IChartSeries> Series { get; }

        /// <summary>
        /// Gets a value indicating whether animations are disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if animations are disabled otherwise, <c>false</c>.
        /// </value>
        bool DisableAnimations { get; }

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
        IChartLegend Legend { get; }

        /// <summary>
        /// Occurs when the charts is initialized.
        /// </summary>
        event Action ChartViewInitialized;

        /// <summary>
        /// Occurs when the reference of a property related to LiveCharts API changes.
        /// </summary>
        event PropertyInstanceChangedHandler PropertyInstanceChanged;

        /// <summary>
        /// Occurs when the chart <see cref="AnimationsSpeed"/> property or <see cref="DisableAnimations"/> property change.
        /// </summary>
        event ChartUpdaterfrequencyChangedHandler UpdaterFrequencyChanged;
    }
}
