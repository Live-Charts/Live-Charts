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
using LiveCharts.Definitions.Series;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SeriesAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected SeriesAlgorithm(ISeriesView view)
        {
            View = view;
        }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public ISeriesView View { get; set; }
        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public ChartCore Chart { get; set; }
        /// <summary>
        /// Gets or sets the series collection.
        /// </summary>
        /// <value>
        /// The series collection.
        /// </value>
        public SeriesCollection SeriesCollection { get; set; }
        /// <summary>
        /// Gets or sets the series orientation.
        /// </summary>
        /// <value>
        /// The series orientation.
        /// </value>
        public SeriesOrientation SeriesOrientation { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets the preferred selection mode.
        /// </summary>
        /// <value>
        /// The preferred selection mode.
        /// </value>
        public TooltipSelectionMode PreferredSelectionMode { get; internal set; }

        /// <summary>
        /// Gets the current x axis.
        /// </summary>
        /// <value>
        /// The current x axis.
        /// </value>
        public AxisCore CurrentXAxis
        {
            get { return Chart.AxisX[View.ScalesXAt]; }
        }
        /// <summary>
        /// Gets the current y axis.
        /// </summary>
        /// <value>
        /// The current y axis.
        /// </value>
        public AxisCore CurrentYAxis
        {
            get { return Chart.AxisY[View.ScalesYAt]; }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public abstract void Update();
    }
}
