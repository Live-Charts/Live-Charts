#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Input;
using LiveCharts.Core.Animations;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Events;

#endregion

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Defines a chart view
    /// </summary>
    public interface IChartView : INotifyPropertyChanged, IDisposable
    {
        // as a suggestion do a Explicit implementation
        // of the following events, these events are used by the core
        // of the library and they are not necessary for the user.
        #region Explicit Implementation

        /// <summary>
        /// Occurs when the charts is initialized.
        /// </summary>
        event ChartEventHandler ChartViewLoaded;

        /// <summary>
        /// Occurs when the chart is resized.
        /// </summary>
        event ChartEventHandler ChartViewResized;

        /// <summary>
        /// Occurs when the pointer moves.
        /// </summary>
        event PointerHandler PointerMoved;

        /// <summary>
        /// Occurs when the pointer goes down.
        /// </summary>
        event PointerHandler PointerDown;

        #endregion

        /// <summary>
        /// Occurs before a chart update is called.
        /// </summary>
        event ChartEventHandler ChartUpdatePreview;

        /// <summary>
        /// Occurs before a chart update is called.
        /// </summary>
        ICommand ChartUpdatePreviewCommand { get; set; }

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        event ChartEventHandler ChartUpdated;

        /// <summary>
        /// Occurs after a chart update was called.
        /// </summary>
        ICommand ChartUpdatedCommand { get; set; }

        /// <summary>
        /// Occurs when a user places the pointer over a point.
        /// </summary>
        event DataInteractionHandler DataPointerEntered;

        /// <summary>
        /// Gets or sets the data mouse enter command, this command will try to be executed 
        /// when  the user places the pointer over a point.
        /// </summary>
        /// <value>
        /// The data mouse enter command.
        /// </value>
        ICommand DataPointerEnteredCommand { get; set; }

        /// <summary>
        /// Occurs when the user moves the pointer away from a data point.
        /// </summary>
        event DataInteractionHandler DataPointerLeft;

        /// <summary>
        /// Gets or sets the data mouse leave command, this command will try to be executed
        /// when the user leaves moves the pointer away from a data point.
        /// </summary>
        /// <value>
        /// The data mouse leave.
        /// </value>
        ICommand DataPointerLeftCommand { get; set; }

        /// <summary>
        /// Occurs when the user moves the pointer down in a point.
        /// </summary>
        event DataInteractionHandler DataPointerDown;

        /// <summary>
        /// Gets or sets the data pointer down command.
        /// </summary>
        /// <value>
        /// The data pointer down command.
        /// </value>
        ICommand DataPointerDownCommand { get; set; }

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
        float[] ControlSize { get; }

        /// <summary>
        /// Gets the draw margin, this margin indicates the distance every axis has to display 
        /// its labels.
        /// </summary>
        /// <value>
        /// The draw margin.
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
        IEnumerable<ISeries> Series { get; set; }

        /// <summary>
        /// Gets or sets the state of the updater.
        /// </summary>
        /// <value>
        /// The state of the updater.
        /// </value>
        UpdaterStates UpdaterState { get; set; }

        /// <summary>
        /// Gets or sets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        TimeSpan AnimationsSpeed { get; set; }

        /// <summary>
        /// Gets or sets the animation line.
        /// </summary>
        /// <value>
        /// The animation line.
        /// </value>
        IEnumerable<Frame> AnimationLine { get; set; }

        /// <summary>
        /// Gets or sets the tooltip time out.
        /// </summary>
        /// <value>
        /// The tooltip time out.
        /// </value>
        TimeSpan TooltipTimeOut { get; set; }

        /// <summary>
        /// Gets the legend.
        /// </summary>
        /// <value>
        /// The legend.
        /// </value>
        ILegend Legend { get; set; }

        /// <summary>
        /// Gets the legend position.
        /// </summary>
        /// <value>
        /// The legend position.
        /// </value>
        LegendPosition LegendPosition { get; set; }

        /// <summary>
        /// Gets the data tooltip.
        /// </summary>
        /// <value>
        /// The data tooltip.
        /// </value>
        IDataToolTip DataToolTip { get; set; }

        /// <summary>
        /// Updates the chart manually.
        /// </summary>
        void ForceUpdate(bool restartAnimations = false);

        /// <summary>
        /// Dispatchers the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        void InvokeOnUiThread(Action action);
    }
}
