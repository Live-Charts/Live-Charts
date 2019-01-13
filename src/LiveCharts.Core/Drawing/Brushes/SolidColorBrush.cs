using System.Drawing;
using LiveCharts.Animations;

namespace LiveCharts.Drawing.Brushes
{
    /// <summary>
    /// Defines an object that is able to paint and animate a shape with a solid color.
    /// </summary>
    public class SolidColorBrush : Brush
    {
        /// <summary>
        /// Initializes a new intance of the <see cref="SolidColorBrush"/> class.
        /// </summary>
        public SolidColorBrush()
        {

        }

        /// <summary>
        /// Initializes a new intance of the <see cref="SolidColorBrush"/> class with a given color.
        /// </summary>
        public SolidColorBrush(Color color)
        {
            Color = color;
        }

        /// <summary>
        /// Initializes a new intance of the <see cref="SolidColorBrush"/> class with the given color components.
        /// </summary>
        public SolidColorBrush(byte alpha, byte red, byte green, byte blue)
        {
            Color = Color.FromArgb(alpha, red, green, blue);
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <inheritdoc></inheritdoc>
        public override IAnimationBuilder Animate(AnimatableArguments args)
        {
            if (Target == null) throw new LiveChartsException(148);
            return UIFactory.GetSolidColorBrushAnimation(Target, args);
        }
    }
}
