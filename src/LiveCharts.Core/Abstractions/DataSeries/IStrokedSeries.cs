using System.Collections.Generic;
using System.Drawing;

namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The shape series interface.
    /// </summary>
    public interface IStrokeSeries : ISeries
    {
        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        Color Fill { get; set; }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        Color Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        float StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        IEnumerable<double> StrokeDashArray { get; set; }
    }
}