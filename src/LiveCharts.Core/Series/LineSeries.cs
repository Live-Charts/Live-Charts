using System.Collections.Generic;
using System.Linq;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Series
{
    /// <summary>
    /// The line series class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="CartesianSeries{TModel}" />
    public class LineSeries<TModel> : CartesianSeries<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries{TModel}"/> class.
        /// </summary>
        public LineSeries()
            : base(LiveCharts.Constants.LineSeries)
        {
        }

        /// <summary>
        /// Gets or sets the line smoothness, goes from 0 to 1, 0: straight lines, 1: max curved line.
        /// </summary>
        /// <value>
        /// The line smoothness.
        /// </value>
        public double LineSmoothness { get; set; }

        /// <inheritdoc />
        protected override void OnUpdateView(ChartModel chart)
        {
            var points = FetchData(chart).Cast<CartesianChartPoint>();
            var smoothness = LineSmoothness > 1 ? 1 : (LineSmoothness < 0 ? 0 : LineSmoothness);


        }
    }
}