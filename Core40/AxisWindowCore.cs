namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AxisWindowCore : IAxisWindow
    {
        /// <summary>
        /// Gets the minimum reserved space for separators
        /// </summary>
        public abstract double MinimumSeparatorWidth { get; }

        /// <summary>
        /// Determines whether a dateTime is a header
        /// </summary>
        public abstract bool IsHeader(double x);

        /// <summary>
        /// Gets or sets a function to determine whether a dateTime is a separator
        /// </summary>
        public abstract bool IsSeparator(double x);

        /// <summary>
        /// Gets or sets a function to format the label for the axis
        /// </summary>
        public abstract string FormatAxisLabel(double x);
    }
}