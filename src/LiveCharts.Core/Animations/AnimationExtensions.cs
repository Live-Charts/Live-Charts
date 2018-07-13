using System;
using System.Collections.Generic;

namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// A set of animations extensions.
    /// </summary>
    public static class AnimationExtensions
    {
        private static readonly KeySpline DelaySpline = new KeySpline(0.25f, 0.1f, 0.25f, 1.0f);
        private static readonly Random Random = new Random();

        /// <summary>
        /// Delays the specified time in percentage.
        /// </summary>
        /// <param name="animation">The animation to delay.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        public static IEnumerable<KeyFrame> Delay(this IEnumerable<KeyFrame> animation, float delay)
        {
            if (delay > 0)
            {
                yield return new KeyFrame(0f, 0);
                yield return new KeyFrame(delay, 0);
            }

            float remaining = 1f - delay;

            foreach (var curve in animation)
            {
                yield return new KeyFrame(delay + remaining * curve.Time, curve.Value);
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
        public static TimeLine Delay(float duration, IEnumerable<KeyFrame> animationLine, float x, DelayRules rule)
        {
            float delayTime;

            switch (rule)
            {
                case DelayRules.LeftToRight:
                    delayTime = DelaySpline.GetY(x) * duration;
                    break;
                case DelayRules.RightToLeft:
                    x = 1 - x;
                    delayTime = DelaySpline.GetY(x) * duration;
                    break;
                case DelayRules.Random:
                    x = (float) Random.NextDouble();
                    delayTime = x * duration;
                    break;
                case DelayRules.None:
                    delayTime = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rule), rule, null);
            }

            return new TimeLine
            {
                AnimationLine = animationLine.Delay(delayTime / (duration + delayTime)),
                Duration = TimeSpan.FromMilliseconds(duration + delayTime)
            };
        }
    }
}
