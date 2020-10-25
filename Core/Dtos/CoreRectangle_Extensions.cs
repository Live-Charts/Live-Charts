namespace LiveCharts.Dtos
{

    /// <summary>
    /// 
    /// </summary>
    public static class CoreRectangle_Extensions
    {
        /// <summary>
        /// Hit test to CoreRectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="hitTestPoint"></param>
        /// <param name="pointSize"></param>
        /// <returns></returns>
        public static bool HitTest(this CoreRectangle rect, CorePoint hitTestPoint, double pointSize)
        {
            return
                (rect.Left - pointSize <= hitTestPoint.X)
                && (rect.Left + rect.Width + pointSize >= hitTestPoint.X)
                && (rect.Top - pointSize <= hitTestPoint.Y)
                && (rect.Top + rect.Height + pointSize >= hitTestPoint.Y);
        }
    }
}
