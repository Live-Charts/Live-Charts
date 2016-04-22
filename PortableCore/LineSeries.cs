using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartsCore
{
    public class LineSeries : Series
    {
        public LineSeries(ISeriesView view) : base(view)
        {
        }

        public LineSeries(ISeriesView view, SeriesConfiguration configuration) : base(view, configuration)
        {
        }

        public override void Update()
        {
            var unitaryOffset = Chart.HasUnitaryPoints
                ? (Chart.Invert
                    ? new LvcPoint(0, ChartFunctions.GetUnitWidth(AxisTags.Y, Chart, ScalesYAt)*.5)
                    : new LvcPoint(ChartFunctions.GetUnitWidth(AxisTags.X, Chart, ScalesXAt)*.5, 0))
                : new LvcPoint();

            ChartPoint previous = null;

            foreach (var chartPoint in Values.Points)
            {
                chartPoint.Coordinates = ChartFunctions.ToDrawMargin(chartPoint, ScalesXAt, ScalesYAt, Chart) +
                                         unitaryOffset;

                if (chartPoint.View == null)
                    chartPoint.View = View.InitializePointView();

                chartPoint.View.UpdateView(previous, chartPoint, Chart);
                
                previous = chartPoint;
            }
        }
    }
}
