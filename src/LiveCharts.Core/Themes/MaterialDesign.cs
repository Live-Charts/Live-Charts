using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Data;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing.Svg;
using Font = LiveCharts.Core.Abstractions.Font;

namespace LiveCharts.Core.Themes
{
    /// <summary>
    /// Predefined themes.
    /// </summary>
    public static class MaterialDesign
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
                })
                .SetDefault<ILineSeries>(lineSeries =>
                {
                    lineSeries.Geometry = Geometry.Circle;
                    lineSeries.GeometrySize = 15d;
                    lineSeries.StrokeThickness = 3.5d;
                    lineSeries.DefaultFillOpacity = .25d;
                    lineSeries.LineSmoothness = .6d;
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
                    axis.YSeparatorStyle = new SeparatorStyle(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 245, 245, 245), 0);
                    axis.XAlternativeSeparatorStyle = SeparatorStyle.Empty;
                    axis.YAlternativeSeparatorStyle = SeparatorStyle.Empty;
                });

            return settings;
        }
    }
}
