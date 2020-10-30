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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point"></param>
        public static void Merge(this CoreRectangle rect, CorePoint point)
        {
            if(rect.Left > point.X)
            {
                rect.Width += (rect.Left - point.X);
                rect.Left = point.X;
            }
            else if (rect.Left + rect.Width < point.X)
            {
                rect.Width = point.X - rect.Left;
            }


            if (rect.Top > point.Y)
            {
                rect.Height += (rect.Top - point.Y);
                rect.Top = point.Y;
            }
            else if (rect.Top + rect.Height < point.Y)
            {
                rect.Height = point.Y - rect.Top;
            }

        }

    }
}
