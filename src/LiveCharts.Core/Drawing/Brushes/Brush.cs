using LiveCharts.Animations;
using LiveCharts.Drawing.Shapes;

namespace LiveCharts.Drawing.Brushes
{
    /// <summary>
    /// Defines an object thyat defines how a shape is painted in the UI.
    /// </summary>
    public abstract class Brush : IAnimatable
    {
        internal object? Target { get; set; }

        /// <inheritdoc></inheritdoc>
        public abstract IAnimationBuilder Animate(AnimatableArguments args);
    }
}
