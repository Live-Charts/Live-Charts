namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// An bounce function based on https://github.com/d3/d3-ease/blob/master/src/bounce.js
    /// </summary>
    public class BounceInFunction : IEasingFunction
    {
        /// <inheritdoc></inheritdoc>
        public double GetProgress(double t)
        {
            return 1 - BounceOutFunction.Calculate(t);
        }
    }
}
