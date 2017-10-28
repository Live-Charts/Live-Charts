using System;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Basic_Bars
{
    public partial class BasicRowExample : UserControl
    {
        public BasicRowExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new RowSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42}
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[1].Values.Add(48d);

            Labels = new[] {"Maria", "Susan", "Charles", "Frida"};
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }
}
