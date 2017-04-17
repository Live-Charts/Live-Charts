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
    /// 
    /// </summary>
    public class BezierData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BezierData"/> class.
        /// </summary>
        public BezierData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierData"/> class.
        /// </summary>
        /// <param name="point">The point.</param>
        public BezierData(CorePoint point)
        {
            Point1 = point;
            Point2 = point;
            Point3 = point;
        }

        /// <summary>
        /// Gets or sets the point1.
        /// </summary>
        /// <value>
        /// The point1.
        /// </value>
        public CorePoint Point1 { get; set; }

        /// <summary>
        /// Gets or sets the point2.
        /// </summary>
        /// <value>
        /// The point2.
        /// </value>
        public CorePoint Point2 { get; set; }
        /// <summary>
        /// Gets or sets the point3.
        /// </summary>
        /// <value>
        /// The point3.
        /// </value>
        public CorePoint Point3 { get; set; }
        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        /// <value>
        /// The start point.
        /// </value>
        public CorePoint StartPoint { get; set; }
    }
}
