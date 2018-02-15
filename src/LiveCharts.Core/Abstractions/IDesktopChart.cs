using LiveCharts.Core.Events;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Describes a desktop chart.
    /// </summary>
    public interface IDesktopChart
    {
        /// <summary>
        /// Occurs when the user click a point.
        /// </summary>
        event DataInteractionHandler DataClick;

        /// <summary>
        /// Occurs when the user double clicks a point.
        /// </summary>
        event DataInteractionHandler DataDoubleClick;

        /// <summary>
        /// Occurs when a user places the mouse over a point.
        /// </summary>
        event DataInteractionHandler DataMouseEnter;

        /// <summary>
        /// Occurs when the user moves the mouse away from a point.
        /// </summary>
        event DataInteractionHandler DataMouseLeave;
    }
}