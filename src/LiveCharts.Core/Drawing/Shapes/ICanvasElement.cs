using LiveCharts.Interaction.Controls;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines an element in the user interface.
    /// </summary>
    public interface ICanvasElement : IAnimatable
    {
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
        void RemoveFromCanvas(ICanvas canvas);
    }
}