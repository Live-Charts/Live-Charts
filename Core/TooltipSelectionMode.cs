namespace LiveCharts
{
    /// <summary>
    /// Tooltip selection modes
    /// </summary>
    public enum TooltipSelectionMode
    {
        /// <summary>
        /// Gets only the hovered point 
        /// </summary>
        OnlySender,
        /// <summary>
        /// Gets all the points that shares the value X in the chart
        /// </summary>
        SharedXValues,
        /// <summary>
        /// Gets all the points that shares the value Y in the chart
        /// </summary>
        SharedYValues,
        /// <summary>
        /// Gets all the points that shares the value X in the hovered series
        /// </summary>
        SharedXInSeries,
        /// <summary>
        /// Gets all the points that shares the value Y in the hovered series
        /// </summary>
        SharedYInSeries
    }
}