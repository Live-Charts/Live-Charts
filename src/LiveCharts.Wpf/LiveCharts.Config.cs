using LiveCharts.Core;
using LiveCharts.Core.Data;
using LiveCharts.Core.DefaultSettings;

namespace LiveCharts.Wpf
{
    public static class Config
    {
        public static LiveChartsSettings UseWpf(this LiveChartsSettings settings)
        {
            settings.DataFactory = new DefaultDataFactory();
            settings.UiProvider = new UiProvider();

            settings
                .UseDefaultSeriesSettings()
                .PlotPrimitiveTypes()
                .PlotDefaultTypes()
                .UseMaterialDesignColors();

            return settings;
        }
    }
}