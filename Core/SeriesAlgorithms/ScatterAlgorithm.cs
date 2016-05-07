//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

namespace LiveCharts.SeriesAlgorithms
{
    public class ScatterAlgorithm : SeriesAlgorithm, ICartesianSeries
    {
        public ScatterAlgorithm(ISeriesView view) : base(view)
        {
            XAxisMode = AxisLimitsMode.OneSeparator;
            YAxisMode = AxisLimitsMode.OneSeparator;
            DefaultConfiguration = DefaultConfigurations.Bubble;
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

                chartPoint.View = View.GetPointView(chartPoint.View,
                    View.DataLabels ? fx(chartPoint.X) + ", " + fy(chartPoint.Y) : null);

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart, View);
            }
        }
    }
}
