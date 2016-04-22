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
            ChartPoint previous = null;

            foreach (var chartPoint in Values.Points)
            {
                chartPoint.Coordinates = ChartFunctions.ToDrawMargin(chartPoint, ScalesXAt, ScalesYAt, Chart);

                if (chartPoint.View == null)
                    chartPoint.View = View.InitializePointView();

                chartPoint.View.Update(previous, chartPoint, Chart);

                previous = chartPoint;
            }
        }
    }
}
