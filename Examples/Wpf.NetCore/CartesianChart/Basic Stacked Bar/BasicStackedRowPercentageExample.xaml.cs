using System;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Basic_Stacked_Bar
{
    /// <summary>
    /// Interaction logic for BasicStackedRowPercentageExample.xaml
    /// </summary>
    public partial class BasicStackedRowPercentageExample : UserControl
    {
        public BasicStackedRowPercentageExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    LabelPoint = p => p.X.ToString()
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    LabelPoint = p => p.X.ToString()
                }
            };

            //adding series updates and animates the chart
            SeriesCollection.Add(new StackedRowSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Percentage,
                DataLabels = true,
                LabelPoint = p => p.X.ToString()
            });

            //adding values also updates and animates
            SeriesCollection[2].Values.Add(4d);

            Labels = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            Formatter = val => val.ToString("P");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }
}
