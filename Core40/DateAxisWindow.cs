using System;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public class DateAxisWindow
    {
        /// <summary>
        /// Gets or sets a value indicating the resolution represented by this window
        /// </summary>
        public SeparatorResolution Resolution { get; set; }

        /// <summary>
        /// Gets or sets a value representing the threshold for this window
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// Gets or sets a function to determine whether a dateTime is a header
        /// </summary>
        public Func<DateTime, bool> IsHeader { get; set; }

        /// <summary>
        /// Gets or sets a function to determine whether a dateTime is a separator
        /// </summary>
        public Func<DateTime, bool> IsSeparator { get; set; }

        /// <summary>
        /// Gets or sets a function to format the label for the axis
        /// </summary>
        public Func<DateTime, string> AxisLabel { get; set; }
    }
}