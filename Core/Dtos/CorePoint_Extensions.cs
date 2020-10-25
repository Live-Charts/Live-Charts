namespace LiveCharts.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public static class CorePoint_Extensions
    {
        /// <summary>
        /// Hit test to CorePoint
        /// </summary>
        /// <param name="point"></param>
        /// <param name="hitTestPoint"></param>
        /// <param name="pointSize"></param>
        /// <returns></returns>
        public static bool HitTest(this CorePoint point, CorePoint hitTestPoint, double pointSize)
        {
            return
                (point.X - pointSize <= hitTestPoint.X)
                && (point.X + pointSize >= hitTestPoint.X)
                && (point.Y - pointSize <= hitTestPoint.Y)
                && (point.Y + pointSize >= hitTestPoint.Y);
        }

    }
}
