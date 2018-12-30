namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// Defines a virtual completed function.
    /// </summary>
    public class CompletedFunction : IEasingFunction
    {
        /// <inheritdoc></inheritdoc>
        public double GetProgress(double time) => 1;
    }
}
