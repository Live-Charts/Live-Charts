namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// A delayed easing function.
    /// </summary>
    public class DelayedFunction : IEasingFunction
    {
        private readonly IEasingFunction _baseEasingFunction;
        private readonly double _delay;
        private readonly double _remaining;
        private readonly double _initial;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseEasingFunction"></param>
        /// <param name="delay"></param>
        public DelayedFunction(IEasingFunction baseEasingFunction, double delay)
        {
            _baseEasingFunction = baseEasingFunction;
            _delay = delay;
            _remaining = 1 - _delay;
            _initial = baseEasingFunction.GetProgress(0);
        }

        /// <inheritdoc></inheritdoc>
        public double GetProgress(double time)
        {
            return time <= _delay 
                ? _initial 
                : _baseEasingFunction.GetProgress((time - _delay) / _remaining);
        }
    }
}
