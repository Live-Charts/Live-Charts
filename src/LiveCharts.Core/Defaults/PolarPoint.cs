using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LiveCharts.Core.Defaults
{
    /// <summary>
    /// Defines an observable polar point, this object notifies the chart to update when any property change.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class PolarPoint : INotifyPropertyChanged
    {
        private double _radius;
        private double _angle;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarPoint"/> class.
        /// </summary>
        public PolarPoint()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarPoint"/> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="angle">The angle.</param>
        public PolarPoint(double radius, double angle)
        {
            Radius = radius;
            Angle = angle;
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when [property changed].
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