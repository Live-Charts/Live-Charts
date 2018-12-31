namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// An bounce function based on https://github.com/d3/d3-ease/blob/master/src/bounce.js
    /// </summary>
    public class BounceOutFunction : IEasingFunction
    {
        private static readonly double 
            b1 = 4 / 11d,
            b2 = 6 / 11d,
            b3 = 8 / 11d,
            b4 = 3 / 4d,
            b5 = 9 / 11d,
            b6 = 10 / 11d,
            b7 = 15 / 16d,
            b8 = 21 / 22d,
            b9 = 63 / 64d,
            b0 = 1 / b1 / b1;

        /// <inheritdoc></inheritdoc>
        public double GetProgress(double t)
        {
            return Calculate(t);
        }

        internal static double Calculate(double t)
        {
            return (t = +t) < b1 ? b0 * t * t : t < b3 ? b0 * (t -= b2) * t + b4 : t < b6 ? b0 * (t -= b5) * t + b7 : b0 * (t -= b8) * t + b9;
        }
    }
}
