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
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Points;

#endregion

namespace LiveCharts.Core.Interaction.Controls
{
    /// <summary>
    /// Defines a data tool tip.
    /// </summary>
    /// <seealso cref="IResource" />
    public interface IDataToolTip
    {
        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>
        /// The selection mode.
        /// </value>
        ToolTipSelectionMode SelectionMode { get; }

        /// <summary>
        /// Gets a value indicating whether the tooltip will be displayed to the closest point to the pointer position.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [snap to closest]; otherwise, <c>false</c>.
        /// </value>
        bool SnapToClosest { get; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        ToolTipPosition Position { get; }

        /// <summary>
        /// Measures this instance with the selected points.
        /// </summary>
        /// <returns></returns>
        SizeF ShowAndMeasure(IEnumerable<PackedPoint> selected, IChartView chart);

        /// <summary>
        ///  Moves to the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="chart">The chart.</param>
        void Move(PointF location, IChartView chart);

        /// <summary>
        /// Hides from specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        void Hide(IChartView chart);
    }
}