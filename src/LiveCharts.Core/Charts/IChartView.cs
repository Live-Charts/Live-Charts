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
        float[] ControlSize { get; }

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
        /// Gets or sets a value indicating whether this <see cref="IChartView"/> is hoverable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if hoverable; otherwise, <c>false</c>.
        /// </value>
        bool Hoverable { get; set; }

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
        void Update(bool restartAnimations = false);

        /// <summary>
        /// Sets the draw area.
        /// </summary>
        /// <param name="drawArea">The draw area.</param>
        void SetDrawArea(RectangleF drawArea);

        /// <summary>
        /// Dispatchers the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        void InvokeOnUiThread(Action action);
    }
}
