using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TChartPoint">The type of the chart point.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref="Series{TModel, TChartPoint, TCoordinate, TViewModel}" />
    public abstract class CartesianSeries<TModel, TCoordinate, TViewModel, TChartPoint> 
        : Series<TModel, TCoordinate, TViewModel, TChartPoint>
        where TChartPoint : ChartPoint<TModel, TCoordinate, TViewModel>, new ()
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianSeries{T,U,V,W}"/> class.
        /// </summary>
        /// <param name="key">the series type.</param>
        protected CartesianSeries(string key)
            : base(key)
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