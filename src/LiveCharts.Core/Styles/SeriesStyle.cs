using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Styles
{
    /// <summary>
    /// Represents the visual style of a series.
    /// </summary>
    public class SeriesStyle : Style
    {
        /// <summary>
        /// Gets or sets the default fill opacity.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        public double DefaultFillOpacity { get; set; }

        /// <summary>
        /// Gets or sets the default geometry.
        /// </summary>
        /// <value>
        /// The default geometry.
        /// </value>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets the data labels position.
        /// </summary>
        /// <value>
        /// The data labels position.
        /// </value>
        public DataLabelsPosition DataLabelsPosition { get; set; }
    }
}