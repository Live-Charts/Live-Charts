using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Core.Interaction.Series;

namespace LiveCharts.Wpf.Views.Providers
{
    /// <summary>
    /// The bezier view provider class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="ISeriesViewProvider{TModel,TCoordinate,TViewModel,TSeries}" />
    public class FromPivotBezierSeriesViewProvider<TModel>
        : IBezierSeriesViewProvider<TModel, PointCoordinate, BezierViewModel, ILineSeries>
    {
        /// <inheritdoc />
        public void OnUpdateStarted(IChartView chart, ILineSeries series, TimeLine timeLine)
        {
        }

        /// <inheritdoc />
        public IPointView<TModel, PointCoordinate, BezierViewModel, ILineSeries> GetNewPoint()
        {
            return new BezierPointView<TModel, TextBlock>();
        }

        /// <inheritdoc />
        public void OnPointHighlight(IChartPoint point, TimeLine timeLine)
        {
            var view = (BezierPointView<TModel, TextBlock>)point.View;
            view.Shape.RenderTransformOrigin = new Point(0.5, 0.5);
            view.Shape.RenderTransform = new ScaleTransform(1.2, 1.2);
        }

        /// <inheritdoc />
        public void RemovePointHighlight(IChartPoint point, TimeLine timeLine)
        {
            var view = (BezierPointView<TModel, TextBlock>)point.View;
            view.Shape.RenderTransformOrigin = new Point();
            view.Shape.RenderTransform = null;
        }

        /// <inheritdoc />
        public void OnUpdateFinished(IChartView chart, ILineSeries series, TimeLine timeLine)
        {
        }

        /// <inheritdoc />
        public ICartesianPath GetNewPath()
        {
            return new SelfDrawnCartesianPath();
        }
    }
}