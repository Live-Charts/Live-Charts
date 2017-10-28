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

using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;

namespace LiveCharts.SeriesAlgorithms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.SeriesAlgorithms.LineAlgorithm" />
    /// <seealso cref="LiveCharts.Definitions.Series.ICartesianSeries" />
    public class VerticalLineAlgorithm : LineAlgorithm, ICartesianSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalLineAlgorithm"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public VerticalLineAlgorithm(ISeriesView view) : base(view)
        {
            SeriesOrientation = SeriesOrientation.Vertical;
            PreferredSelectionMode = TooltipSelectionMode.SharedYValues;
        }

        double ICartesianSeries.GetMinX(AxisCore axis)
        {
            return AxisLimits.SeparatorMin(axis);
        }

        double ICartesianSeries.GetMaxX(AxisCore axis)
        {
            return AxisLimits.SeparatorMaxRounded(axis);
        }

        double ICartesianSeries.GetMinY(AxisCore axis)
        {
            return AxisLimits.StretchMin(axis);
        }

        double ICartesianSeries.GetMaxY(AxisCore axis)
        {
            return AxisLimits.StretchMax(axis);
        }
    }
}
