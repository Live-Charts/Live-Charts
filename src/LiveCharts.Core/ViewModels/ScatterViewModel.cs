using System.Drawing;

namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// A scatter view model.
    /// </summary>
    public struct ScatterViewModel
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public PointF Location { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public double Diameter { get; set; }
    }
}