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

using LiveCharts.Core.Drawing.Svg;

#endregion

namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The line series interface.
    /// </summary>
    public interface ILineSeries : ISeries, ICartesianSeries, IStrokeSeries
    {
        /// <summary>
        /// Gets or sets the line smoothness, this property goes from 0 to 1, 0 will generate straight beziers, 1 super curved beziers.
        /// </summary>
        /// <value>
        /// The line smoothness.
        /// </value>
        float LineSmoothness { get; set; }

        /// <summary>
        /// Gets or sets the size of the <see cref="ISeries.Geometry"/> property. 
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        float GeometrySize { get; set; }
    }
}