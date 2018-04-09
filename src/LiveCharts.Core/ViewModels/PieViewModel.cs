using System.Drawing;

namespace LiveCharts.Core.ViewModels
{
    /// <summary>
    /// The Pie view model.
    /// </summary>
    public struct PieViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PieViewModel"/> struct.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="chartCenter">Center.</param>
        public PieViewModel(SliceViewModel from, SliceViewModel to, PointF chartCenter)
        {
            From = from;
            To = to;
            ChartCenter = chartCenter;
        }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public SliceViewModel From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public SliceViewModel To { get; set; }

        /// <summary>
        /// Gets or sets the chart center.
        /// </summary>
        /// <value>
        /// The chart center.
        /// </value>
        public PointF ChartCenter { get; set; }
    }
}