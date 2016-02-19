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
            barChart.UnsafeUpdate();

            var lineChart = new LineChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries {Values = null}
                }
            };
            lineChart.UnsafeUpdate();

            var pieChart = new PieChart
            {
                Series = new SeriesCollection
                {
                    new PieSeries {Values = null}
                }
            };
            pieChart.UnsafeUpdate();

            var stackedChart = new StackedBarChart
            {
                Series = new SeriesCollection
                {
                    new StackedBarSeries {Values = null}
                }
            };
            stackedChart.UnsafeUpdate();
        }
    }
}
