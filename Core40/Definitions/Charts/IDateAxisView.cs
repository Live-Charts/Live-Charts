using System;
using LiveCharts.Helpers;

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDateAxisView : IAxisView
    {
        /// <summary>
        /// The datetime used for the first point to calculate relative date values
        /// </summary>
        DateTime ReferenceDateTime { get; set; }

        /// <summary>
        /// Gets the separator resolution picked by the axis
        /// </summary>
        SeparatorResolution SeparatorResolution { get; }

        /// <summary>
        /// Gets or sets the resolution for the series
        /// </summary>
        SeriesResolution Resolution { get; set; }

        /// <summary>
        /// 
        /// </summary>
        void SetSeparatorResolution(SeparatorResolution resolution);
    }
}