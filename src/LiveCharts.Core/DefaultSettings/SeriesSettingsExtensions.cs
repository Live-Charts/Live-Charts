using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Styles;

namespace LiveCharts.Core.DefaultSettings
{
    /// <summary>
    /// Series settings Extensions.
    /// </summary>
    public static class SeriesSettingsExtensions
    {
        /// <summary>
        /// Uses the default series settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static LiveChartsSettings UseDefaultSeriesSettings(this LiveChartsSettings settings)
        {
            settings.SetStyle(LiveChartsSelectors.Column, style =>
                {
                    style.Geometry = Geometry.Square;
                    style.DefaultFillOpacity = .8;
                })
                .SetStyle(LiveChartsSelectors.Line, style =>
                {
                    style.Geometry = Geometry.HorizontalLine;
                    style.DefaultFillOpacity = .35;
                });
            return settings;
        }
    }
}