namespace LiveCharts.Core.Defaults
{
    /// <summary>
    /// Defines an observable weighted object, this object notifies the chart to update when any property change.
    /// </summary>
    /// <seealso cref="ObservablePoint" />
    public class WeightedPoint : ObservablePoint
    {
        private double _weight;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedPoint"/> class.
        /// </summary>
        public WeightedPoint()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedPoint"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="weight">The weight.</param>
        public WeightedPoint(double x, double y, double weight)
            : base(x, y)
        {
            Weight = weight;
        }

        /// <summary>
        /// Gets or sets the weight value.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }
    }
}