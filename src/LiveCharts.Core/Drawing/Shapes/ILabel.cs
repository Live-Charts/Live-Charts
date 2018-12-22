using System.Drawing;
using LiveCharts.Core.Drawing.Brushes;
using LiveCharts.Core.Drawing.Styles;
using FontStyle = LiveCharts.Core.Drawing.Styles.FontStyle;

namespace LiveCharts.Core.Drawing.Shapes
{
    public interface IUiElement
    {

    }

    public interface ILabel
    {
        /// <summary>
        /// Gets the platform specific shape.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        object Shape { get; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        float Left { get; set; }
        
        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        float Top { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        string Content { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>
        /// The font family.
        /// </value>
        string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        float FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <value>
        /// The font style.
        /// </value>
        FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>
        /// The font weight.
        /// </value>
        FontWeight FontWeight { get; set; }

        /// <summary>
        /// Paints the label with the given brush.
        /// </summary>
        /// <param name="brush">The brush.</param>
        void Paint(IBrush brush);

        /// <summary>
        /// Measures this instance.
        /// </summary>
        /// <returns></returns>
        SizeF Measure();
    }
}
