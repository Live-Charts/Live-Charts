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
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Updating;

#endregion

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A weighted coordinate.
    /// </summary>
    public class WeightedCoordinate : ICoordinate
    {
        /// <summary>
        /// The _vector.
        /// </summary>
        private readonly float[][] _vector = new float[3][];

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCoordinate"/> struct.
        /// </summary>
        public WeightedCoordinate(float x, float y, float weight)
        {
            _vector[0] = new []{x};
            _vector[1] = new []{y};
            _vector[2] = new[] {weight};
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

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <value>
        /// The w.
        /// </value>
        public float Weight => _vector[2][0];

        /// <inheritdoc />
        public void CompareDimensions(IDataFactoryContext context)
        {
            var x = context.UpdateContext.Ranges[0][context.SeriesScalesAt[0]];
            var y = context.UpdateContext.Ranges[1][context.SeriesScalesAt[1]];
            var w = context.UpdateContext.Ranges[2][context.SeriesScalesAt[2]];

            if (X > x[1]) x[1] = X;
            if (X < x[0]) x[0] = X;
            if (Y > y[1]) y[1] = Y;
            if (Y < y[0]) y[0] = Y;
            if (Weight > w[1]) w[1] = Weight;
            if (Weight < w[0]) w[0] = Weight;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                dimensions[0].FormatValue(X),
                dimensions[1].FormatValue(Y),
                dimensions[2].FormatValue(Weight)
            };
        }
    }
}