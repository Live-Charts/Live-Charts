using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Defaults;

namespace LiveCharts.Core.DefaultSettings
{
    /// <summary>
    /// Primitive types configuration.
    /// </summary>
    public static class PlotTypesConfig
    {
        /// <summary>
        /// Configures LiveCharts to plot C# primitive types.
        /// </summary>
        /// <param name="settings">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsSettings PlotPrimitiveTypes(this LiveChartsSettings settings)
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

            return settings;
        }

        /// <summary>
        /// Configures LiveCharts to plot the already the default objects at LiveCharts.Core.Defaults namespace.
        /// </summary>
        /// <param name="charting">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsSettings PlotDefaultTypes(this LiveChartsSettings charting)
        {
            charting.Has2DPlotFor<decimal>((value, index) => new Point2D(index, (double) value));
            charting.Has2DPlotFor<ObservableModel>((om, index) => new Point2D(index, om.Value));
            charting.Has2DPlotFor<ObservablePointModel>((opm, index) => new Point2D(opm.X, opm.Y));

            charting.PlotFinancial<FinancialModel>(
                (fm, index) => new FinancialPoint(index, fm.Open, fm.High, fm.Low, fm.Close));

            charting.PlotPolar<PolarModel>((pm, index) => new PolarPoint(pm.Radius, pm.Angle));

            charting.PlotWeighted2D<Weighted2DPoint>((point, index) => new Weighted2DPoint(point.X, point.Y, point.Weight));

            return charting;
        }
    }
}