using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.ViewModels;
using LiveCharts.Core.Views;
using LiveCharts.Wpf.PointViews;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default WPF UI builder.
    /// </summary>
    public class UiProvider : IUiProvider
    {
        /// <inheritdoc cref="IUiProvider.MeasureString"/>
        Size IUiProvider.MeasureString(string text, string fontFamily, double fontSize, FontStyles fontStyle)
        {
            throw new System.NotImplementedException();
        }

        public ChartPointView<TModel, ChartPoint<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel> BuildColumnPointView<TModel>()
        {
            return new ColumnPointView<TModel, ChartPoint<TModel, Point2D, ColumnViewModel>>();
        }
    }
}
