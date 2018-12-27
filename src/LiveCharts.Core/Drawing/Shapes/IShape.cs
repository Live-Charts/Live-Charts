using System.Collections.Generic;
using System.Drawing;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a shape in the user interface.
    /// </summary>
    public interface IShape : IUIElement
    {
        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        double Left { get; set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        double Top { get; set; }

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
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        double StrokeThickness { get; set; }

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

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        IEnumerable<double>? StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash offset.
        /// </summary>
        /// <value>
        /// The stroke dash offset.
        /// </value>
        double StrokeDashOffset { get; set; }
    }
}