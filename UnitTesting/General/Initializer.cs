using System.Windows;
using LiveCharts.CoreComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting.General
{
    [TestClass]
    public partial class GeneralTest
    {
        [AssemblyInitialize]
        public static void Initializer(TestContext context)
        {
            Chart.MockedArea = new Rect { X = 0, Y = 0, Width = 800, Height = 600 };
        }
    }
}
