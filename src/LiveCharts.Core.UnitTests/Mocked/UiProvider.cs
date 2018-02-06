using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;

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

        public IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel> ColumnViewProvider<TModel>()
        {
            return new ColumnView<TModel>();
        }
    }
}
