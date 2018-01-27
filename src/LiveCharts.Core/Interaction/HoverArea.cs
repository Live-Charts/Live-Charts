using LiveCharts.Core.Abstractions;

namespace LiveCharts.Core.Interaction
{
    /// <summary>
    /// The hover area class.
    /// </summary>
    public abstract class HoverArea
    {
        /// <summary>
        /// Determines whether this area contains the given n dimensions point.
        /// </summary>
        /// <param name="selectionModel">The selection model.</param>
        /// <param name="dimensions">The dimensions.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified selection model]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool Contains(TooltipSelectionMode selectionModel, params double[] dimensions);
    }
}
