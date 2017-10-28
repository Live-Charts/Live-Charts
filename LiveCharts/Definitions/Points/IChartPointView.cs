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
using LiveCharts.Dtos;

namespace LiveCharts.Definitions.Points
{

    /// <summary>
    /// 
    /// </summary>
    public interface IChartPointView
    {
        /// <summary>
        /// Gets a value indicating whether this instance is new.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
        bool IsNew { get; }
        /// <summary>
        /// Gets the valid area.
        /// </summary>
        /// <value>
        /// The valid area.
        /// </value>
        CoreRectangle ValidArea { get; }
        /// <summary>
        /// Draws the or move.
        /// </summary>
        /// <param name="previousDrawn">The previous drawn.</param>
        /// <param name="current">The current.</param>
        /// <param name="index">The index.</param>
        /// <param name="chart">The chart.</param>
        void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart);
        /// <summary>
        /// Removes from view.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void RemoveFromView(ChartCore chart);
        /// <summary>
        /// Called when [hover].
        /// </summary>
        /// <param name="point">The point.</param>
        void OnHover(ChartPoint point);
        /// <summary>
        /// Called when [hover leave].
        /// </summary>
        /// <param name="point">The point.</param>
        void OnHoverLeave(ChartPoint point);
    }

}
