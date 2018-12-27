using System.Drawing;

namespace LiveCharts.Core.Drawing.Shapes
{
    /// <summary>
    /// Defines a shape with a specified solid color.
    /// </summary>
    public interface IColoredShape : IShape
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        Color Color { get; set; }
    }
}