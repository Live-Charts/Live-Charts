using System.Windows.Input;

namespace LiveCharts.Wpf.Events
{
    /// <summary>
    /// A chart that defines wpf specific behavior.
    /// </summary>
    public interface IWpfChart
    {
        /// <summary>
        /// Occurs when a user places the mouse over a point.
        /// </summary>
        event MouseDataInteractionHandler DataMouseEntered;

        /// <summary>
        /// Gets or sets the data mouse enter command, this command will try to be executed 
        /// when  the user places the pointer over a point.
        /// </summary>
        /// <value>
        /// The data mouse enter command.
        /// </value>
        ICommand DataMouseEnteredCommand { get; set; }

        /// <summary>
        /// Occurs when the user moves the mouse away from a data point.
        /// </summary>
        event MouseDataInteractionHandler DataMouseLeft;

        /// <summary>
        /// Gets or sets the data mouse leave command, this command will try to be executed
        /// when the user leaves moves the pointer away from a data point.
        /// </summary>
        /// <value>
        /// The data mouse leave.
        /// </value>
        ICommand DataMouseLeftCommand { get; set; }

        /// <summary>
        /// Occurs when the user moves the pointer down in a point.
        /// </summary>
        event MouseButtonDataInteractionHandler DataMouseDown;

        /// <summary>
        /// Gets or sets the data pointer down command.
        /// </summary>
        /// <value>
        /// The data pointer down command.
        /// </value>
        ICommand DataMouseDownCommand { get; set; }
    }
}