using System.Windows;
using System.Windows.Media.Animation;

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// LiveCharts easing functions for wpf.
    /// </summary>
    public class LiveChartsEasingFunction : EasingFunctionBase
    {
        private readonly LiveCharts.Animations.Ease.IEasingFunction _easingFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsEasingFunction"/> class.
        /// </summary>
        /// <param name="liveChartsEasingFunction"></param>
        public LiveChartsEasingFunction(LiveCharts.Animations.Ease.IEasingFunction liveChartsEasingFunction)
        {
            _easingFunction = liveChartsEasingFunction;
        }

        /// <inheritdoc></inheritdoc>
        protected override Freezable CreateInstanceCore()
        {
            return new LiveChartsEasingFunction(_easingFunction);
        }

        /// <inheritdoc></inheritdoc>
        protected override double EaseInCore(double normalizedTime)
        {
            return _easingFunction.GetProgress(normalizedTime);
        }
    }
}
