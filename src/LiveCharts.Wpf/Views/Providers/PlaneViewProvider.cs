using System;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Dimensions;
using LiveCharts.Wpf.Controls;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IPlaneViewProvider" />
    public class PlaneViewProvider : IPlaneViewProvider
    {
        /// <summary>
        /// Gets the new visual.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IPlaneSeparatorView GetNewVisual()
        {
            return new PlaneView<AxisLabel>();
        }

        /// <summary>
        /// Gets the new label.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IMeasurableLabel GetMeasurableLabel()
        {
            return new AxisLabel();
        }
    }
}
