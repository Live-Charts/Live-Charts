using System.ComponentModel;
using System.Drawing;
using LiveCharts.Core.Drawing.Svg;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Core.Abstractions.DataSeries
{
    /// <summary>
    /// Data Series
    /// </summary>
    public interface ISeries
    {
        /// <summary>
        /// Gets or sets a value indicating whether [data labels].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data labels]; otherwise, <c>false</c>.
        /// </value>
        bool DataLabels { get; set; }

        /// <summary>
        /// Gets or sets the data labels position.
        /// </summary>
        /// <value>
        /// The data labels position.
        /// </value>
        DataLabelsPosition DataLabelsPosition { get; set; }

        /// <summary>
        /// Gets or sets the default fill opacity.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        double DefaultFillOpacity { get; set; }

        /// <summary>
        /// Gets the default width of the point.
        /// </summary>
        /// <value>
        /// The default width of the point.
        /// </value>
        double[] DefaultPointWidth { get; }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        Color Fill { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        Geometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets the scales at.
        /// </summary>
        /// <value>
        /// The scales at.
        /// </value>
        int[] ScalesAt { get; }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        Color Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the index of the z.
        /// </summary>
        /// <value>
        /// The index of the z.
        /// </value>
        int ZIndex { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}