namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// Defines a property easing function.
    /// </summary>
    public interface IEasingFunction
    {
        /// <summary>
        /// Gets the progress in porcentage at a given time in percentage.
        /// </summary>
        /// <param name="time">the time.</param>
        /// <returns>the progress.</returns>
        double GetProgress(double time);
    }
}