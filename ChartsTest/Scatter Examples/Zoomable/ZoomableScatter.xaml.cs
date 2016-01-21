using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;

namespace ChartsTest.Scatter_Examples
{
    public partial class ZoomableScatter
    {
        public ZoomableScatter()
        {
            InitializeComponent();
            var secondaryValues = new List<double>();
            for (double i = 0; i <= 1080; i += 36) secondaryValues.Add(i);
            var s1 = secondaryValues.Select(Math.Sin).ToList();
            Serie1.Values = s1.AsChartValues();
            Chart.AxisY.MaxValue = s1.Max();
            Chart.AxisY.MinValue = s1.Min();
            Chart.AxisX.MaxValue = secondaryValues.Max();
            Chart.AxisX.MinValue = secondaryValues.Min();
            Chart.AxisY.LabelFormatter = LabelFormatters.Number;
            Chart.AxisX.LabelFormatter = val => val.ToString("N1") + "°";
        }

        private void JustAreasAndZoomable_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
