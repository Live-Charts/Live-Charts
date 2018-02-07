using System;
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
                },
                AnimationsSpeed = TimeSpan.FromMilliseconds(100)
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

            chart.UpdatePreview += sender =>
            {
                Assert.IsTrue(c + 1 == chart.Model.InvalidateCount, "on seres property changed fired");
            };

            chart.Series = newSeries;
        }
    
    }
}