using System.Drawing;

namespace LiveCharts.Core.Drawing
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
