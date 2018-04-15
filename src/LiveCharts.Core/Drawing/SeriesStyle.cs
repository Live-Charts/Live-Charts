using System.Collections.Generic;
using System.Drawing;

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// The series style struct.
    /// </summary>
    public struct SeriesStyle
    {
        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Color Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public float StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        public IEnumerable<float> StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Color Fill { get; set; }
    }
}