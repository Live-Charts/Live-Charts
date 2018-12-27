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

using LiveCharts.Charts;
using LiveCharts.Dimensions;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Styles;

#endregion

namespace LiveCharts.Interaction.Events
{
    /// <summary>
    /// The Cartesian Axis Separator Arguments.
    /// </summary>
    public class CartesianAxisSectionArgs
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="CartesianAxisSectionArgs"/> class.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="plane"></param>
        /// <param name="section"></param>
        /// <param name="style"></param>
        public CartesianAxisSectionArgs(IChartView view, Plane plane, Section section, ShapeStyle style)
        {
            ChartView = view;
            Plane = plane;
            Section = section;
            Style = style;
        }

        /// <summary>
        /// Gets or sets the index of the z.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        public int ZIndex { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CartesianAxisSectionArgs"/> is draggable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if draggable; otherwise, <c>false</c>.
        /// </value>
        public bool Draggable { get; internal set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        internal RectangleViewModel Rectangle { get; set; }

        /// <summary>
        /// Gets or sets the axis label model.
        /// </summary>
        /// <value>
        /// The axis label model.
        /// </value>
        internal AxisSectionViewModel Label { get; set; }

        /// <summary>
        /// Gets the plane.
        /// </summary>
        /// <value>
        /// The plane.
        /// </value>
        public Plane Plane { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CartesianAxisSectionArgs"/> is disposing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposing; otherwise, <c>false</c>.
        /// </value>
        public bool Disposing { get; internal set; }

        /// <summary>
        /// Gets the plane.
        /// </summary>
        /// <value>
        /// The plane.
        /// </value>
        public ShapeStyle Style { get; internal set; }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>
        /// The section.
        /// </value>
        public Section Section { get; internal set; }

        /// <summary>
        /// Gets the chart view.
        /// </summary>
        /// <value>
        /// The chart view.
        /// </value>
        public IChartView ChartView { get; internal set; }
    }
}
