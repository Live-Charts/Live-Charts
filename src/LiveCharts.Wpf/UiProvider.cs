using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.Controls;
using LiveCharts.Wpf.Views;
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
        public IPointView<TModel, Point<TModel, Point, ColumnViewModel>, Point, ColumnViewModel> 
            GetNerBarPointView<TModel>()
        {
            return new BarColumnPointView<TModel, Point<TModel, Point, ColumnViewModel>, Rectangle, DataLabel>();
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, Point, BezierViewModel>, Point, BezierViewModel>
            GetNewBezierView<TModel>()
        {
            return new BezierPointView<TModel, Point<TModel, Point, BezierViewModel>, DataLabel>();
        }

        /// <inheritdoc />
        public IPointView<TModel, Point<TModel, WeightedPoint, ScatterViewModel>, WeightedPoint, ScatterViewModel>
            GetNewScatterView<TModel>()
        {
            return new ScatterPointView<TModel, Point<TModel, WeightedPoint, ScatterViewModel>, DataLabel>();
        }
    }
}
