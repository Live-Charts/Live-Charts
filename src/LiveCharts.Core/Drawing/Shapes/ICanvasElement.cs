using LiveCharts.Drawing.Brushes;
using LiveCharts.Interaction.Controls;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines an element in the user interface.
    /// </summary>
    public interface ICanvasElement : IAnimatable
    {
        /// <summary>
        /// Draws the element in the user interface.
        /// </summary>
        void Paint();

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