using Windows.Foundation;

namespace LiveCharts.Uwp.Points
{
    /// <summary>
    /// 
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// Offsets the specified x.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public static void Offset(this Point point, double x, double y)
        {
            point.X += x;
            point.Y += y;
        }
    }
}
