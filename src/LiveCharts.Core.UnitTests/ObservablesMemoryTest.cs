using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveCharts.Core.UnitTests
{
    [TestClass]
    public class ObservablesMemoryTest
    {
        [TestMethod]
        public void ObservablesMemoryLeaks()
        {
            var chart = new MockedChart();
        }
    }
}