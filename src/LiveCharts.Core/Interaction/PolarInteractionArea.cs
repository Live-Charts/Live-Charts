using System;
using System.Drawing;

namespace LiveCharts.Core.Interaction
{
    /// <summary>
    /// The polar interaction area class.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Interaction.InteractionArea" />
    public class PolarInteractionArea : InteractionArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolarInteractionArea"/> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="angleFrom">The angle from.</param>
        /// <param name="angleTo">The angle to.</param>
        /// <param name="chartCenter">The chart center.</param>
        public PolarInteractionArea(float radius, float angleFrom, float angleTo, PointF chartCenter)
        {
            Radius = radius;
            AngleFrom = angleFrom;
            AngleTo = angleTo;
            Center = chartCenter;
        }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public float Radius { get; }

        /// <summary>
        /// Gets the angle start point in radians.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public float AngleFrom { get; }

        /// <summary>
        /// Gets the angle end point in radians.
        /// </summary>
        /// <value>
        /// The angle to.
        /// </value>
        public float AngleTo { get; }

        /// <summary>
        /// Gets the center of the chart.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public PointF Center { get; }

        /// <inheritdoc />
        public override bool Contains(params double[] dimensions)
        {
            var x = dimensions[0];
            var y = dimensions[1];

            var dx = Math.Abs(x - Center.X);
            var dy = Math.Abs(y - Center.Y);

            var radius = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            var angle = Math.Atan(dy / dx);

            return AngleFrom <= angle && AngleTo >= angle && radius < Radius;
        }
    }
}