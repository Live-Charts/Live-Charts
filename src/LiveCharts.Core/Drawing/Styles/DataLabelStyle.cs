using LiveCharts.Drawing.Brushes;

namespace LiveCharts.Drawing.Styles
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
        public Brush? Foreground { get; set; }

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
        public Padding Padding { get; set; }

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
                // we only use 2 quadrants...
                // see appendix/labels.1.png
                double alpha = LabelsRotation % 360;
                if (alpha < 0) alpha += 360;
                if (alpha >= 90 && alpha < 180)
                {
                    alpha += 180;
                }
                else if (alpha >= 180 && alpha < 270)
                {
                    alpha -= 180;
                }

                return alpha;
            }
        }
    }
}