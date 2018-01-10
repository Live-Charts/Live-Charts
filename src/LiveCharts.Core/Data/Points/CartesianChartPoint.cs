using System;
using LiveCharts.Core.Abstractions.PointViews;
using LiveCharts.Core.Coordinates;

namespace LiveCharts.Core.Data.Points
{
    public class ColumnChartPoint<TModel> : CartesianChartPoint<TModel, ColumnViewModel>
    {
    }

    public class LineChartPoint<TModel> : CartesianChartPoint<TModel, BezierModel>
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref="ChartPoint{TModel, PointCoordinate, TViewModel}" />
    public class CartesianChartPoint<TModel, TViewModel> : ChartPoint<TModel, Point2D, TViewModel>
    {
        /// <inheritdoc cref="ChartPoint{U, U, V}.CompareDimensions"/>
        public override bool CompareDimensions(DimensionRange[] ranges, SeriesSkipCriteria skipCriteria)
        {
            var xDimensionRange = ranges[0];
            var yDimensionRange = ranges[1];

            switch (skipCriteria)
            {
                case SeriesSkipCriteria.IgnoreXOverflow:
                    if (Coordinate.X < xDimensionRange.From || Coordinate.X > xDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreYOverflow:
                    if (Coordinate.Y < yDimensionRange.From || Coordinate.Y > yDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.IgnoreOverflow:
                    if (Coordinate.X < xDimensionRange.From || Coordinate.X > xDimensionRange.To ||
                        Coordinate.Y < yDimensionRange.From || Coordinate.Y > yDimensionRange.To) return true;
                    break;
                case SeriesSkipCriteria.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(skipCriteria), skipCriteria, null);
            }

            if (Coordinate.X > xDimensionRange.Max) xDimensionRange.Max = Coordinate.X;
            if (Coordinate.X < xDimensionRange.Min) xDimensionRange.Min = Coordinate.X;
            if (Coordinate.Y > yDimensionRange.Max) yDimensionRange.Max = Coordinate.Y;
            if (Coordinate.Y < yDimensionRange.Min) yDimensionRange.Min = Coordinate.Y;
            return false;
        }
    }
}