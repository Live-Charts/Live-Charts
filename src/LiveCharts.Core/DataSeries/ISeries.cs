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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;
using LiveCharts.Core.Updating;
using Brush = LiveCharts.Core.Drawing.Brush;
#if NET45 || NET46
using Font = LiveCharts.Core.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The series interface.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref="ISeries" />
    public interface ISeries<TModel, TCoordinate, TViewModel, TSeries> : ISeries<TCoordinate>
        where TCoordinate : ICoordinate
        where TSeries : ISeries
    {
        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        ModelToCoordinateMapper<TModel, TCoordinate> Mapper { get; set; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        IEnumerable<Point<TModel, TCoordinate, TViewModel, TSeries>> Points { get; }

        /// <summary>
        /// Gets the points count.
        /// </summary>
        /// <value>
        /// The points count.
        /// </value>
        int PointsCount { get; }

        /// <summary>
        /// Gets the view provider.
        /// </summary>
        /// <value>
        /// The view provider.
        /// </value>
        ISeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries> ViewProvider { get; set; }

        
    }

    /// <summary>
    /// A series with a defined coordinate.
    /// </summary>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <seealso cref="LiveCharts.Core.DataSeries.ISeries" />
    public interface ISeries<TCoordinate> : ISeries
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Gets or sets the data label formatter, a delegate that will build every DataLabel.
        /// </summary>
        /// <value>
        /// The data label formatter.
        /// </value>
        Func<TCoordinate, string> DataLabelFormatter { get; set; }

        /// <summary>
        /// Gets or sets the tooltip formatter, a delegate that will build the coordinate in the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip formatter.
        /// </value>
        Func<TCoordinate, string> TooltipFormatter { get; set; }
    }


    /// <summary>
    /// The series interface.
    /// </summary>
    public interface ISeries : IResource, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the resource key, the type used to style this element.
        /// </summary>
        /// <value>
        /// The resource key.
        /// </value>
        Type ThemeKey { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        SeriesMetadata Metadata { get; }

        /// <summary>
        /// Gets or sets the values to display in the plot.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        IEnumerable Values { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the series should display a label for every point in the series.
        /// </summary>
        /// <value>
        ///   <c>true</c> if display labels; otherwise, <c>false</c>.
        /// </value>
        bool DataLabels { get; set; }

        /// <summary>
        /// Gets or sets the data labels position.
        /// </summary>
        /// <value>
        /// The data labels position.
        /// </value>
        DataLabelsPosition DataLabelsPosition { get; set; }

        /// <summary>
        /// Gets or sets the data labels foreground.
        /// </summary>
        /// <value>
        /// The data labels foreground.
        /// </value>
        Brush DataLabelsForeground { get; set; }

        /// <summary>
        /// Gets or sets the default fill opacity, this property is used to determine the fill opacity of a point when 
        /// LiveCharts sets the fill automatically based on the theme.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        double DefaultFillOpacity { get; set; }

        /// <summary>
        /// Gets the default width of the point, this property is used internally by the library and should only be used
        ///  by you if you need to build a custom cartesian series.
        /// </summary>
        /// <value>
        /// The default width of the point.
        /// </value>
        float[] DefaultPointWidth { get; }

        /// <summary>
        /// Gets or sets the font, the font will be used as the <see cref="DataLabels"/> font of this series.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        Font DataLabelsFont { get; set; }

        /// <summary>
        /// Gets or sets the geometry, the geometry property is used to represent the series in the legend and
        /// depending on the series type it could also set the base geometry to draw every point (Line, Scatter and Bubble series).
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        Geometry Geometry { get; set; }

        /// <summary>
        /// Gets the series style.
        /// </summary>
        /// <value>
        /// The series style.
        /// </value>
        SeriesStyle Style { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }

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
        /// Gets or sets the delay rule.
        /// </summary>
        /// <value>
        /// The delay rule.
        /// </value>
        DelayRules DelayRule { get; set; }

        /// <summary>
        /// Gets the index of the group, -1 indicates that the series is not grouped.
        /// </summary>
        /// <value>
        /// The index of the group.
        /// </value>
        int GroupingIndex { get; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        Dictionary<ChartModel, Dictionary<string, object>> Content { get; }

        /// <summary>
        /// Gets the point margin, this property is used internally by the library and should only be used
        /// by you if you need to build a custom cartesian series.
        /// </summary>
        /// <value>
        /// The point margin.
        /// </value>
        float PointMargin { get; }

        /// <summary>
        /// Fetches the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="context">The update context.</param>
        void Fetch(ChartModel chart, UpdateContext context);

        /// <summary>
        /// Stores the series as a chart resource.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void UsedBy(ChartModel chart);

        /// <summary>
        /// Gets the interacted points according to a mouse position.
        /// </summary>
        /// <param name="pointerLocation">The pointer location.</param>
        /// <returns></returns>
        IEnumerable<PackedPoint> GetHoveredPoints(PointF pointerLocation);

        /// <summary>
        /// Highlights a point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="chart">The chart view.</param>
        void OnPointHighlight(PackedPoint point, IChartView chart);

        /// <summary>
        /// Removes the highlight of a point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="chart">The chart view.</param>
        void RemovePointHighlight(PackedPoint point, IChartView chart);

        /// <summary>
        /// Gets the label for.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        string GetDataLabel(ICoordinate coordinate);

        /// <summary>
        /// Gets the tooltip label.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns></returns>
        string GetTooltipLabel(ICoordinate coordinate);

        /// <summary>
        /// Updates the started.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void UpdateStarted(IChartView chart);

        /// <summary>
        /// Updates the finished.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void UpdateFinished(IChartView chart);

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="context">The context.</param>
        void UpdateView(ChartModel chart, UpdateContext context);
    }
}
