using LiveCharts.Core.Drawing;
using Brush = LiveCharts.Core.Drawing.Brush;

namespace LiveCharts.Core.Interaction.Styles
{
    /// <summary>
    /// Defines a data label style.
    /// </summary>
    public class LabelStyle
    {
        /// <summary>
        /// Gets or sets the fore ground color of the label.
        /// </summary>
        /// <value>
        /// The color of the fore.
        /// </value>
        public Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font of the label.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        /// <value>
        /// The padding.
        /// </value>
        public Margin Padding { get; set; }

        /// <summary>
        /// Gets or sets the labels rotation.
        /// </summary>
        /// <value>
        /// The labels rotation.
        /// </value>
        public double LabelsRotation { get; set; }

        /// <summary>
        /// Gets the actual labels rotation.
        /// </summary>
        /// <value>
        /// The actual labels rotation.
        /// </value>
        public double ActualLabelsRotation
        {
            get
            {
                // we only allow angles from -90° to 90°
                // see appendix/labels.1.png
                var alpha = LabelsRotation % 360;
                if (alpha < -90) alpha += 360;
                if (alpha > 90) alpha += 180;
                return alpha;
            }
        }
    }
}