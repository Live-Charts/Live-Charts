namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// Represents a rectangle sketch.
    /// </summary>
    public struct RectangleD
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleD"/> struct.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RectangleD(double x, double y, double width, double height)
        {
            Top = y;
            Left = x;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleD"/> struct.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        public RectangleD(PointD location, SizeD size)
        {
            Top = location.Y;
            Left = location.X;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Gets or sets the top coordinate.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the left coordinate.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets an empty rectange.
        /// </summary>
        public static RectangleD Empty => new RectangleD(0,0,0,0);

        /// <inheritdoc></inheritdoc>
        public override bool Equals(object obj)
        {
            if (!(obj is RectangleD))
            {
                return false;
            }

            var d = (RectangleD) obj;
            return Top == d.Top &&
                   Left == d.Left &&
                   Width == d.Width &&
                   Height == d.Height;
        }

        /// <inheritdoc></inheritdoc>
        public override int GetHashCode()
        {
            var hashCode = -2056332411;
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}