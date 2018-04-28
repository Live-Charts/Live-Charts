using System;
using System.Collections.Generic;

namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// A set of animations extensions.
    /// </summary>
    public static class AnimationExtensions
    {
        private static readonly KeySpline _delaySpline = new KeySpline(0.25, 0.1, 0.25, 1.0);
        private static readonly Random _random = new Random();

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

        /// <summary>
        /// Delays the specified time in percentage.
        /// </summary>
        /// <param name="duration">The duration in ms.</param>
        /// <param name="animationLine">The animation line.</param>
        /// <param name="x">The x to interpolate.</param>
        /// <param name="rule">the delay rule.</param>
        /// <returns></returns>
        public static TimeLine Delay(double duration, IEnumerable<Frame> animationLine, double x, DelayRules rule)
        {
            switch (rule)
            {
                case DelayRules.RightToLeft:
                    x = 1 - x;
                    break;
                case DelayRules.Random:
                    x = _random.NextDouble();
                    break;
                case DelayRules.None:
                case DelayRules.LeftToRight:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rule), rule, null);
            }

            var delayTime = _delaySpline.GetY(x) * duration;
            return new TimeLine
            {
                AnimationLine = animationLine.Delay(delayTime / (duration + delayTime)),
                Duration = TimeSpan.FromMilliseconds(duration + delayTime)
            };
        }
    }
}
