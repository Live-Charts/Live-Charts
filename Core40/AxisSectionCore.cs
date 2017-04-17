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

using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public class AxisSectionCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisSectionCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="chart">The chart.</param>
        public AxisSectionCore(IAxisSectionView view, ChartCore chart)
        {
            View = view;
            Chart = chart;
        }
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public IAxisSectionView View { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public AxisOrientation Source { get; set; }
        /// <summary>
        /// Gets or sets the index of the axis.
        /// </summary>
        /// <value>
        /// The index of the axis.
        /// </value>
        public int AxisIndex { get; set; }
        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public ChartCore Chart { get; set; }
    }
}
