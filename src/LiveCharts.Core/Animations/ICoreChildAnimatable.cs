using LiveCharts.Animations.Ease;
using System;

namespace LiveCharts.Animations
{
    /// <summary>
    /// Defines an element that specifies how <see cref="Drawing.Shapes.IAnimatable"/> objects will animate.
    /// </summary>
    public interface ICoreChildAnimatable
    {
        /// <summary>
        /// Gets or sets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        TimeSpan? AnimationsSpeed { get; set; }

        /// <summary>
        /// Gets or sets the animation line.
        /// </summary>
        /// <value>
        /// The animation line.
        /// </value>
        IEasingFunction? EasingFunction { get; set; }
    }
}
