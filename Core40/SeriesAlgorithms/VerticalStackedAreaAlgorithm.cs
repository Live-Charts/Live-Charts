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

using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.SeriesAlgorithms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.SeriesAlgorithms.StackedAreaAlgorithm" />
    public class VerticalStackedAreaAlgorithm : StackedAreaAlgorithm
    {
        private readonly IStackModelableSeriesView _stackModelable;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalStackedAreaAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public VerticalStackedAreaAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Vertical;
            _stackModelable = (IStackModelableSeriesView) view;
            PreferredSelectionMode = TooltipSelectionMode.SharedYValues;
        }

        /// <summary>
        /// Gets the stacked point.
        /// </summary>
        /// <param name="chartPoint">The chart point.</param>
        /// <returns></returns>
        protected override CorePoint GetStackedPoint(ChartPoint chartPoint)
        {
            if (_stackModelable.StackMode == StackMode.Values)
                return new CorePoint(
                    ChartFunctions.ToDrawMargin(chartPoint.To, AxisOrientation.X, Chart, View.ScalesXAt),
                    ChartFunctions.ToDrawMargin(chartPoint.Y, AxisOrientation.Y, Chart, View.ScalesYAt));

            return new CorePoint(
                ChartFunctions.ToDrawMargin(chartPoint.StackedParticipation, AxisOrientation.X, Chart, View.ScalesXAt),
                ChartFunctions.ToDrawMargin(chartPoint.Y, AxisOrientation.Y, Chart, View.ScalesYAt));
        }
    }
}
