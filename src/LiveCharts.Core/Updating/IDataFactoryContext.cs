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

using System;
using LiveCharts.Core.Charts;

#endregion

namespace LiveCharts.Core.Updating
{
    /// <summary>
    /// An abstraction of the data factory context.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataFactoryContext : IDisposable
    {
        /// <summary>
        /// Gets the series scales at.
        /// </summary>
        /// <value>
        /// The series scales at.
        /// </value>
        int[] SeriesScalesAt { get; }

        /// <summary>
        /// Gets the index of the grouping.
        /// </summary>
        /// <value>
        /// The index of the grouping.
        /// </value>
        int SeriesGroupingIndex { get; }

        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        ChartModel Chart { get; }

        /// <summary>
        /// Gets the update context.
        /// </summary>
        /// <value>
        /// The update context.
        /// </value>
        UpdateContext UpdateContext { get; }
    }
}