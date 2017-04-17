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
    public struct CorePoint
    {
        /// <summary>
        /// Initializes a new instance of CorePoint
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public CorePoint(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of CorePoint
        /// </summary>
        /// <param name="point">source pont</param>
        public CorePoint(CorePoint point) : this()
        {
            X = point.X;
            Y = point.Y;
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Sums every property between 2 given points
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <returns></returns>
        public static CorePoint operator +(CorePoint p1, CorePoint p2)
        {
            return new CorePoint(p1.X + p2.X, p1.Y + p2.Y);
        }

        /// <summary>
        /// Subtracts every property between 2 given points
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <returns></returns>
        public static CorePoint operator -(CorePoint p1, CorePoint p2)
        {
            return new CorePoint(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
}