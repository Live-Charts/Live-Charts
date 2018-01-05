using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// 
    /// </summary>The column series class.
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ColumnSeries<TModel> : CartesianSeries<TModel>
    {
        public ColumnSeries()
            : base(
                (ChartPointTypes) ChartPointTypes.Cartesian,
                LiveCharts.Options.DefaultColumnSeriesFillOpacity,
                (SeriesSkipCriteria) SeriesSkipCriteria.None)
        {
        }

        protected override void OnUpdateView(ChartModel chart)
        {
            base.OnUpdateView(chart);
        }
    }
}