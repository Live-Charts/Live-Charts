using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.DefaultSettings
{
    /// <summary>
    /// Series configuration class.
    /// </summary>
    public class SeriesSettings
    {
        /// <summary>
        /// Gets or sets the default fill opacity.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        public double FillOpacity { get; set; }

        /// <summary>
        /// Gets or sets the default geometry.
        /// </summary>
        /// <value>
        /// The default geometry.
        /// </value>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font { get; set; }
    }

    /// <summary>
    /// Series settings Extensions.
    /// </summary>
    public static class SeriesSettingsExtensions
    {
        public static LiveChartsSettings UseDefaultSeriesSettings(this LiveChartsSettings settings)
        {
            settings.SetSeriesSettings(Series.Series.All, seriesSettings =>
                {
                    // override default settings for all the series here...
                    seriesSettings.Font = new Font
                    {
                        FamilyName = "Arial",
                        Size = 10,
                        Style = FontStyles.Regular,
                        Weight = FontWeight.Regular
                    };
                    seriesSettings.Geometry = Geometry.Circle;
                    seriesSettings.FillOpacity = .8d;
                })
                .SetSeriesSettings(Series.Series.Column, seriesSettings =>
                {
                    seriesSettings.Geometry = Geometry.Square;
                })
                .SetSeriesSettings(Series.Series.Line, seriesSettings =>
                {
                    seriesSettings.Geometry = Geometry.HorizontalLine;
                    seriesSettings.FillOpacity = .35;
                });
            return settings;
        }
    }
}