//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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

using System.Collections.Generic;
using LiveCharts.Helpers;

namespace LiveCharts
{
    public interface IChartValues : INoisyCollection
    {
        /// <summary>
        /// Gets series points to draw.
        /// </summary>
        IEnumerable<ChartPoint> Points { get; }
        /// <summary>
        /// Gets or sets series that owns the values
        /// </summary>
        SeriesAlgorithm Series { get; set; }
        CoreLimit Limit1 { get; }
        CoreLimit Limit2 { get; }
        CoreLimit Limit3 { get; }
        /// <summary>
        /// Forces values to calculate max, min and index data.
        /// </summary>
        void GetLimits();
        /// <summary>
        /// Initializes the garbage collector
        /// </summary>
        void InitializeGarbageCollector();
        /// <summary>
        /// Removes all unecessary points from the view
        /// </summary>
        void CollectGarbage();
    }
}