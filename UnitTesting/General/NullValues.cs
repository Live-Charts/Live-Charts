using LiveCharts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting.General
{
    public partial class GeneralTest
    {
        [TestMethod, TestCategory("General")]
        public void NullValues()
        {
            var barChart = new BarChart
            {
                Series = new SeriesCollection
                {
                    new BarSeries {Values = null},
                    new LineSeries {Values = null}
                }
            };
            barChart.UnsafeRedraw();

            var lineChart = new LineChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries {Values = null}
                }
            };
            lineChart.UnsafeRedraw();

            var pieChart = new PieChart
            {
                Series = new SeriesCollection
                {
                    new PieSeries {Values = null}
                }
            };
            pieChart.UnsafeRedraw();

            var stackedChart = new StackedBarChart
            {
                Series = new SeriesCollection
                {
                    new StackedBarSeries {Values = null}
                }
            };
            stackedChart.UnsafeRedraw();
        }
    }
}
