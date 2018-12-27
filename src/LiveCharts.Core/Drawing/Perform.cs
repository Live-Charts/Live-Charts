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

#endregion

namespace LiveCharts.Drawing
{
    /// <summary>
    /// Vector operations.
    /// </summary>
    public static class Perform
    {
        /// <summary>
        /// Sums the specified p1.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns></returns>
        public static PointF Sum(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        /// <summary>
        /// Sums 2 bi-dimensional vectors components.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float[] SumEach2D(float[] v1, float[] v2)
        {
            return new[] {v1[0] + v2[0], v1[1] + v2[1]};
        }

        /// <summary>
        /// Subtracts 2 bi-dimensional vectors components.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float[] SubstractEach2D(float[] v1, float[] v2)
        {
            return new[] {v1[0] - v2[0], v1[1] - v2[1]};
        }

        /// <summary>
        /// Multiplies 2 bi-dimensional vectors components.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float[] TimesEach2D(float[] v1, float[] v2)
        {
            return new[] { v1[0] * v2[0], v1[1] * v2[1] };
        }

        /// <summary>
        /// Divides every component in the same order the arguments were passed.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float[] DivideEach2D(float[] v1, float[] v2)
        {
            return new[] { v1[0] / v2[0], v1[1] / v2[1] };
        }

        /// <summary>
        /// Compares 2 vectors and returns the max value for every index.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float[] MaxEach2D(float[] v1, float[] v2)
        {
            return new[]
            {
                v1[0] > v2[0] ? v1[0] : v2[0],
                v1[1] > v2[1] ? v1[1] : v2[1]
            };
        }


        /// <summary>
        /// Inverts the X (0) and Y (1) position of a given vector.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static float[] InvertXy(this float[] source)
        {
            return new[] {source[1], source[0]};
        }
    }
}
