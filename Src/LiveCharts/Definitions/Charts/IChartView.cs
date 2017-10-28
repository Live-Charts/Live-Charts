//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using LiveCharts.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Events;

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChartView
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        ChartCore Model { get; }

        /// <summary>
        /// Occurs when [data click].
        /// </summary>
        event DataClickHandler DataClick;

        /// <summary>
        /// Occurs when [data hover]
        /// </summary>
        event DataHoverHandler DataHover;

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        SeriesCollection Series { get; set; }
        /// <summary>
        /// Gets the actual series.
        /// </summary>
        /// <value>
        /// The actual series.
        /// </value>
        IEnumerable<ISeriesView> ActualSeries { get; }
        /// <summary>
        /// Gets or sets the tooltip timeout.
        /// </summary>
        /// <value>
        /// The tooltip timeout.
        /// </value>
        TimeSpan TooltipTimeout { get; set; }
        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        ZoomingOptions Zoom { get; set; }
        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        PanningOptions Pan { get; set; }
        /// <summary>
        /// Gets or sets the zooming speed.
        /// </summary>
        /// <value>
        /// The zooming speed.
        /// </value>
        double ZoomingSpeed { get; set; }
        /// <summary>
        /// Gets or sets the legend location.
        /// </summary>
        /// <value>
        /// The legend location.
        /// </value>
        LegendLocation LegendLocation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [disable animations].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable animations]; otherwise, <c>false</c>.
        /// </value>
        bool DisableAnimations { get; set; }
        /// <summary>
        /// Gets or sets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        TimeSpan AnimationsSpeed { get; set; }
        /// <summary>
        /// Gets or sets the state of the updater.
        /// </summary>
        /// <value>
        /// The state of the updater.
        /// </value>
        UpdaterState UpdaterState { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has tooltip.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has tooltip; otherwise, <c>false</c>.
        /// </value>
        bool HasTooltip { get; }
        /// <summary>
        /// Gets a value indicating whether this instance has data click event attached.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has data click event attached; otherwise, <c>false</c>.
        /// </value>
        bool HasDataClickEventAttached { get; }
        /// <summary>
        /// Gets a value indicating whether this instance has data hover event attached.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has data hover event attached; otherwise, <c>false</c>.
        /// </value>
        bool HasDataHoverEventAttached { get; }
        /// <summary>
        /// Gets a value indicating whether this <see cref="IChartView"/> is hoverable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if hoverable; otherwise, <c>false</c>.
        /// </value>
        bool Hoverable { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is control loaded.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is control loaded; otherwise, <c>false</c>.
        /// </value>
        bool IsControlLoaded { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is in design mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in design mode; otherwise, <c>false</c>.
        /// </value>
        bool IsInDesignMode { get; }

        /// <summary>
        /// Sets the draw margin top.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetDrawMarginTop(double value);
        /// <summary>
        /// Sets the draw margin left.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetDrawMarginLeft(double value);
        /// <summary>
        /// Sets the height of the draw margin.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetDrawMarginHeight(double value);
        /// <summary>
        /// Sets the width of the draw margin.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetDrawMarginWidth(double value);

        /// <summary>
        /// Adds to view.
        /// </summary>
        /// <param name="element">The element.</param>
        void AddToView(object element);
        /// <summary>
        /// Adds to draw margin.
        /// </summary>
        /// <param name="element">The element.</param>
        void AddToDrawMargin(object element);
        /// <summary>
        /// Removes from view.
        /// </summary>
        /// <param name="element">The element.</param>
        void RemoveFromView(object element);
        /// <summary>
        /// Removes from draw margin.
        /// </summary>
        /// <param name="element">The element.</param>
        void RemoveFromDrawMargin(object element);
        /// <summary>
        /// Ensures the element belongs to current view.
        /// </summary>
        /// <param name="element">The element.</param>
        void EnsureElementBelongsToCurrentView(object element);
        /// <summary>
        /// Ensures the element belongs to current draw margin.
        /// </summary>
        /// <param name="element">The element.</param>
        void EnsureElementBelongsToCurrentDrawMargin(object element);
        
        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        void HideTooltip();
        /// <summary>
        /// Shows the legend.
        /// </summary>
        /// <param name="at">At.</param>
        void ShowLegend(CorePoint at);
        /// <summary>
        /// Hides the legend.
        /// </summary>
        void HideLegend();

        /// <summary>
        /// Loads the legend.
        /// </summary>
        /// <returns></returns>
        CoreSize LoadLegend();
        /// <summary>
        /// Maps the x axes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        List<AxisCore> MapXAxes(ChartCore chart);
        /// <summary>
        /// Maps the y axes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        List<AxisCore> MapYAxes(ChartCore chart);
    }
}