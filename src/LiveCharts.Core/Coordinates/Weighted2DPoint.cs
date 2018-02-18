using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;

namespace LiveCharts.Core.Coordinates
{
    /// <summary>
    /// A weighted coordinate.
    /// </summary>
    public struct Weighted2DPoint : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2D"/> struct.
        /// </summary>
        public Weighted2DPoint(double x, double y, double weight)
        {
            X = x;
            Y = y;
            Weight = weight;
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { get; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { get; }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <value>
        /// The w.
        /// </value>
        public double Weight { get; }

        /// <inheritdoc />
        public void CompareDimensions(DataRange[] dimensions)
        {
            var x = dimensions[0];
            var y = dimensions[1];
            var w = dimensions[2];

            if (X > x.MaxValue) x.MaxValue = X;
            if (X < x.MinValue) x.MinValue = X;
            if (Y > y.MaxValue) y.MaxValue = Y;
            if (Y < y.MinValue) y.MinValue = Y;
            if (Weight > w.MaxValue) w.MaxValue = Weight;
            if (Weight < w.MinValue) w.MinValue = Weight;
        }

        /// <inheritdoc />
        public string[] AsTooltipData(params Plane[] dimensions)
        {
            return new[]
            {
                dimensions[0].FormatValue(X),
                dimensions[1].FormatValue(Y),
                dimensions[2].FormatValue(Weight)
            };
        }
    }
}