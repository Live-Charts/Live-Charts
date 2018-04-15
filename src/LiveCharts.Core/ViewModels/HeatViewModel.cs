using System.Drawing;

namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// The heat view model.
    /// </summary>
    public struct HeatViewModel
    {
        /// <summary>
        /// Gets or sets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public RectangleF Rectangle { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public Color From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public Color To { get; set; }
    }
}