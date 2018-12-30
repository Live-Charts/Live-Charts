namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// A lineal easing function.
    /// </summary>
    public class LinealFunction : IEasingFunction
    {
        /// <inheritdoc></inheritdoc>
        public double GetProgress(double time) => time;
    }
}
