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

namespace LiveCharts.Uwp
{
    /// <summary>
    /// Contains an already defined collection of geometries, useful to set the Series.PointGeomety property
    /// </summary>
    public static class DefaultGeometries
    {
        /// <summary>
        /// Gets the none.
        /// </summary>
        /// <value>
        /// The none.
        /// </value>
        public static PointGeometry None => null;
        /// <summary>
        /// Gets the circle.
        /// </summary>
        /// <value>
        /// The circle.
        /// </value>
        public static PointGeometry Circle => new PointGeometry("M 0,0 A 180,180 180 1 1 1,1 Z");
        /// <summary>
        /// Gets the square.
        /// </summary>
        /// <value>
        /// The square.
        /// </value>
        public static PointGeometry Square => new PointGeometry("M 1,1 h -2 v -2 h 2 z");
        /// <summary>
        /// Gets the diamond.
        /// </summary>
        /// <value>
        /// The diamond.
        /// </value>
        public static PointGeometry Diamond => new PointGeometry("M 1,0 L 2,1  1,2  0,1 z");
        /// <summary>
        /// Gets the triangle.
        /// </summary>
        /// <value>
        /// The triangle.
        /// </value>
        public static PointGeometry Triangle => new PointGeometry("M 0,1 l 1,1 h -2 Z");
        /// <summary>
        /// Gets the cross.
        /// </summary>
        /// <value>
        /// The cross.
        /// </value>
        public static PointGeometry Cross => new PointGeometry("M0, 0 L1, 1 M0, 1 l1, -1");
    }
}
