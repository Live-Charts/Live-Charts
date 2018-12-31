using LiveCharts.Animations;
using LiveCharts.Drawing.Shapes;
using System.Drawing;

namespace LiveCharts.Drawing.Brushes
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IBrush" />
    public interface ISolidColorBrush : IBrush
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Color Color { get; set; }
    }
}
