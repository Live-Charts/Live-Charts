using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.DefaultSettings.Themes
{
    /// <summary>
    /// Predefined themes.
    /// </summary>
    public static class Themes
    {
        /// <summary>
        /// Uses the material design light theme.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static LiveChartsSettings UseMaterialDesignLightTheme(this LiveChartsSettings settings)
        {
            var baseFont = new Font("Arial", 11, FontStyles.Regular, FontWeight.Regular);

            settings.PlotDefaults()
                .UseMaterialDesignColors()
                .SetStyle(LiveChartsSelectors.Default, style =>
                {
                    style.Font = baseFont;
                    style.StrokeThickness = 0;
                })
                .SetStyle(LiveChartsSelectors.DefaultSeries, style =>
                {
                    style.Font = baseFont;
                    style.Fill = Color.Empty;   // means that the DataFactory will set the default next color.
                    style.Stroke = Color.Empty; // same ^^
                    style.StrokeThickness = 0;
                    style.Geometry = Geometry.Circle;
                    style.DefaultFillOpacity = 1;
                    style.DataLabelsPosition = new DataLabelsPosition
                    {
                        HorizontalAlignment = HorizontalAlingment.Centered,
                        VerticalAlignment = VerticalLabelPosition.Top,
                        Rotation = 0
                    };
                })
                .SetStyle(LiveChartsSelectors.DefaultPlane, style =>
                {
                    style.Font = baseFont;
                    style.Fill = new Color(0, 0, 0, 0);
                    style.Stroke = new Color(255, 30, 30, 30);
                    style.StrokeThickness = 1.5;
                })
                .SetStyle(LiveChartsSelectors.Column, style =>
                {
                    style.Geometry = Geometry.Square;
                })
                .SetStyle(LiveChartsSelectors.Line, style =>
                {
                    style.Geometry = Geometry.HorizontalLine;
                    style.StrokeThickness = 3.5;
                    style.DefaultFillOpacity = .25;
                });

            return settings;
        }
    }
}
