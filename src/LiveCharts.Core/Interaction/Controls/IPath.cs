﻿#region License
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
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;

#endregion

namespace LiveCharts.Core.Interaction.Controls
{
    /// <summary>
    /// Defines a path.
    /// </summary>
    public interface ICartesianPath
    {
        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="timeLine">The animation.</param>
        void Initialize(IChartView view, TimeLine timeLine);

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="startPoint">The start point of the path.</param>
        /// <param name="toPivot">The location of the pivot at the start point.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="zIndex">The z index.</param>
        /// <param name="strokeDashArray">The stroke dash array.</param>
        void SetStyle(
            PointF startPoint,
            PointF toPivot,
            Drawing.Brush stroke,
            Drawing.Brush fill,
            double strokeThickness,
            int zIndex,
            IEnumerable<double> strokeDashArray);

        /// <summary>
        /// Adds the bezier segment and returns the instance added.
        /// </summary>
        /// <param name="segment">The segment instance.</param>
        /// <param name="index">The index to insert the segment at.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        object InsertSegment(object segment, int index, PointF p1, PointF p2, PointF p3);

        /// <summary>
        /// Removes the segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        void RemoveSegment(object segment);

        /// <summary>
        /// Closes the path.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="pivot">The location of pivot at the end point.</param>
        /// <param name="length">The length.</param>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        void Close(
            IChartView view,
            PointF pivot,
            float length,
            float i,
            float j);

        /// <summary>
        /// Disposes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        void Dispose(IChartView view);
    }
}
