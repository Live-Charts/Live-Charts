using LiveCharts.Drawing.Brushes;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a shape in the user interface.
    /// </summary>
    public interface IShape : ICanvasElement, IDashable, IPlaceable
    {
        /// <summary>
        /// Gets or sets the Fill.
        /// </summary>
        Brush? Fill { get; set; }

        /// <summary>
        /// Gets or sets the Stroke.
        /// </summary>
        Brush? Stroke { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        double Height { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        double Rotation { get; set; }

        /// <summary>
        /// Gets or sets the index of the z.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        int ZIndex { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        double Opacity { get; set; }
    }
}