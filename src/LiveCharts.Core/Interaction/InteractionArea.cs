namespace LiveCharts.Core.Interaction
{
    /// <summary>
    /// The interaction area class.
    /// </summary>
    public abstract class InteractionArea
    {
        /// <summary>
        /// Determines whether this area contains the given n dimensions point.
        /// </summary>
        /// <param name="dimensions">The dimensions.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified selection model]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool Contains(params double[] dimensions);
    }
}
