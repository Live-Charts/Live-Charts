using System.Drawing;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a shape with a specified solid color.
    /// </summary>
    public interface IHeatShape : IShape
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        Color Color { get; set; }
    }
}