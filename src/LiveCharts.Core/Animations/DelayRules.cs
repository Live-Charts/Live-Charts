namespace LiveCharts.Animations
{
    /// <summary>
    /// Delay rules.
    /// </summary>
    public enum DelayRules
    {
        /// <summary>
        /// The none
        /// </summary>
        None,
        /// <summary>
        /// The left to right delay.
        /// </summary>
        LeftToRight,
        /// <summary>
        /// The right to left delay.
        /// </summary>
        RightToLeft,
        /// <summary>
        /// The left to right delay using a cubic function.
        /// </summary>
        LeftToRightCubic,
        /// <summary>
        /// The right to left delay using a cubic function.
        /// </summary>
        RightToLeftCubic,
        /// <summary>
        /// The left to right delay using a elastic function.
        /// </summary>
        LeftToRightElastic,
        /// <summary>
        /// The right to left delay using a elastic function.
        /// </summary>
        RightToLeftelastic,
        /// <summary>
        /// The left to right delay using a bounce function.
        /// </summary>
        LeftToRightBounce,
        /// <summary>
        /// The right to left delay using a bounce function.
        /// </summary>
        RightToLeftBounce,
        /// <summary>
        /// The random delay.
        /// </summary>
        Random
    }
}