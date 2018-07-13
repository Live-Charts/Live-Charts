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

using LiveCharts.Core.Interaction;

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Defines a cartesian chart view.
    /// </summary>
    /// <seealso cref="IChartView" />
    public interface ICartesianChartView : IChartView
    {
        /// <summary>
        /// Gets or sets a value indicating whether the X/Y axis are inverted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the axis is inverted; otherwise, <c>false</c>, 
        /// default is false.
        /// </value>
        bool InvertAxes { get; set; }

        /// <summary>
        /// Gets or sets the zoom, default is Zoom.None.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        Zooming Zooming { get; set; }

        /// <summary>
        /// Gets or sets the zooming speed, the property goes from 0 to 1
        /// where 0 is the fastest and 1 the slowest, any value out of that range will
        /// be caped.
        /// </summary>
        /// <value>
        /// The zooming speed.
        /// </value>
        double ZoomingSpeed { get; set; }

        /// <summary>
        /// Gets or sets the panning, default is Panning.Unset it means that it will be 
        /// based on the <see cref="Zooming"/> property.
        /// </summary>
        /// <value>
        /// The panning.
        /// </value>
        Panning Panning { get; set; }
    }
}