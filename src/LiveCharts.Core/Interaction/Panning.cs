namespace LiveCharts.Interaction
{
    /// <summary>
    /// The panning options.
    /// </summary>
    public enum Panning
    {
        /// <summary>
        /// By default chart Panning is Unset, this means it will be based on the chart zoom property.
        /// </summary>
        Unset,
        /// <summary>
        /// Disables panning.
        /// </summary>
        None,
        /// <summary>
        /// Panning only in the X axis
        /// </summary>
        X,
        /// <summary>
        /// Panning only in the Y axis
        /// </summary>
        Y,
        /// <summary>
        /// Panning in both X and Y axes
        /// </summary>
        Xy
    }
}