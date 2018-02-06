using Assets.Models;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.UnitTests.Mocked;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveCharts.Core.UnitTests
{
    [TestClass]
    public class ObservablesMemoryTest
    {
        [TestMethod]
        public void ObservablesMemoryLeaks()
        {
            var chart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries<City>
                    {
                        new City
                        {
                            Population = 10
                        }
                    }
                }
            };

            var newSeries = new SeriesCollection
            {
                new ColumnSeries<City>
                {
                    new City
                    {
                        Population = 10
                    }
                }
            };

            chart.Loaded();

            var c = chart.Model.InvalidateCount;

            chart.Series = newSeries;

            Assert.IsTrue(c == chart.Model.InvalidateCount + 1, "on seres property changed fired");
        }
    
    }
}