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

using System.Collections.Generic;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Helpers.INoisyCollection" />
    public interface IChartValues : INoisyCollection
    {
        /// <summary>
        /// Forces values to calculate max, min and index data.
        /// </summary>
        void Initialize(ISeriesView seriesView);

        /// <summary>
        /// Gets the current chart points in the view, the view is required as an argument, because an instance of IChartValues could hold many ISeriesView instances.
        /// </summary>
        /// <param name="seriesView">The series view</param>
        /// <returns></returns>
        IEnumerable<ChartPoint> GetPoints(ISeriesView seriesView);
        /// <summary>
        /// Initializes the garbage collector
        /// </summary>
        void InitializeStep(ISeriesView seriesView);
        /// <summary>
        /// Removes all unnecessary points from the view
        /// </summary>
        void CollectGarbage(ISeriesView seriesView);

        /// <summary>
        /// Gets series that owns the values
        /// </summary>
        PointTracker GetTracker(ISeriesView view);
    }
}