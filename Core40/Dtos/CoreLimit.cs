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

namespace LiveCharts.Dtos
{
    /// <summary>
    /// Defines a portable limit
    /// </summary>
    public struct CoreLimit
    {
        /// <summary>
        /// Initializes a new instance of CoreLimit
        /// </summary>
        /// <param name="min">minimum value</param>
        /// <param name="max">maximum value</param>
        public CoreLimit(double min, double max) : this()
        {
            Max = max;
            Min = min;
        }

        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
        public double Max { get; set; }
        /// <summary>
        /// Gets or sets the minimum value
        /// </summary>
        public double Min { get; set; }
        /// <summary>
        /// Gets the range between max and min values
        /// </summary>
        public double Range { get { return Max - Min; } }
    }
}