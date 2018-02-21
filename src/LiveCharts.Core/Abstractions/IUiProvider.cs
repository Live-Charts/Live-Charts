using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a drawing provider.
    /// </summary>
    public interface IUiProvider
    {
        /// <summary>
        /// The axis label provider.
        /// </summary>
        /// <returns></returns>
        IPlaneLabelControl GetNewAxisLabel();

        /// <summary>
        /// The label provider.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns></returns>
        IDataLabelControl GetNewDataLabel<TModel, TCoordinate, TViewModel>()
            where TCoordinate : ICoordinate;

        /// <summary>
        /// The axis separator provider.
        /// </summary>
        /// <returns></returns>
        ICartesianAxisSeparator GetNewAxisSeparator();

        /// <summary>
        /// The path provider.
        /// </summary>
        /// <returns></returns>
        ICartesianPath GetNewPath();

        /// <summary>
        /// Provides LiveCharts with a builder that returns a column view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel> 
            GetNewColumnView<TModel>();

        /// <summary>
        /// Provides LiveCharts with a builder that returns a bezier view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IPointView<TModel, Point<TModel, Point2D, BezierViewModel>, Point2D, BezierViewModel>
            GetNewBezierView<TModel>();

        /// <summary>
        /// gs the et new scatter view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IPointView<TModel, Point<TModel, Weighted2DPoint, ScatterViewModel>, Weighted2DPoint, ScatterViewModel>
            GetNewScatterView <TModel>();
    }
}
