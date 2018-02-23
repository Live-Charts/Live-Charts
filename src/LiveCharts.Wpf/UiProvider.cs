using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;
using LiveCharts.Wpf.PointViews;
using LiveCharts.Wpf.Separators;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default WPF UI builder.
    /// </summary>
    public class UiProvider : IUiProvider
    {
        /// <inheritdoc />
        public IChartContent GetChartContent()
        {
            return new ChartContent();
        }

        /// <inheritdoc />
        public IPlaneLabelControl GetNewAxisLabel()
        {
            return new AxisLabel();
        }

        /// <inheritdoc />
        public IDataLabelControl GetNewDataLabel<TModel, TCoordinate, TViewModel>()
            where TCoordinate : ICoordinate
        {
            return new DataLabel();
        }

        /// <inheritdoc />
        public ICartesianAxisSeparator GetNewAxisSeparator()
        {
            return new CartesianAxisSeparatorView<AxisLabel>();
        }

        /// <inheritdoc />
        public ICartesianPath GetNewPath()
        {
            return new CartesianPath();
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel>
            GetNewColumnView<TModel>()
        {
            return new ColumnPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Rectangle, DataLabel>();
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, Point2D, BezierViewModel>, Point2D, BezierViewModel>
            GetNewBezierView<TModel>()
        {
            return new BezierPointView<TModel, Point<TModel, Point2D, BezierViewModel>, DataLabel>();
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, Weighted2DPoint, ScatterViewModel>, Weighted2DPoint, ScatterViewModel>
            GetNewScatterView<TModel>()
        {
            return new ScatterPointView<TModel, Point<TModel, Weighted2DPoint, ScatterViewModel>, DataLabel>();
        }
    }
}
