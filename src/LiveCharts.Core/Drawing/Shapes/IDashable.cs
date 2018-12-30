using System.Collections.Generic;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines an UI object that its stroke could use a thickness, dash array and/or offset..
    /// </summary>
    public interface IDashable
    {
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        IEnumerable<double>? StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash offset.
        /// </summary>
        /// <value>
        /// The stroke dash offset.
        /// </value>
        double StrokeDashOffset { get; set; }
    }
}