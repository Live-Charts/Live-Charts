using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.ViewModels;
using Point = LiveCharts.Core.Coordinates.Point;

namespace LiveCharts.Core.UnitTests.Mocked
{
    public class UiProvider : IUiProvider
    {
        public IPlaneLabelControl AxisLabelProvider()
        {
            return new PlaneLabel();
        }

        public IDataLabelControl DataLabelProvider<TModel, TCoordinate, TViewModel>() where TCoordinate : ICoordinate
        {
            return new DataLabel();
        }

        public ICartesianAxisSeparator CartesianAxisSeparatorProvider()
        {
            return new AxisSeparator();
        }

        public IPointView<TModel, Point<TModel, Point, Rectangle>, Point, Rectangle> ColumnViewProvider<TModel>()
        {
            return new ColumnView<TModel>();
        }

        public IPointView<TModel, Point<TModel, Point, BezierViewModel>, Point, BezierViewModel> BezierViewProvider<TModel>()
        {
            return new BezierProvider<TModel>();
        }
    }
}
