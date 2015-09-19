namespace Charts
{
    public enum LineChartLineType
    {
        /// <summary>
        /// Use no line
        /// </summary>
        None,
        /// <summary>
        /// Uses an aproximation algorithm to make soft curves that passes by all points in serie.
        /// </summary>
        Bezier,
        /// <summary>
        /// Line that passes exactly by all points.
        /// </summary>
        Polyline
    }
}