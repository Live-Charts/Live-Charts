using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using LiveCharts.Core.Abstractions;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// a default label for a point.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.TextBlock" />
    public class DefaultPointLabel : TextBlock, INotifyPropertyChanged
    {
        private ICoordinate _coordinate;

        public DefaultPointLabel()
        {
            BindingOperations.SetBinding(
                this,
                TextProperty,
                new Binding {Source = Coordinate.AsTooltipData()});
        }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>
        /// <value>
        /// The coordinate.
        /// </value>
        public ICoordinate Coordinate
        {
            get => _coordinate;
            set
            {
                _coordinate = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
