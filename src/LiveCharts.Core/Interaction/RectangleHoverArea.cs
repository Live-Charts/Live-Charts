using LiveCharts.Core.Abstractions;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.Interaction
{
    /// <summary>
    /// The hover rectangle class.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Interaction.HoverArea" />
    public class RectangleHoverArea : HoverArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleHoverArea"/> class.
        /// </summary>
        public RectangleHoverArea()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleHoverArea"/> class.
        /// </summary>
        /// <param name="columnViewModel">The column view model.</param>
        public RectangleHoverArea(ColumnViewModel columnViewModel)
        {
            Top = columnViewModel.Top;
            Left = columnViewModel.Left;
            Height = columnViewModel.Height;
            Width = columnViewModel.Width;
        }

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
        public override bool Contains(TooltipSelectionMode selectionMode, params double[] dimensions)
        {
            // at least 2 dimensions are required.
            // x = dimensions[0]
            // y = dimensions[1]
            var x = dimensions[0];
            var y = dimensions[1];

            switch (selectionMode)
            {
                case TooltipSelectionMode.SharedX:
                    return x >= Left && x <= Left + Width;
                case TooltipSelectionMode.SharedY:
                    return y >= Top && y <= Top + Height;
                default:
                    return x >= Left && x <= Left + Width &&
                           y >= Top && y <= Top + Height;
            }
        }
    }
}