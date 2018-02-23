using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;

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
        event ChartEventHandler ChartViewLoaded;

        /// <summary>
        /// Occurs when the chart is resized.
        /// </summary>
        event ChartEventHandler ChartViewResized;

        /// <summary>
        /// Occurs when pointer moves.
        /// </summary>
        event PointerMovedHandler PointerMoved;

        /// <summary>
        /// Occurs before a chart update is called.
        /// </summary>
        event ChartEventHandler UpdatePreview;

        /// <summary>
        /// Occurs before a chart update is called.
        /// </summary>
        ICommand UpdatePreviewCommand { get; set; }

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        event ChartEventHandler Updated;

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        ICommand UpdatedCommand { get; set; }

        /// <summary>
        /// Gets the chart model.
        /// </summary>
        /// <value>
        /// The chart model.
        /// </value>
        ChartModel Model { get; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        IChartContent Content { get; set; }
        
        /// <summary>
        /// Gets the size of the control.
        /// </summary>
        /// <value>
        /// The size of the control.
        /// </value>
        double[] ControlSize { get; }

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
        IEnumerable<DataSet> Series { get; }

        /// <summary>
        /// Gets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        TimeSpan AnimationsSpeed { get; }

        /// <summary>
        /// Gets the tooltip time out.
        /// </summary>
        /// <value>
        /// The tooltip time out.
        /// </value>
        TimeSpan TooltipTimeOut { get; }

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
        LegendPosition LegendPosition { get; }

        /// <summary>
        /// Gets the data tooltip.
        /// </summary>
        /// <value>
        /// The data tooltip.
        /// </value>
        IDataToolTip DataToolTip { get; }

        /// <summary>
        /// Dispatchers the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        void InvokeOnUiThread(Action action);
    }
}
