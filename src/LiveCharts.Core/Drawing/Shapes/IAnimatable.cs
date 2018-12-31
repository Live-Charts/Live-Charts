using LiveCharts.Animations;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines an UI object whose properties are able to animate.
    /// </summary>
    public interface IAnimatable
    {
        /// <summary>
        /// Returns an animation builder for the given time line.
        /// </summary>
        /// <param name="args">The animation arguments.</param>
        /// <returns></returns>
        IAnimationBuilder Animate(AnimatableArguments args);
    }
}