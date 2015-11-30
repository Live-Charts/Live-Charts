using System;
using System.Collections.Generic;
using System.Windows;
using LiveCharts;

namespace ChartsTest.Line_Examples
{
    public partial class PerformanceLine
    {
        public PerformanceLine()
        {
            InitializeComponent();
            var l = new List<double>();
            var r = new Random();
            for (double i = 0; i < 1000000; i++) l.Add(r.Next(0, 30));
            DataContext = new ViewModel
            {
                Values = l.ToArray(),
                Name = "Sales"
            };
            Chart.PerformanceConfiguration = new PerformanceConfiguration();
        }

        private void MvvmPerformanceOptimization_OnLoaded(object sender, RoutedEventArgs e)
        {
            var startedAt = DateTime.Now;
            Chart.ClearAndPlot();
            //this normally returns less than 30ms, visually it looks bigger, clear and plot method
            //takes 100ms to draw the changes, it is maybe because of that
            //ToDo: be sure of this.
            OutPut.Text = "Performance Optimization. In this example line chart is taking 1 million points to plot. " +
                          "In this run LiveCharts drew one million points in " +
                          (DateTime.Now - startedAt).TotalMilliseconds + " ms";
        }
    }

    public class ViewModel
    {
        public double[] Values { get; set; }
        public string Name { get; set; }
    }
}
