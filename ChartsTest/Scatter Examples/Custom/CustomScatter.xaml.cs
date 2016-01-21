using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using lvc;

namespace ChartsTest.Scatter_Examples
{
    public partial class CustomScatter
    {
        public CustomScatter()
        {
            InitializeComponent();

            var plotRange = new List<double>();
            for (double i = 0; i < 1; i+= 0.01d) plotRange.Add(i);

            Serie1.Values = plotRange.Select(x => new Point(x, Math.Asin(x))).AsChartValues();
            Serie2.Values = plotRange.Select(x => new Point(x, Math.Acos(x))).AsChartValues();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
