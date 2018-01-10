using LiveCharts.Core.Abstractions.PointViews;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data.Builders;
using LiveCharts.Core.Data.Points;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <seealso cref="CartesianSeries{T,U,V,W}" />
    public abstract class LineSeries<TModel, TView>
        : CartesianSeries<TModel, CartesianChartPoint<TModel, BezierModel>, Point2D, BezierModel>
        where TView : ChartPointView<TModel, CartesianChartPoint<TModel, BezierModel>, Point2D, BezierModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TModel,TView}"/> class.
        /// </summary>
        protected LineSeries()
            : base(SeriesKeys.Line)
        {
            PointGeometry = ChartingConfig.GetDefault(SeriesKeys.Line).Geometry;
        }

        /// <summary>
        /// Gets or sets the line smoothness, goes from 0 to 1, 0: straight lines, 1: max curved line.
        /// </summary>
        /// <value>
        /// The line smoothness.
        /// </value>
        public double LineSmoothness { get; set; }

        /// <summary>
        /// Gets or sets the point geometry.
        /// </summary>
        /// <value>
        /// The point geometry.
        /// </value>
        public Geometry PointGeometry { get; set; }

        /// <inheritdoc />
        protected override void OnUpdateView(ChartModel chart)
        {

        }
    }
}
