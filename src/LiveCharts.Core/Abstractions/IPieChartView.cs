namespace LiveCharts.Core.Abstractions
{
    public interface IPieChartView : IChartView
    {
        /// <summary>
        /// Gets or sets the starting rotation angle in degrees.
        /// </summary>
        /// <value>
        /// The starting rotation angle.
        /// </value>
        double StartingRotationAngle { get; set; }

        /// <summary>
        /// Gets or sets the inner radius.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        double InnerRadius { get; set; }
    }
}