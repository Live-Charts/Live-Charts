
using LiveCharts.Drawing.Styles;
using System.Drawing;
using FontStyle = LiveCharts.Drawing.Styles.FontStyle;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a label in the user interface.
    /// </summary>
    public interface ILabel : IPaintable, IPlaceable
    {
        /// <summary>
        /// Gets or set the label rotation.
        /// </summary>
        double Rotation { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        Padding Padding { get; set; }

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
        double FontSize { get; set; }

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
