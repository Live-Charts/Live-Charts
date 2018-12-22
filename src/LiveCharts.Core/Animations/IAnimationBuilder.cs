using LiveCharts.Core.Drawing;
using System;
using System.Drawing;

namespace LiveCharts.Core.Animations
{
    public interface IAnimationBuilder
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// Begins the animation.
        /// </summary>
        void Begin();

        /// <summary>
        /// Applies a double animation to a given property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        IAnimationBuilder Property(string property, float from, float to, double delay = 0);

        /// <summary>
        /// Applies a point animation to a given property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        IAnimationBuilder Property(string property, PointD from, PointD to, double delay = 0);

        /// <summary>
        /// Applies color animation to a given property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        IAnimationBuilder Property(string property, Color from, Color to, double delay = 0);

        /// <summary>
        /// Takes a delegate and runs it once the animation is finished.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        IAnimationBuilder Then(EventHandler callback);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
    }
}
