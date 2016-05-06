using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveCharts.SeriesAlgorithms
{
    public class ScatterAndBubbleAlgorthm : SeriesCore, ICartesianSeries
    {
        public ScatterAndBubbleAlgorthm(ISeriesView view) : base(view)
        {
            XAxisMode = AxisLimitsMode.HalfSparator;
            YAxisMode = AxisLimitsMode.HalfSparator;
        }

        public AxisLimitsMode XAxisMode { get; set; }
        public AxisLimitsMode YAxisMode { get; set; }

        public override void Update()
        {
            var fx = CurentXAxis.GetFormatter();
            var fy = CurrentYAxis.GetFormatter();

            foreach (var chartPoint in View.Values.Points)
            {
                chartPoint.ChartLocation = ChartFunctions.ToDrawMargin(chartPoint, View.ScalesXAt, View.ScalesYAt, Chart);

                chartPoint.View = View.GetView(chartPoint.View,
                    View.DataLabels ? fx(chartPoint.X) + ", " + fy(chartPoint.Y) : null);

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);
            }
        }
    }
}
