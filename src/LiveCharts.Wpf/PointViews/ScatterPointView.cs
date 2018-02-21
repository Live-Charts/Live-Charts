using System.Windows;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Wpf.PointViews
{
    public class ScatterPointView<TModel, TPoint, TLabel>
        : PointView<TModel, TPoint, Weighted2DPoint, ScatterViewModel, Path, TLabel>
        where TPoint : Point<TModel, Weighted2DPoint, ScatterViewModel>, new()
        where TLabel : FrameworkElement, IDataLabelControl, new()
    {

    }
}