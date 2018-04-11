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

using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing.Svg;

#endregion

namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// Data Series
    /// </summary>
    public interface ISeries
    {
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
        /// Gets or sets the default fill opacity, this property is used to determine the fill opacity of a point when 
        /// LiveCharts sets the <see cref="Fill"/> automatically based on the theme.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        float DefaultFillOpacity { get; set; }

        /// <summary>
        /// Gets the default width of the point, this property is used internally by the library and should only be used
        ///  by you if you need to build a custom cartesian series.
        /// </summary>
        /// <value>
        /// The default width of the point.
        /// </value>
        float[] DefaultPointWidth { get; }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        Color Fill { get; set; }

        /// <summary>
        /// Gets or sets the font, the font will be used as the <see cref="DataLabels"/> font of this series.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the geometry, the geometry property is used to represent the series in the legend and
        /// depending on the series type it could also set the base geometry to draw every point (Line, Scatter and Bubble series).
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        Geometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        Color Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        float StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        IEnumerable<double> StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }

        /// <summary>
        /// Gets the index of the group, -1 indicates that the series is not grouped.
        /// </summary>
        /// <value>
        /// The index of the group.
        /// </value>
        int GroupingIndex { get; }

        /// <summary>
        /// Gets the content, this property is used internally by the library and should only be used
        /// by you if you need to build a custom series.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        Dictionary<ChartModel, Dictionary<string, object>> Content { get; }
    }
}