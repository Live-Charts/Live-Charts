using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LiveCharts.Core.Defaults
{
    /// <summary>
    /// Defines an observable point object, this object notifies the chart to update when any property change.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class ObservablePointModel : INotifyPropertyChanged
    {
        private float _x;
        private float _y;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservablePointModel"/> class.
        /// </summary>
        public ObservablePointModel()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservablePointModel"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public ObservablePointModel(float x, float y)
        {
            
        }

        /// <summary>
        /// Gets or sets the x value.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public float X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the y value.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}