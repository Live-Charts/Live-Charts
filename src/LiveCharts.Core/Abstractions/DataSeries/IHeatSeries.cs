using System.Collections.Generic;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// The heat series interface.
    /// </summary>
    public interface IHeatSeries: ICartesianSeries
    {
        /// <summary>
        /// Gets or sets the gradient stop collection.
        /// </summary>
        /// <value>
        /// The gradient stop collection.
        /// </value>
        IEnumerable<GradientStop> Gradient { get; set; }
    }
}