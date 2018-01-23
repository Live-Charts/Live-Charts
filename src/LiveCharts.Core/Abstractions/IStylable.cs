using System.ComponentModel;
using LiveCharts.Core.Styles;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines an element that has a visual style.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IStylable : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the selector.
        /// </summary>
        /// <value>
        /// The selector.
        /// </value>
        string Selector { get; }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        Style Style { get; set; }
    }
}