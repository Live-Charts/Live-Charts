using LiveCharts.Drawing.Brushes;

namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines an element in the user interface.
    /// </summary>
    public interface IPaintable : IAnimatable, IUIContent
    {
        /// <summary>
        /// Paints the shape with the given stroke and fill.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        void Paint(IBrush? stroke, IBrush? fill);
    }
}