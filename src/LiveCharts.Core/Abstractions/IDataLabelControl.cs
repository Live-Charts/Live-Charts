using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a control that can be used as a data label.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public interface IDataLabelControl<TModel, TCoordinate, TViewModel>
        where TCoordinate : ICoordinate
    {
        Size Measure(Point<TModel, TCoordinate, TViewModel> point);
    }
}