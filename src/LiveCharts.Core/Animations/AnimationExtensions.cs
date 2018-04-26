using System.Collections.Generic;

namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// A set of animations extensions.
    /// </summary>
    public static class AnimationExtensions
    {
        /// <summary>
        /// Delays the specified time in percentage.
        /// </summary>
        /// <param name="animation">The animation to delay.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        public static IEnumerable<Frame> Delay(this IEnumerable<Frame> animation, double delay)
        {
            if (delay > 0)
            {
                yield return new Frame(0d, 0d);
                yield return new Frame(delay, 0d);
            }

            var remaining = 1 - delay;

            foreach (var curve in animation)
            {
                yield return new Frame(delay + remaining * curve.Time, curve.Value);
            }
        }
    }
}
