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

#endregion

namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// Slice Builder
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public struct S‪liceBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="S‪liceBuilder"/> struct.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="isRadiusLargeArc">The is large arc.</param>
        /// <param name="isInnerRadiusLargeArc">The is inner radius large arc.</param>
        public S‪liceBuilder(PointF[] points, float cornerRadius, bool isRadiusLargeArc, bool isInnerRadiusLargeArc)
        {
            Points = points;
            CornerRadius = cornerRadius;
            IsRadiusLargeArc = isRadiusLargeArc;
            IsInnerRadiusLargeArc = isInnerRadiusLargeArc;
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public PointF[] Points { get; set; }

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public float CornerRadius { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is large arc.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is large arc; otherwise, <c>false</c>.
        /// </value>
        public bool IsRadiusLargeArc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is inner radius large arc.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is inner radius large arc; otherwise, <c>false</c>.
        /// </value>
        public bool IsInnerRadiusLargeArc { get; set; }
    }
}