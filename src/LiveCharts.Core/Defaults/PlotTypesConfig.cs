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
            // in the previous line, since we knew the cartesian coordinate type is Point2D
            // we could also use the following syntax =>
            // charting.PlotAs<short, Point2D>((value, index) => new Point2D());
            settings.Has2DPlotFor<ushort>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<int>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<long>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<ulong>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<double>((value, index) => new Point2D(index, value));
            settings.Has2DPlotFor<float>((value, index) => new Point2D(index, value));

            settings.Has2DPlotFor<decimal>((value, index) => new Point2D(index, (double)value));
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