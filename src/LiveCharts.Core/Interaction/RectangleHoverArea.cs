namespace LiveCharts.Core.Interaction
{
    /// <summary>
    /// The hover rectangle class.
    /// </summary>
    /// <seealso cref="InteractionArea" />
    public class RectangleInteractionArea : InteractionArea
    {
        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public double Height { get; set; }

        /// <inheritdoc />
        public override bool Contains(params double[] dimensions)
        {
            var x = dimensions[0];
            var y = dimensions[1];

            return x >= Left && x <= Left + Width &&
                   y >= Top && y <= Top + Height;
        }
    }
}