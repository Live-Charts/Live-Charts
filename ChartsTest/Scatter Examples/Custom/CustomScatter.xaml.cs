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
            var secondaryValues = new List<double>();
            for (double i = 0; i < 1; i+= 0.01d) secondaryValues.Add(i);
            var s1 = secondaryValues.Select(Math.Asin).ToList();
            var s2 = secondaryValues.Select(Math.Acos).ToList();
            Serie1.Values = s1.AsChartValues(val => 0, val => val);
            Serie1.SecondaryValues = secondaryValues;
            Serie2.Values = s2.AsChartValues(val => 9, val => val);
            Serie2.SecondaryValues = secondaryValues;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }
}
