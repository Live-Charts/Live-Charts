using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Interaction.Controls;

namespace LiveCharts.Core.Interaction.Dimensions
{
    /// <summary>
    /// The plane vie provider.
    /// </summary>
    public interface IPlaneViewProvider
    {
        /// <summary>
        /// Gets the new visual.
        /// </summary>
        /// <returns></returns>
        IPlaneSection GetNewVisual();

        /// <summary>
        /// Gets the new label.
        /// </summary>
        /// <returns></returns>
        IMeasurableLabel GetMeasurableLabel();
    }
}
