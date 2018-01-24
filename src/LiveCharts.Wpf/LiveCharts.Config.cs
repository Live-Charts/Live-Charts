using LiveCharts.Core;
using LiveCharts.Core.Data;

namespace LiveCharts.Wpf
{
    public static class Config
    {
        public static LiveChartsSettings UseWpf(this LiveChartsSettings settings)
        {
            settings.DataFactory = new DefaultDataFactory();
            settings.UiProvider = new UiProvider();

            return settings;
        }
    }
}