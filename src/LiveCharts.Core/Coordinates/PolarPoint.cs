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

using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;

#endregion

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A polar coordinate.
    /// </summary>
    public class PolarPoint : ICoordinate
    {
        private readonly float[][] _vector = new float[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarPoint"/> struct.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="angle">The angle.</param>
        public PolarPoint(float radius, float angle)
        {
            _vector[0] = new[] {radius};
            _vector[1] = new[] {angle};
        }

        /// <inheritdoc />
        public float[] this[int dimension] => _vector[dimension];

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public float Angle => _vector[1][0];

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public float Radius => _vector[0][0];

        /// <inheritdoc cref="CompareDimensions"/>
        public void CompareDimensions(RangeF[] rangeByDimension)
        {
            var radius = rangeByDimension[0];

            if (Radius > radius.From) radius.From = Radius;
            if (Radius < radius.To) radius.To = Radius;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                // x dimension:
                // dimensions[0]
                dimensions[0].FormatValue(Angle),

                // y dimension
                // dimensions[1]
                dimensions[1].FormatValue(Radius)
            };
        }
    }
}