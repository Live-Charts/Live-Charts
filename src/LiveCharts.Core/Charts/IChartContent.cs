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

using System.Drawing;
using LiveCharts.Core.Interaction.Events;

#endregion

namespace LiveCharts.Core.Charts
{
    /// <summary>
    /// Defines the chart content view.
    /// </summary>
    public interface IChartContent
    {
        // as a suggestion do a Explicit implementation
        // of the following events, these events are used by the core
        // of the library and they are not necessary for the user.
        #region Explicit implementation events

        /// <summary>
        /// Occurs when the pointer moves.
        /// </summary>
        event PointerHandler PointerMoved;

        /// <summary>
        /// Occurs when the pointer goes down.
        /// </summary>
        event PointerHandler PointerDown;

        #endregion

        /// <summary>
        /// Gets or sets the draw area.
        /// </summary>
        /// <value>
        /// The draw area.
        /// </value>
        RectangleF DrawArea { get; set; }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        /// <value>
        /// The size of the control.
        /// </value>
        SizeF ControlSize { get; set; }

        /// <summary>
        /// Adds a child child.
        /// </summary>
        void AddChild(object child, bool isClipped);

        /// <summary>
        /// Removes the child.
        /// </summary>
        void DisposeChild(object child, bool isClipped);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
    }
}