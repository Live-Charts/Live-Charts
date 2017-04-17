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

using System.Collections.Generic;
using LiveCharts.Dtos;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public class PointTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointTracker"/> class.
        /// </summary>
        public PointTracker()
        {
            Indexed = new Dictionary<int, ChartPoint>();
            Referenced = new Dictionary<object, ChartPoint>();
        }

        /// <summary>
        /// Gets the x limit.
        /// </summary>
        /// <value>
        /// The x limit.
        /// </value>
        public CoreLimit XLimit { get; internal set; }
        /// <summary>
        /// Gets the y limit.
        /// </summary>
        /// <value>
        /// The y limit.
        /// </value>
        public CoreLimit YLimit { get; internal set; }
        /// <summary>
        /// Gets the w limit.
        /// </summary>
        /// <value>
        /// The w limit.
        /// </value>
        public CoreLimit WLimit { get; internal set; }
        /// <summary>
        /// Gets the gci.
        /// </summary>
        /// <value>
        /// The gci.
        /// </value>
        public int Gci { get; internal set; }
        /// <summary>
        /// Gets or sets the indexed.
        /// </summary>
        /// <value>
        /// The indexed.
        /// </value>
        public Dictionary<int, ChartPoint> Indexed { get; set; }
        /// <summary>
        /// Gets or sets the referenced.
        /// </summary>
        /// <value>
        /// The referenced.
        /// </value>
        public Dictionary<object, ChartPoint> Referenced { get; set; }
    }
}