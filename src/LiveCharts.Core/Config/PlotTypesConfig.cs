using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Defaults;

namespace LiveCharts.Core.Config
{
    /// <summary>
    /// Primitive types configuration.
    /// </summary>
    public static class PlotTypesConfig
    {
        /// <summary>
        /// Configures LiveCharts to plot C# primitive types.
        /// </summary>
        /// <param name="charting">The configuration.</param>
        /// <returns></returns>
        public static ChartingConfig PlotPrimitiveTypes(this ChartingConfig charting)
        {
            charting.Plot2D<short>((value, index) => new Point2D(index, value));
            // in the previous line, since we knew the cartesian coordinate type is Point2D
            // we could also use the following syntax =>
            // charting.PlotAs<short, Point2D>((value, index) => new Point2D());
            charting.Plot2D<ushort>((value, index) => new Point2D(index, value));
            charting.Plot2D<int>((value, index) => new Point2D(index, value));
            charting.Plot2D<long>((value, index) => new Point2D(index, value));
            charting.Plot2D<ulong>((value, index) => new Point2D(index, value));
            charting.Plot2D<double>((value, index) => new Point2D(index, value));
            charting.Plot2D<float>((value, index) => new Point2D(index, value));

            return charting;
        }

        /// <summary>
        /// Configures LiveCharts to plot the already the default objects at LiveCharts.Core.Defaults namespace.
        /// </summary>
        /// <param name="charting">The configuration.</param>
        /// <returns></returns>
        public static ChartingConfig PlotDefaultTypes(this ChartingConfig charting)
        {
            charting.Plot2D<decimal>((value, index) => new Point2D(index, (double) value));
            charting.Plot2D<ObservableModel>((om, index) => new Point2D(index, om.Value));
            charting.Plot2D<ObservablePointModel>((opm, index) => new Point2D(opm.X, opm.Y));

            charting.PlotFinancial<FinancialModel>(
                (fm, index) => new FinancialPoint(index, fm.Open, fm.High, fm.Low, fm.Close));

            charting.PlotPolar<PolarModel>((pm, index) => new PolarPoint(pm.Radius, pm.Angle));

            charting.PlotWeighted2D<Weighted2DPoint>((point, index) => new Weighted2DPoint(point.X, point.Y, point.Weight));

            return charting;
        }
    }
}