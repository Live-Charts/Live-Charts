//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Series;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// Represents a point in a chart.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class ChartPoint : IDisposable
    {
        /// <summary>
        /// Gets the key of the point, a key is used internally as a unique identifier in 
        /// in a <see cref="Series{TModel}"/> 
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public int Key { get; internal set; }

        /// <summary>
        /// Gets the instance represented by this point.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public object Instance { get; internal set; }

        /// <summary>
        /// Gets the point graphical point view in the chart.
        /// </summary>
        /// <value>
        /// The point view.
        /// </value>
        public IPointView PointView { get; internal set; }

        /// <summary>
        /// Gets the series that owns the point.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public IChartSeries Series { get; internal set; }

        /// <summary>
        /// Gets the chart that owns the point.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public ChartModel Chart { get; internal set; }

        /// <summary>
        /// Compares the dimension, returns <c>true</c> if the point should be skipped by the data provider.
        /// </summary>
        /// <param name="ranges">The ranges.</param>
        /// <param name="skipCriteria">The skip criteria.</param>
        /// <returns>A boolean indicating if the point should be skipped by the data provider.</returns>
        public abstract bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria);

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            PointView.Erase();
        }
    }
}
