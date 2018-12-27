namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// A size with double precision.
    /// </summary>
    public struct SizeD
    {
        /// <summary>
        /// Inisializes a new instance of the SizeD struct.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SizeD(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height { get; set; }

        /// <inheritdoc></inheritdoc>
        public override bool Equals(object obj)
        {
            if (!(obj is SizeD))
            {
                return false;
            }

            var d = (SizeD)obj;
            return Width == d.Width &&
                   Height == d.Height;
        }

        /// <inheritdoc></inheritdoc>
        public override int GetHashCode()
        {
            var hashCode = 859600377;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}