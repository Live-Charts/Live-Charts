using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default WPF UI builder.
    /// </summary>
    public class WpfLiveChartsProvider : IUiProvider
    {
        public Size MeasureString(string text, string fontFamily, double fontSize, FontStyles fontStyle)
        {
            throw new System.NotImplementedException();
        }

        public IPointView DefaultColumnViewGetter()
        {
            throw new System.NotImplementedException();
        }
    }
}
