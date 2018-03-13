using System.Drawing;

namespace LiveCharts.Core.Interaction
{
    /// <summary>
    /// The hover rectangle class.
    /// </summary>
    /// <seealso cref="InteractionArea" />
    public class RectangleInteractionArea : InteractionArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleInteractionArea"/> class.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public RectangleInteractionArea(RectangleF rectangle)
        {
            Rectangle = rectangle;
        }

        /// <summary>
        /// Gets or sets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public RectangleF Rectangle { get; set; }

        /// <inheritdoc />
        public override bool Contains(params double[] dimensions)
        {
            var x = dimensions[0];
            var y = dimensions[1];

            return x >= Rectangle.Left && x <= Rectangle.Left +Rectangle.Width &&
                   y >= Rectangle.Top && y <= Rectangle.Top + Rectangle.Height;
        }
    }
}