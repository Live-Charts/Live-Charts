using LiveCharts.Core;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Wpf
{
    public static class Config
    {
        public static ChartingConfig UseWpf(this ChartingConfig config)
        {
            config.DataFactory = new DefaultDataFactory();
            config.UiProvider = new UiProvider();

            config.HasDefaults(SeriesConstants.Line, defaults =>
                {
                    defaults.FillOpacity = .35;
                    defaults.Geometry = Geometry.HorizontalLine;
                });

            config.HasDefaults(SeriesConstants.Column, defaults =>
            {
                defaults.FillOpacity = .8;
                defaults.Geometry = Geometry.Square;
            });

            config.PlotPrimitiveTypes()
                .PlotDefaultTypes()
                .UseMaterialDesignColors();

            return config;
        }
    }
}