namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// The Slice view model.
    /// </summary>
    public struct SliceViewModel
    {
        /// <summary>
        /// Gets the outer radius.
        /// </summary>
        /// <value>
        /// The outer radius.
        /// </value>
        public float OuterRadius { get; internal set; }

        /// <summary>
        /// Gets the inner radius.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        public float InnerRadius { get; internal set; }

        /// <summary>
        /// Gets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public float Rotation { get; internal set; }

        /// <summary>
        /// Gets the wedge.
        /// </summary>
        /// <value>
        /// The wedge.
        /// </value>
        public float Wedge { get; set; }
    }
}
