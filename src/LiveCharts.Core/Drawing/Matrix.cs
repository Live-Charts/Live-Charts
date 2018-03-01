namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Vector operations.
    /// </summary>
    public static class Vector
    {
        /// <summary>
        /// Sums 2 bi-dimensional vectors.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float[] Sum2D(float[] v1, float[] v2)
        {
            return new[] {v1[0] + v2[0], v1[1] + v2[1]};
        }

        /// <summary>
        /// Subtracts 2 bi-dimensional vectors.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static float[] Substract2D(float[] v1, float[] v2)
        {
            return new[] {v1[0] + v2[0], v1[1] + v2[1]};
        }
    }
}
