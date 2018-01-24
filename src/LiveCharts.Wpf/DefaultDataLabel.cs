using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf
{
    public class DefaultDataLabel<TModel, TCoordinate, TViewModel> 
        : TextBlock, IDataLabelControl<TModel, TCoordinate, TViewModel>
        where TCoordinate : ICoordinate
    {
        Size IDataLabelControl<TModel, TCoordinate, TViewModel>.Measure(Point<TModel, TCoordinate, TViewModel> point)
        {
            DataContext = point;
            return Dispatcher.Invoke(() =>
            {
                Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                return new Size(DesiredSize.Width, DesiredSize.Height);
            });
        }
    }
}