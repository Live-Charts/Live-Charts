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
        /// <param name="timeline">The timeline.</param>
        /// <returns></returns>
        IAnimationBuilder Animate(Transition timeline);
    }
}