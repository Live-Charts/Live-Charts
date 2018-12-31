using LiveCharts.Animations.Ease;
using System;

namespace LiveCharts.Animations
{
    /// <summary>
    /// The animatable arguments class.
    /// </summary>
    public class AnimatableArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatableArguments" /> class.
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="easingFunction"></param>
        public AnimatableArguments(TimeSpan duration, IEasingFunction easingFunction)
        {
            Duration = duration;
            EasingFunction = easingFunction;
        }

        /// <summary>
        /// Gets the duration
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Gets the easing function.
        /// </summary>
        public IEasingFunction EasingFunction { get; private set; }

        /// <summary>
        /// Sets a delay to the easing function.
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="delay"></param>
        public void SetDelay(DelayRules rule, double delay)
        {
            EasingFunction = EasingFunction.Delay(rule, delay);
        }

        /// <summary>
        /// Builds the animation argmunets based on the parent and child objects.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static AnimatableArguments BuildFrom(ICoreParentAnimatable parent, ICoreChildAnimatable child)
        {
            return new AnimatableArguments(
                            child.AnimationsSpeed ?? parent.AnimationsSpeed,
                            child.EasingFunction ?? parent.EasingFunction);
        }
    }
}