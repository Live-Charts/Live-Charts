namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// The interaction area class.
    /// </summary>
    public interface IInteractionArea
    {
        /// <summary>
        /// Determines whether this area contains the given n dimensions point.
        /// </summary>
        /// <param name="dimensions">The dimensions.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified selection model]; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(params double[] dimensions);
    }
}
