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
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Styles;
#if NET45 || NET46
using Font = LiveCharts.Core.Interaction.Styles.Font;
#endif

#endregion

namespace LiveCharts.Core.Interaction.Controls
{
    /// <summary>
    /// Defines a control that is able to measure it's size.
    /// </summary>
    public interface IPlaneLabelControl
    {
        /// <summary>
        /// Measures the specified label.
        /// </summary>
        /// <param name="content">The chart content.</param>
        /// <param name="font">The Font.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        SizeF MeasureAndUpdate(IChartContent content, Font font, string label);
    }
}