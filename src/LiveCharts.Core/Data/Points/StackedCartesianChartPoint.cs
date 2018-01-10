using LiveCharts.Core.Coordinates;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref=".ChartPoint{TModel, StackedCoordinate, TViewModel}" />
    public class StackedCartesianChartPoint<TModel, TViewModel> : ChartPoint<TModel, StackedPoint, TViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackedCartesianChartPoint{TModel, TViewModel}"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="participation">The participation.</param>
        /// <param name="value">The value.</param>
        /// <param name="createdAt">The created at.</param>
        public StackedCartesianChartPoint(int index, double participation, double value, object createdAt)
            : base(createdAt)
        {
            Coordinate = new StackedPoint(participation, value, index);
        }

        /// <inheritdoc cref="ChartPoint{TModel, StackedCoordinate, TViewmodel}.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            throw new System.NotImplementedException();
        }
    }
}