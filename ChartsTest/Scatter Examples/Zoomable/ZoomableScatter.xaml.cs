using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using lvc;

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
            Serie1.SecondaryValues = secondaryValues;
            Chart.AxisX.MaxValue = s1.Max();
            Chart.AxisX.MinValue = s1.Min();
            Chart.AxisY.MaxValue = secondaryValues.Max();
            Chart.AxisY.MinValue = secondaryValues.Min();
            Chart.AxisX.LabelFormatter = LabelFormatters.Number;
            Chart.AxisY.LabelFormatter = val => val.ToString("N1") + "°";
        }

        private void JustAreasAndZoomable_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
