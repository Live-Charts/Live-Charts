using LiveCharts.Animations.Ease;
using System;

namespace LiveCharts.Animations
{
    /// <summary>
    /// A set of animations extensions.
    /// </summary>
    public static class AnimationExtensions
    {
        private static readonly CubicBezierFunction _cubicDelayer = new CubicBezierFunction(0.25f, 0.1f, 0.25f, 1.0f);
        private static readonly ElasticInOutFunction _elasticDelay = new ElasticInOutFunction();
        private static readonly BounceInOutFunction _bounceDelayer = new BounceInOutFunction();
        private static readonly Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easingFunction"></param>
        /// <param name="rule"></param>
        /// <param name="delay">delay in percentage</param>
        /// <returns></returns>
        public static IEasingFunction Delay(this IEasingFunction easingFunction, DelayRules rule, double delay)
        {
            switch (rule)
            {
                case DelayRules.None:
                    return easingFunction;
                case DelayRules.LeftToRight:
                    return new DelayedFunction(easingFunction, delay);
                case DelayRules.RightToLeft:
                    return new DelayedFunction(easingFunction, 1 - delay);
                case DelayRules.Random:
                    return new DelayedFunction(easingFunction, _random.NextDouble());
                case DelayRules.LeftToRightCubic:
                    return new DelayedFunction(easingFunction, _cubicDelayer.GetProgress(delay));
                case DelayRules.RightToLeftCubic:
                    return new DelayedFunction(easingFunction, _cubicDelayer.GetProgress(1 - delay));
                case DelayRules.LeftToRightElastic:
                    return new DelayedFunction(easingFunction, _elasticDelay.GetProgress(delay));
                case DelayRules.RightToLeftelastic:
                    return new DelayedFunction(easingFunction, _elasticDelay.GetProgress(1 - delay));
                case DelayRules.LeftToRightBounce:
                    return new DelayedFunction(easingFunction, _bounceDelayer.GetProgress(delay));
                case DelayRules.RightToLeftBounce:
                    return new DelayedFunction(easingFunction, _bounceDelayer.GetProgress(1 - delay));
                default:
                    throw new ArgumentOutOfRangeException(nameof(rule), rule, null);
            }
        }
    }
}
