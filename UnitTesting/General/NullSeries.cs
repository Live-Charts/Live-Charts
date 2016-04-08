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
            barChart.UnsafeUpdate();

            var lineChart = new LineChart { Series = null };
            lineChart.UnsafeUpdate();

            var pieChart = new PieChart { Series = null };
            pieChart.UnsafeUpdate();

            //currently disabled
            //var stackedChart = new StackedBarChart { Series = null };
            //stackedChart.UnsafeUpdate();
        }
    }
}
