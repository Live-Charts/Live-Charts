namespace LiveCharts.Drawing
{
    /// <summary>
    /// Slice Builder
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public struct S‪liceBuilderViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="S‪liceBuilderViewModel"/> struct.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="isRadiusLargeArc">The is large arc.</param>
        /// <param name="isInnerRadiusLargeArc">The is inner radius large arc.</param>
        public S‪liceBuilderViewModel(PointD[] points, float cornerRadius, bool isRadiusLargeArc, bool isInnerRadiusLargeArc)
        {
            Points = points;
            CornerRadius = cornerRadius;
            IsRadiusLargeArc = isRadiusLargeArc;
            IsInnerRadiusLargeArc = isInnerRadiusLargeArc;
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public PointD[] Points { get; set; }

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public float CornerRadius { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is large arc.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is large arc; otherwise, <c>false</c>.
        /// </value>
        public bool IsRadiusLargeArc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is inner radius large arc.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is inner radius large arc; otherwise, <c>false</c>.
        /// </value>
        public bool IsInnerRadiusLargeArc { get; set; }
    }
}