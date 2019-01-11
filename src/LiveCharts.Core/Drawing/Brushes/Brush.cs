using LiveCharts.Animations;
using LiveCharts.Drawing.Shapes;

namespace LiveCharts.Drawing.Brushes
{
    /// <summary>
    /// Defines an object thyat defines how a shape is painted in the UI.
    /// </summary>
    public abstract class Brush
    {
        /// <summary>
        /// Animates the fill/stroke of a shape.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public abstract IAnimationBuilder Animate(IShape shape, AnimatableArguments args);
    }
}
