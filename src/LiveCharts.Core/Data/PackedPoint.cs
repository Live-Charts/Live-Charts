using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// A boxed copy of the <see cref="Point{TModel,TCoordinate,TViewModel}"/> class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    public class PackedPoint<TModel, TCoordinate>
    {
        /// <summary>
        /// Gets the key of the point, a key is used internally as a unique identifier in 
        /// in a <see cref="Series"/> 
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public int Key { get; internal set; }

        /// <summary>
        /// Gets the instance represented by this point.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public TModel Model { get; internal set; }

        /// <summary>
        /// Gets the view model,the model to drawn in the user interface.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public object ViewModel { get; internal set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public object View { get; internal set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public TCoordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the series that owns the point.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public Series.Series Series { get; internal set; }

        /// <summary>
        /// Gets the chart that owns the point.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public ChartModel Chart { get; internal set; }
    }
}