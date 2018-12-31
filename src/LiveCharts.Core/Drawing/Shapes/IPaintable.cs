using LiveCharts.Drawing.Brushes;
using LiveCharts.Interaction.Controls;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines an element in the user interface.
    /// </summary>
    public interface IPaintable : IAnimatable
    {
        /// <summary>
        /// Paints the shape with the given stroke and fill.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        void Paint(IBrush? stroke, IBrush? fill);

        /// <summary>
        /// Flushes all the visuals to the canvas.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="clippedToDrawMargin"></param>
        void FlushToCanvas(ICanvas canvas, bool clippedToDrawMargin);

        /// <summary>
        /// Removes the element from the canvas.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="isclipped"></param>
        void RemoveFromCanvas(ICanvas canvas);
    }
}