using Brush = LiveCharts.Core.Drawing.Brush;

namespace LiveCharts.Core.Interaction.Styles
{
    /// <summary>
    /// Defines a data label style.
    /// </summary>
    public class DataLabelStyle
    {
        /// <summary>
        /// Gets or sets the fore color.
        /// </summary>
        /// <value>
        /// The color of the fore.
        /// </value>
        public Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font { get; set; }
    }
}