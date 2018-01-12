using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.ViewModels;

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

        public IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel> ColumnViewProvider<TModel>()
        {
            throw new NotImplementedException();
        }
    }
}
