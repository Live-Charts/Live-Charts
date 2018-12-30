namespace LiveCharts.Drawing.Shapes
{
    /// <summary>
    /// Defines a slice shape in the user interface.
    /// </summary>
    public interface ISlice : IShape
    {
        /// <summary>
        /// Gets or sets the inner radius.
        /// </summary>
        double InnerRadius { get; set; }
        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        double Radius { get; set; }
        /// <summary>
        /// Gets or sets thye force angle property.
        /// </summary>
        bool ForceAngle { get; set; } 
        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        double CornerRadius { get; set; }
        /// <summary>
        /// Gets or sets the pushout.
        /// </summary>
        double PushOut { get; set; }
        /// <summary>
        /// Gets or sets the wedge.
        /// </summary>
        double Wedge { get; set; }
    }
}
