using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Wpf.PointViews
{
    /// <summary>
    /// Column series point view.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TPoint">The type of the chart point.</typeparam>
    /// <seealso cref="IPointView{TModel,TPoint,TCoordinate,TViewModel}" />
    public class ColumnPointView<TModel, TPoint>
        : IPointView<TModel, TPoint, Point2D, ColumnViewModel>
        where TPoint : Point<TModel, Point2D, ColumnViewModel>, new()
    {
        /// <inheritdoc />
        public object VisualElement { get; protected set; }

        public void Draw(TPoint point, TPoint previous, IChartView chart, ColumnViewModel model)
        {
            var isNew = VisualElement == null;

            if (isNew)
            {
                VisualElement = new Rectangle();
            }
        }

        public virtual void Erase(IChartView chart)
        {
            throw new System.NotImplementedException();
        }
    }
}
