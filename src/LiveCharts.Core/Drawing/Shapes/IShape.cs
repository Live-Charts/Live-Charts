using LiveCharts.Core.Animations;
using LiveCharts.Core.Drawing.Brushes;
using System.Collections.Generic;

namespace LiveCharts.Core.Drawing.Shapes
{
    public interface IShape
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
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        float Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        float Height { get; set; }

        /// <summary>
        /// Gets or sets the fill brush.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        IBrush Fill { get; set; }

        /// <summary>
        /// Gets or sets the stroke brush.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        IBrush Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        float StrokeThickness { get; set; }

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
        float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        IEnumerable<double> StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash offset.
        /// </summary>
        /// <value>
        /// The stroke dash offset.
        /// </value>
        double StrokeDashOffset { get; set; }

        /// <summary>
        /// Returns an animation builder for the given time line.
        /// </summary>
        /// <param name="timeline">The timeline.</param>
        /// <returns></returns>
        IAnimationBuilder Animate(TimeLine timeline);

        /// <summary>
        /// Paints the shape with the given stroke and fill.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        void Paint(IBrush stroke, IBrush fill);
    }
}