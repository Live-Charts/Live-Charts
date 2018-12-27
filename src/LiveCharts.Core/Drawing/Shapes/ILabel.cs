using LiveCharts.Core.Drawing.Styles;
using System.Drawing;
using FontStyle = LiveCharts.Core.Drawing.Styles.FontStyle;

namespace LiveCharts.Core.Drawing.Shapes
{
    /// <summary>
    /// Defines a label in the user interface.
    /// </summary>
    public interface ILabel : IUiElement
    {
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
        /// Measures this instance.
        /// </summary>
        /// <returns></returns>
        SizeF Measure();
    }
}
