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

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISeparatorElementView
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        SeparatorElementCore Model { get; }
        /// <summary>
        /// Gets the label model.
        /// </summary>
        /// <value>
        /// The label model.
        /// </value>
        LabelEvaluation LabelModel { get; }

        /// <summary>
        /// Updates the label.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        LabelEvaluation UpdateLabel(string text, AxisCore axis, AxisOrientation source);

        /// <summary>
        /// Clears the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void Clear(IChartView chart);

        //No animated methods
        /// <summary>
        /// Places the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="axisIndex">Index of the axis.</param>
        /// <param name="toLabel">To label.</param>
        /// <param name="toLine">To line.</param>
        /// <param name="tab">The tab.</param>
        void Place(ChartCore chart, AxisCore axis, AxisOrientation direction, int axisIndex, double toLabel, double toLine, double tab);
        /// <summary>
        /// Removes the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void Remove(ChartCore chart);

        //Animated methods
        /// <summary>
        /// Moves the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="axisIndex">Index of the axis.</param>
        /// <param name="toLabel">To label.</param>
        /// <param name="toLine">To line.</param>
        /// <param name="tab">The tab.</param>
        void Move(ChartCore chart, AxisCore axis, AxisOrientation direction, int axisIndex, double toLabel, double toLine, double tab);
        /// <summary>
        /// Fades the in.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="chart">The chart.</param>
        void FadeIn(AxisCore axis, ChartCore chart);
        /// <summary>
        /// Fades the out and remove.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void FadeOutAndRemove(ChartCore chart);
    }
}