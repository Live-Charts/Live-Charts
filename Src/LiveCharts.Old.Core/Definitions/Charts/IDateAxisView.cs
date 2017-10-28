using System;
using LiveCharts.Helpers;

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDateAxisView : IWindowAxisView
    {
        /// <summary>
        /// The datetime used for the first point to calculate relative date values
        /// </summary>
        DateTime InitialDateTime { get; set; }

        /// <summary>
        /// Gets or sets the period used by the series in this axis
        /// </summary>
        PeriodUnits Period { get; set; }
    }
}