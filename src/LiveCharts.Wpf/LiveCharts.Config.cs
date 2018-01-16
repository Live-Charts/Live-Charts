using LiveCharts.Core;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Wpf
{
    public static class Config
    {
        public static LiveChartsSettings UseWpf(this LiveChartsSettings defaults)
        {
            defaults.DataFactory = new DefaultDataFactory();
            defaults.UiProvider = new UiProvider();

            defaults.Series(
                SeriesConstants.Line,
                seriesDefault =>
                {
                    seriesDefault.FillOpacity = .35;
                    seriesDefault.Geometry = Geometry.HorizontalLine;
                });

            defaults.Series(
                SeriesConstants.Column,
                seriesDefault =>
                {
                    seriesDefault.FillOpacity = .8;
                    seriesDefault.Geometry = Geometry.Square;
                });

            defaults.PlotPrimitiveTypes()
                .PlotDefaultTypes()
                .UseMaterialDesignColors();

            return defaults;
        }
    }
}