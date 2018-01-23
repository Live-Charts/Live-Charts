using System;
using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a chart view
    /// </summary>
    public interface IChartView : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when the charts is initialized.
        /// </summary>
        event Action ChartViewLoaded;

        /// <summary>
        /// Occurs when the chart is resized.
        /// </summary>
        event Action ChartViewResized;

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
        IEnumerable<Series> Series { get; }

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
