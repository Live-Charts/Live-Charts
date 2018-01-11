using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.ViewModels;
using LiveCharts.Core.Views;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TChartPoint">the type of the chart point.</typeparam>
    public abstract class LineSeries<TModel, TChartPoint, TView>
        : CartesianSeries<TModel, Point2D, BezierViewModel, TChartPoint>
        where TChartPoint : ChartPoint<TModel, Point2D, BezierViewModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TModel, TChartPoint, TView}"/> class.
        /// </summary>
        protected LineSeries()
            : base(SeriesConstants.Line)
        {
            PointGeometry = ChartingConfig.GetDefault(SeriesConstants.Line).Geometry;
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
