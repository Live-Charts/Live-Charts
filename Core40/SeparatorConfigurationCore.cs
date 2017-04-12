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

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public class SeparatorConfigurationCore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorConfigurationCore"/> class.
        /// </summary>
        /// <param name="axis">The axis.</param>
        public SeparatorConfigurationCore(AxisCore axis)
        {
            Axis = axis;
        }

        /// <summary>
        /// Gets or sets the axis.
        /// </summary>
        /// <value>
        /// The axis.
        /// </value>
        public AxisCore Axis { get; set; }

        /// <summary>
        /// Gets or sets if separators are enabled (will be drawn)
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets sepator step, this means the value between each line, use null for auto.
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public AxisOrientation Source { get; set; }
    }
}