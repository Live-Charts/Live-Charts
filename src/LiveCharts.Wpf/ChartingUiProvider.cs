using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default WPF UI builder.
    /// </summary>
    public class ChartingUiProvider : IUiProvider
    {
        /// <inheritdoc cref="IUiProvider.MeasureString"/>
        Size IUiProvider.MeasureString(string text, string fontFamily, double fontSize, FontStyles fontStyle)
        {
            throw new System.NotImplementedException();
        }
    }
}
