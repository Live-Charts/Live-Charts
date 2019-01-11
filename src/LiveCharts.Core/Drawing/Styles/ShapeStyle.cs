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

#endregion

using LiveCharts.Drawing.Brushes;
using System.Collections.Generic;

namespace LiveCharts.Drawing.Styles
{
    /// <summary>
    /// Defines a shape style.
    /// </summary>
    public class ShapeStyle : LabelStyle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeStyle"/> struct.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="strokeDashArray">The stroke dash array.</param>
        public ShapeStyle(Brush? stroke, Brush? fill, float strokeThickness, IEnumerable<double>? strokeDashArray)
        {
            Stroke = stroke;
            Fill = fill;
            StrokeThickness = strokeThickness;
            StrokeDashArray = strokeDashArray;
        }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Brush? Stroke { get; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness { get; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        public IEnumerable<double>? StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush? Fill { get; }

        /// <summary>
        /// Gets the default shape style.
        /// </summary>
        public static ShapeStyle Default => new ShapeStyle(
            new SolidColorBrush(255, 0, 0, 0),
            new SolidColorBrush(255, 0, 0, 0),
            2,
            null);
    }
}
