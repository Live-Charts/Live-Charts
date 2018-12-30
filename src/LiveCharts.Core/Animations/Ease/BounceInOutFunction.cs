namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// An bounce function based on https://github.com/d3/d3-ease/blob/master/src/bounce.js
    /// </summary>
    public class BounceInOutFunction : IEasingFunction
    {
        public double GetProgress(double t)
        {
            return ((t *= 2) <= 1 ? 1 - BounceOutFunction.Calculate(1 - t) : BounceOutFunction.Calculate(t - 1) + 1) / 2;
        }
    }
}
