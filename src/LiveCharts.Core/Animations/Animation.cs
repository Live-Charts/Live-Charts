using System;
using System.Collections.Generic;

namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// Just an animation.
    /// </summary>
    public class TimeLine
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the time line.
        /// </summary>
        /// <value>
        /// The time line.
        /// </value>
        public IEnumerable<Frame> AnimationLine { get; set; }
    }
}