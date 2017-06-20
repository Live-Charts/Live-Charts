using System.Collections.Generic;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAxisWindow
    {
        /// <summary>
        /// Gets the minimum reserved space for separators
        /// </summary>
        double MinimumSeparatorWidth { get; }

        /// <summary>
        /// Determines whether a dateTime is a header
        /// </summary>
        bool IsHeader(double x);

        /// <summary>
        /// Gets or sets a function to determine whether a dateTime is a separator
        /// </summary>
        bool IsSeparator(double x);

        /// <summary>
        /// Gets or sets a function to format the label for the axis
        /// </summary>
        string FormatAxisLabel(double x);

        bool TryGetSeparatorIndices(IEnumerable<double> indices, int maximumSeparatorcount, out IEnumerable<double> separatorIndices);
    }
}