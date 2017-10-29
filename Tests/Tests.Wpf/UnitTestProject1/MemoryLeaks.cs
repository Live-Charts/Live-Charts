using System.Diagnostics;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Wpf
{
    [TestClass]
    public class MemoryLeaks
    {
        [TestMethod]
        public void UiElementsmemoryLeaks()
        {
            var chart = new CartesianChart
            {
                DisableAnimations = true,
                Width = 500,
                Height = 500,
                Series = new SeriesCollection(),
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        ShowLabels = false,
                        Separator = new Separator
                        {
                            IsEnabled = false
                        }
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        ShowLabels = false,
                        Separator = new Separator
                        {
                            IsEnabled = false
                        }
                    }
                }
            };

            foreach (var series in Utils.TestSeries)
            {
                var seriesName = series.GetType().Name;
                Trace.WriteLine(seriesName);

            }
        }
    }
}
