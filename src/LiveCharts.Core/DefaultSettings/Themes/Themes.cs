using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Data;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
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

            var t = typeof(ColumnSeries<>);

            settings.PlotDefaults()
                .UseMaterialDesignColors()

                // sets a base for all the series
                .SetDefault<ISeries>(series =>
                {
                    series.IsVisible = true;
                    series.Font = baseFont;
                    series.DataLabels = false;
                    series.DefaultFillOpacity = 1;
                    series.Fill = Color.Empty; // if the color is empty, the DataFactory will assign it.
                    series.Stroke = Color.Empty;
                    series.StrokeThickness = 0;
                    series.Geometry = Geometry.Circle;
                    series.Title = "Unnamed Series";
                    series.DataLabelsPosition = new DataLabelsPosition
                    {
                        HorizontalAlignment = HorizontalAlingment.Centered,
                        VerticalAlignment = VerticalLabelPosition.Top,
                        Rotation = 0
                    };
                })
                .SetDefault<IColumnSeries>(columnSeries =>
                {
                    columnSeries.Geometry = Geometry.Square;
                    columnSeries.MaxColumnWidth = 20d;
                    columnSeries.PointCornerRadius = 8d;
                })
                .SetDefault<ILineSeries>(lineSeries =>
                {
                    lineSeries.Geometry = Geometry.HorizontalLine;
                    lineSeries.StrokeThickness = 3.5;
                    lineSeries.DefaultFillOpacity = .25;
                })

                // sets a base for all the planes
                .SetDefault<Plane>(plane =>
                {
                    plane.Font = baseFont;
                    plane.LabelFormatter = Builders.AsMetricNumber;
                })
                .SetDefault<Axis>(axis =>
                {
                    axis.XSeparatorStyle = SeparatorStyle.Empty;
                    axis.YSeparatorStyle = new SeparatorStyle(new Color(0, 0, 0, 0), new Color(255, 245, 245, 245), 0);
                    axis.XAlternativeSeparatorStyle = SeparatorStyle.Empty;
                    axis.YAlternativeSeparatorStyle = SeparatorStyle.Empty;
                });

            return settings;
        }
    }
}
