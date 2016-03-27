using System;
using LiveCharts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting.PieChartTest
{
    [TestClass]
    public class PieSumEqualsToZero
    {
        [TestMethod, TestCategory("PieChart")]
        public void SumsZero()
        {
            var pieChart = new PieChart
            {
                Series = new SeriesCollection
                {
                    new PieSeries {Values = new ChartValues<double> {0}},
                    new PieSeries {Values = new ChartValues<double> {0}},
                    new PieSeries {Values = new ChartValues<double> {0}}
                }
            };
            pieChart.UnsafeUpdate();
        }
    }
}
