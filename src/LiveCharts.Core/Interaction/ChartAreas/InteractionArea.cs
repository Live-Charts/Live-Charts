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

using System.Drawing;
using LiveCharts.Core.Interaction.Controls;

namespace LiveCharts.Core.Interaction.ChartAreas
{
    /// <summary>
    /// The interaction area class.
    /// </summary>
    public abstract class InteractionArea
    {
        /// <summary>
        /// Determines whether this area contains the given n dimensions point.
        /// </summary>
        /// <param name="pointerLocation">The dimensions.</param>
        /// <param name="selectionMode">The selection mode.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified selection model]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool Contains(PointF pointerLocation, ToolTipSelectionMode selectionMode);

        /// <summary>
        /// Gets the distance to.
        /// </summary>
        /// <param name="pointerLocation">The pointer location.</param>
        /// <param name="selectionMode">The selection mode.</param>
        /// <returns></returns>
        public abstract float DistanceTo(PointF pointerLocation, ToolTipSelectionMode selectionMode);
    }
}
