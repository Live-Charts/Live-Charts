using System;
using LiveCharts.Core;
using LiveCharts.Core.Config;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Builders;
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Wpf.PointViews;

namespace LiveCharts.Wpf
{
    public static class Config
    {
        public static LiveChartsConfig UseWpf(this LiveChartsConfig config)
        {
            config.PointFactory = new DefaultDataFactory();
            config.UiProvider = new WpfLiveChartsProvider();

            config.HasSeriesDefault(LiveChartsConstants.LineSeries)
                .WithFillOpacity(.35d)
                .WithSkipCriteria(SeriesSkipCriteria.IgnoreXOverflow)
                .WithPointViewProvider(() => throw new NotImplementedException())
                .WithDefaultGeometry(Geometry.HorizontalLine);

            config.HasSeriesDefault(LiveChartsConstants.ColumnSeries)
                .WithFillOpacity(.8d)
                .WithSkipCriteria(SeriesSkipCriteria.None)
                .WithPointViewProvider(() => new ColumnPointView())
                .WithDefaultGeometry(Geometry.Square);

            config.AddPrimitivesPlotTypes()
                .AddDefaultPlotObjects()
                .UseMaterialDesignColors();

            return config;
        }
    }
}