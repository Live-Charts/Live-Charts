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
using System.Windows.Input;
using LiveCharts.Core.DataSeries.Data;

#endregion

namespace LiveCharts.Wpf.Interaction
{
    /// <summary>
    /// User interaction with chart data event arguments.
    /// </summary>
    /// <seealso cref="MouseButtonEventArgs" />
    public class DataInteractionEventArgs : MouseButtonEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataInteractionEventArgs"/> class.
        /// </summary>
        /// <param name="mouse">The mouse.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="button">The button.</param>
        /// <param name="points">The points.</param>
        public DataInteractionEventArgs(
            MouseDevice mouse, 
            int timestamp, 
            MouseButton button, 
            IEnumerable<PackedPoint> points) 
            : base(mouse, timestamp, button)
        {
            Points = points;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInteractionEventArgs"/> class.
        /// </summary>
        /// <param name="mouse">The mouse.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="button">The button.</param>
        /// <param name="stylusDevice">The stylus device.</param>
        /// <param name="points">The points.</param>
        public DataInteractionEventArgs(
            MouseDevice mouse, 
            int timestamp, 
            MouseButton button, 
            StylusDevice stylusDevice, 
            IEnumerable<PackedPoint> points) 
            : base(mouse, timestamp, button, stylusDevice)
        {
            Points = points;
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public IEnumerable<PackedPoint> Points { get; }
    }
}
