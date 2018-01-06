using LiveCharts.Core.Config;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Builders;
using LiveCharts.Wpf.PointViews;

namespace LiveCharts.Wpf
{
    public static class Config
    {
        public static LiveChartsConfig UseWpf(this LiveChartsConfig config)
        {
            config.PointFactory = new DefaultDataFactory();
            config.UiProvider = new WpfLiveChartsProvider();

            config.HasSeriesMetadata(Core.LiveCharts.Constants.ColumnSeries)
                .WithFillOpacity(.8)
                .WithSkipCriteria(SeriesSkipCriteria.None)
                .WithPointViewProvider(() => new ColumnPointView());

            config.AddPrimitivesPlotTypes()
                .AddDefaultPlotObjects()
                .UseMaterialDesignColors();

            return config;
        }
    }
}