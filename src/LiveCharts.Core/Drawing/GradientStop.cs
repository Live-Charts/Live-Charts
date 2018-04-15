using System;
using System.Drawing;

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a gradient stop.
    /// </summary>
    public struct GradientStop
    {
        private double _offset;

        /// <summary>
        /// Gets or sets the offset, the value goes from 0 (cold) to 1 (hot).
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public double Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                if (_offset > 1 || _offset < 0)
                {
                    throw new ArgumentOutOfRangeException($"A {nameof(GradientStop)}.{nameof(Offset)} should be greater or equals to 0 and equals or less than 1.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color { get; set; }
    }
}