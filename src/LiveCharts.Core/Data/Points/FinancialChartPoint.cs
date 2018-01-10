using LiveCharts.Core.Coordinates;

namespace LiveCharts.Core.Data.Points
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <seealso cref="ChartPoint{TModel, FinancialCoordinate, TView}" />
    public class FinancialChartPoint<TModel, TView> : ChartPoint<TModel, FinancialPoint, TView>
    {
        /// <inheritdoc cref="ChartPoint{TModel, FinancialCoordinate, TView}.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var indexDimensionRange = ranges[0];
            var yDimensionRange = ranges[1];

            if (Coordinate.Index > indexDimensionRange.Max) indexDimensionRange.Max = Coordinate.Index;
            if (Coordinate.Index < indexDimensionRange.Min) indexDimensionRange.Min = Coordinate.Index;
            if (Coordinate.High > yDimensionRange.Max) yDimensionRange.Max = Coordinate.High;
            if (Coordinate.Low < yDimensionRange.Min) yDimensionRange.Min = Coordinate.Low;

            return false;
        }
    }
}
