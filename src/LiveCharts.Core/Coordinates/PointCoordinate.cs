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
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.Dimensions;

#endregion

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A point coordinate.
    /// </summary>
    public class PointCoordinate : ICoordinate
    {
        private readonly float[][] _vector = new float[2][];

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCoordinate"/> struct.
        /// </summary>
        public PointCoordinate(float x, float y)
        {
            _vector[0] = new[] {x};
            _vector[1] = new[] {y};
        }

        /// <inheritdoc />
        public float[] this[int dimension] => _vector[dimension];

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public float X => _vector[0][0];

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public float Y => _vector[1][0];

        /// <inheritdoc />
        public void CompareDimensions(IDataFactoryContext context)
        {
            var x = context.UpdateContext.RangeByDimension[0];
            var y = context.UpdateContext.RangeByDimension[1];

            if (X > x[1]) x[1] = X; // 0: min, 1: Max
            if (X < x[0]) x[0] = X;
            if (Y > y[1]) y[1] = Y;
            if (Y < y[0]) y[0] = Y;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                // x dimension:
                // dimensions[0]
                dimensions[0].FormatValue(X),

                // y dimension
                // dimensions[1]
                dimensions[1].FormatValue(Y)
            };
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static PointCoordinate operator +(PointCoordinate p1, PointCoordinate p2)
        {
            return new PointCoordinate(p1.X+ p2.X, p1.Y+p2.Y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static PointCoordinate operator -(PointCoordinate p1, PointCoordinate p2)
        {
            return new PointCoordinate(p1.X - p2.X, p1.Y - p2.Y);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
