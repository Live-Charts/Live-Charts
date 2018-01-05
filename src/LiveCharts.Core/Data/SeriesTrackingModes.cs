namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Defines series tracking modes.
    /// </summary>
    public enum SeriesTrackingModes
    {
        /// <summary>
        /// Tells the series to track every chart point by reference, only supported if the chart point instance is a reference type.
        /// </summary>
        ByReference,
        /// <summary>
        /// Tells the series to track every chart point by the index of the item in the array.
        /// </summary>
        ByIndex
    }
}