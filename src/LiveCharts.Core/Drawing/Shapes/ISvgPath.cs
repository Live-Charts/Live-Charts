namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a SVG-based path in the user interface.
    /// </summary>
    public interface ISvgPath : IShape
    {
        /// <summary>
        /// The SVG vector.
        /// </summary>
        string Svg { get; set; }
    }
}
