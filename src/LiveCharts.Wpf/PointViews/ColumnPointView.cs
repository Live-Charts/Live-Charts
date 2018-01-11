using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Core.Views;

namespace LiveCharts.Wpf.PointViews
{
    public class ColumnPointView<TModel, TChartPoint>
        : ChartPointView<TModel, TChartPoint, Point2D, ColumnViewModel>
        where TChartPoint : ChartPoint<TModel, Point2D, ColumnViewModel>, new()
    {
        public Rectangle Rectangle { get; set; }

        public override void Draw(TChartPoint point, ColumnViewModel model, TChartPoint previous, IChartView chart)
        {
            throw new System.NotImplementedException();
        }

        public override void Erase(IChartView chart)
        {
            throw new System.NotImplementedException();
        }
    }
}
