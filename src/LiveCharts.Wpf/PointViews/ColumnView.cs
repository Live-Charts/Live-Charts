using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.PointViews;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Wpf.PointViews
{
    public class ColumnView<TModel>
        : ChartPointView<TModel, CartesianChartPoint<TModel, ColumnViewModel>, Point2D, ColumnViewModel>
    {
        public override void Draw(
            CartesianChartPoint<TModel, ColumnViewModel> point,
            ColumnViewModel viewModel,
            CartesianChartPoint<TModel, ColumnViewModel> previous,
            IChartView chart)
        {
            
            throw new System.NotImplementedException();
        }

        public override void Erase(IChartView chart)
        {
            throw new System.NotImplementedException();
        }
    }
}
