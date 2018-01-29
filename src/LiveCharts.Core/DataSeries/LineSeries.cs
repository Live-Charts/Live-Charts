using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TPoint">the type of the chart point.</typeparam>
    public abstract class LineSeries<TModel, TPoint, TView>
        : CartesianSeries<TModel, Point2D, BezierViewModel, TPoint>, ILineSeries where TPoint : Point<TModel, Point2D, BezierViewModel>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TModel, TPoint, TView}"/> class.
        /// </summary>
        protected LineSeries()
        {
            LiveChartsSettings.Build<ILineSeries>(this);
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
        public override Point DefaultPointWidth => Point.Empty;

        /// <inheritdoc />
        public override void UpdateView(ChartModel chart)
        {

        }
    }
}
