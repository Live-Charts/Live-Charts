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

using System.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Interaction.Controls;

#endregion

namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// A bezier view model.
    /// </summary>
    public class BezierViewModel
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public PointF Location { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public ICartesianPath Path { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets the size of the geometry.
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        public float GeometrySize { get; set; }

        /// <summary>
        /// Gets or sets the point1.
        /// </summary>
        /// <value>
        /// The point1.
        /// </value>
        public PointF Point1 { get; set; }

        /// <summary>
        /// Gets or sets the point2.
        /// </summary>
        /// <value>
        /// The point2.
        /// </value>
        public PointF Point2 { get; set; }

        /// <summary>
        /// Gets or sets the point3.
        /// </summary>
        /// <value>
        /// The point3.
        /// </value>
        public PointF Point3 { get; set; }

        /// <summary>
        /// Gets or sets the length of the aprox.
        /// </summary>
        /// <value>
        /// The length of the aprox.
        /// </value>
        public double AproxLength { get; set; }
    }
}