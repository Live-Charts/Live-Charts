using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// A chart series with a cartesian plane coordinates system (x, y).
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="Series{TModel}" />
    public abstract class CartesianSeries<TModel> : Series<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianSeries{TModel}"/> class.
        /// </summary>
        /// <param name="chartPointType">Type of the chart point.</param>
        /// <param name="defaultFillOpacity">Default fill opacity</param>
        /// <param name="skipCriteria">The skip criteria.</param>
        protected CartesianSeries(
            ChartPointTypes chartPointType, double defaultFillOpacity, SeriesSkipCriteria skipCriteria)
            : base(chartPointType, defaultFillOpacity, skipCriteria)
        {
            // A cartesian chart has 2 axis, X, Y
            // A cartesian chart can have as many axis as the user needs
            //      this means, There are always only 2 dimensions, X and Y
            //      but the user can define, for example multiple X axis
            //      this with the intention to allow the charts to compare
            //      trends, every axis has its own scale.
            //      see: https://lvcharts.net/App/examples/v1/wf/Multiple%20Axes
            // The ScaleAt array, for a cartesian series has 2 dimensions:
            //               {x, y}
            ScalesAt = new[] {0, 0};
            // This means that by default, any cartesian series is scaled at
            // the first element in the axis array for both, X and Y dimensions.
            // A user can change where the series is scaled using the properties
            // ScalesXAt and ScalesYAt, see properties below.
            // NOTE: notice we could get an OutOfRangeException if the index of this property
            // goes out of range with the CartesianChart.XAxis/YAxis array.
        }

        /// <summary>
        /// Gets or sets the index of the axis where the X coordinate is scaled at.
        /// </summary>
        /// <value>
        /// The scales x at.
        /// </value>
        public int ScalesXAt
        {
            get => ScalesAt[0];
            set => ScalesAt[0] = value;
        }

        /// <summary>
        /// Gets or sets the index of the axis where the Y coordinate is scaled at.
        /// </summary>
        /// <value>
        /// The scales y at.
        /// </value>
        public int ScalesYAt
        {
            get => ScalesAt[1];
            set => ScalesAt[1] = value;
        }
    }
}