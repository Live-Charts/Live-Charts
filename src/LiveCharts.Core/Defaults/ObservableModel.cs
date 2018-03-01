using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LiveCharts.Core.Defaults
{
    /// <summary>
    /// Defines an observable object, this object notifies the chart to update when the value property changes.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class ObservableModel : INotifyPropertyChanged
    {
        private float _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableModel"/> class.
        /// </summary>
        public ObservableModel()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableModel"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ObservableModel(float value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when Value property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when Value property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
