using System.Windows;
using LiveCharts;
using LiveCharts.CoreComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting.General
{
    public partial class GeneralTest
    {

        //Charts should be able to scale correclty even when the scale range is zero
        //max - min == 0

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

            //currently disabled
            //var stackedChart = new StackedBarChart
            //{
            //    Series = new SeriesCollection
            //    {
            //        new StackedBarSeries {Values = vals}
            //    }
            //};
            //stackedChart.UnsafeUpdate();
        }
    }
}
