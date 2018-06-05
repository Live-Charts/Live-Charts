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
using System.Drawing;
using System.Threading.Tasks;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Styles;

#endregion

namespace LiveCharts.Core.Interaction.Controls
{
    /// <summary>
    /// Defines a chart legend.
    /// </summary>
    /// <seealso cref="IResource" />
    public interface ILegend : IResource
    {
        /// <summary>
        /// Measures and places this instance in the UI.
        /// </summary>
        /// <param name="seriesCollection">The series collection.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        float[] Measure(IEnumerable<ISeries> seriesCollection, Orientation orientation, IChartView chart);

        /// <summary>
        /// Moves to the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        void Move(PointF location, IChartView chart);
    }
}