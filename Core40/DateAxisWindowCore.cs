using System;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DateAxisWindowCore : AxisWindowCore
    {
        public DateAxisCore DateAxisCore { get; set; }

        public override bool IsHeader(double x)
        {
            var date = DateAxisCore.GetdateTime(x);
            return IsHeader(date);
        }

        public override bool IsSeparator(double x)
        {
            var date = DateAxisCore.GetdateTime(x);
            return IsSeparator(date);
        }

        public override string FormatAxisLabel(double x)
        {
            var date = DateAxisCore.GetdateTime(x);
            return FormatAxisLabel(date);
        }

        /// <summary>
        /// Determines whether a dateTime is a header
        /// </summary>
        public abstract bool IsHeader(DateTime x);

        /// <summary>
        /// Gets or sets a function to determine whether a dateTime is a separator
        /// </summary>
        public abstract bool IsSeparator(DateTime x);

        /// <summary>
        /// Gets or sets a function to format the label for the axis
        /// </summary>
        public abstract string FormatAxisLabel(DateTime x);

    }
}