using LiveCharts.Core.Animations;
using System.Drawing;

namespace LiveCharts.Core.Drawing.Brushes
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Drawing.Brushes.IBrush" />
    public interface ISolidColorBrush : IBrush
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Color Color { get; set; }

        /// <summary>
        /// Animates according to the specified time line.
        /// </summary>
        /// <param name="timeline">The time line.</param>
        /// <returns></returns>
        IAnimationBuilder Animate(TimeLine timeline);
    }
}
