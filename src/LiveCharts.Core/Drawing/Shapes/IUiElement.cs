using LiveCharts.Core.Animations;
using LiveCharts.Core.Drawing.Brushes;

namespace LiveCharts.Core.Drawing.Shapes
{
    /// <summary>
    /// Defines an element in the user interface.
    /// </summary>
    public interface IUiElement
    {
        /// <summary>
        /// Paints the shape with the given stroke and fill.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        void Paint(IBrush? stroke, IBrush? fill);

        /// <summary>
        /// Returns an animation builder for the given time line.
        /// </summary>
        /// <param name="timeline">The timeline.</param>
        /// <returns></returns>
        IAnimationBuilder Animate(TimeLine timeline);
    }
}