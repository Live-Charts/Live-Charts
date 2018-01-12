using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.Abstractions
{
    /// <summary>
    /// Defines a drawing provider.
    /// </summary>
    public interface IUiProvider
    {
        /// <summary>
        /// Measures the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontStyle">The font style.</param>
        /// <returns></returns>
        Size MeasureString(string text, string fontFamily, double fontSize, FontStyles fontStyle);

        /// <summary>
        /// Gets the column view.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel> 
            ColumnViewProvider<TModel>();
    }
}
