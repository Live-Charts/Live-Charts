using LiveCharts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting.General
{
    public partial class GeneralTest
    {
        [TestMethod, TestCategory("General")]
        public void NullSeries ()
        {
            var barChart = new BarChart {Series = null};
            barChart.UnsafeRedraw();

            var lineChart = new LineChart { Series = null };
            lineChart.UnsafeRedraw();

            var pieChart = new PieChart { Series = null };
            pieChart.UnsafeRedraw();

            var stackedChart = new StackedBarChart { Series = null };
            stackedChart.UnsafeRedraw();
        }
    }
}
