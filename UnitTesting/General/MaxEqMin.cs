using System.Windows;
using LiveCharts;
using LiveCharts.CoreComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting.General
{
    public partial class GeneralTest
    {
        [TestMethod, TestCategory("General")]
        public void MaxEqMinValues()
        {
            var vals = new ChartValues<double> {1, 1};

            var barChart = new BarChart
            {
                Series = new SeriesCollection
                {
                    new BarSeries {Values = vals},
                    new LineSeries {Values = vals}
                }
            };
            barChart.UnsafeUpdate();

            var lineChart = new LineChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries {Values = vals}
                }
            };
            lineChart.UnsafeUpdate();

            var pieChart = new PieChart
            {
                Series = new SeriesCollection
                {
                    new PieSeries {Values = vals}
                }
            };
            pieChart.UnsafeUpdate();

            var stackedChart = new StackedBarChart
            {
                Series = new SeriesCollection
                {
                    new StackedBarSeries {Values = vals}
                }
            };
            stackedChart.UnsafeUpdate();
        }
    }
}
