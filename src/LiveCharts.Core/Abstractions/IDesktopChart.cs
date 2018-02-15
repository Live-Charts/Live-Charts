using System.Windows.Input;
using LiveCharts.Core.Events;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Describes a desktop chart.
    /// </summary>
    public interface IDesktopChart
    {
        /// <summary>
        /// Occurs when a user places the mouse over a point.
        /// </summary>
        event DataInteractionHandler DataMouseEnter;

        /// <summary>
        /// Gets or sets the data mouse enter command.
        /// </summary>
        /// <value>
        /// The data mouse enter command.
        /// </value>
        ICommand DataMouseEnterCommand { get; set; }

        /// <summary>
        /// Occurs when the user moves the mouse away from a point.
        /// </summary>
        event DataInteractionHandler DataMouseLeave;

        /// <summary>
        /// Gets or sets the data mouse leave.
        /// </summary>
        /// <value>
        /// The data mouse leave.
        /// </value>
        ICommand DataMouseLeaveCommand { get; set; }
    }
}