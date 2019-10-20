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

using System;

namespace LiveCharts
{
    /// <summary>
    /// Describes where a label should be placed
    /// </summary>
    public enum BarLabelPosition
    {
        /// <summary>
        /// Places a label at the top of a bar
        /// </summary>
        Top,
        /// <summary>
        /// Places a labels inside the bar
        /// </summary>
        [Obsolete("Instead use BarLabelPosition.Parallel")]
        Merged,
        /// <summary>
        /// Places a labels in a parallel orientation to the bar height.
        /// </summary>
        Parallel,
        /// <summary>
        /// Places a labels in a perpendicular orientation to the bar height.
        /// </summary>
        Perpendicular
    }
}