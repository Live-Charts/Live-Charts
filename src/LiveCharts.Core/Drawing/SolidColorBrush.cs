using System.Drawing;

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// The solid color brush class, represents a solid color fill.
    /// </summary>
    public class SolidColorBrush : Brush
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolidColorBrush"/> class.
        /// </summary>
        public SolidColorBrush()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidColorBrush"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        public SolidColorBrush(Color color)
        {
            Color = color;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public double Opacity { get; set; }
    }
}