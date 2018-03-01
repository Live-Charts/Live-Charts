using LiveCharts.Core.Coordinates;

namespace LiveCharts.Core.Defaults
{
    /// <summary>
    /// Primitive types configuration.
    /// </summary>
    public static class PlotTypesConfig
    {
        /// <summary>
        /// Configures LiveCharts to plot the default types.
        /// </summary>
        /// <param name="settings">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsSettings PlotDefaults(this LiveChartsSettings settings)
        {
            settings.Has2DPlotFor<short>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<ushort>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<int>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<long>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<ulong>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<double>((value, index) => new Point2D(index, (float) value));
            settings.Has2DPlotFor<float>((value, index) => new Point2D(index, value));

            settings.HasWeighed2DPlotFor<short>((value, index) => new Weighted2DPoint(index, value, 0));
            settings.HasWeighed2DPlotFor<ushort>((value, index) => new Weighted2DPoint(index, value, 0));
            settings.HasWeighed2DPlotFor<int>((value, index) => new Weighted2DPoint(index, value, 0));
            settings.HasWeighed2DPlotFor<long>((value, index) => new Weighted2DPoint(index, value, 0));
            settings.HasWeighed2DPlotFor<ulong>((value, index) => new Weighted2DPoint(index, value, 0));
            settings.HasWeighed2DPlotFor<double>((value, index) => new Weighted2DPoint(index, (float) value, 0));
            settings.HasWeighed2DPlotFor<float>((value, index) => new Weighted2DPoint(index, value, 0));

            settings.Has2DPlotFor<decimal>((value, index) => new Point2D(index, (float) value));
            settings.HasWeighed2DPlotFor<decimal>((value, index) => new Weighted2DPoint(index, (float) value, 0));

            settings.Has2DPlotFor<ObservableModel>((om, index) => new Point2D(index, om.Value));
            settings.Has2DPlotFor<ObservablePointModel>((opm, index) => new Point2D(opm.X, opm.Y));

            settings.PlotFinancial<FinancialModel>(
                (fm, index) => new FinancialPoint(index, fm.Open, fm.High, fm.Low, fm.Close));

            settings.PlotPolar<PolarModel>((pm, index) => new PolarPoint(pm.Radius, pm.Angle));

            settings.HasWeighed2DPlotFor<Weighted2DPoint>((point, index) => new Weighted2DPoint(point.X, point.Y, point.Weight));

            return settings;
        }
    }
}