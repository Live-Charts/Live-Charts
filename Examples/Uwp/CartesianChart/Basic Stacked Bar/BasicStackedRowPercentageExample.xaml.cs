using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.Basic_Stacked_Bar
{
    using LiveCharts;
    using LiveCharts.Uwp;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasicStackedRowPercentageExample : Page
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

            this.Tooltip = new DefaultTooltip() { SelectionMode = TooltipSelectionMode.SharedYValues };
        }

        public UserControl Tooltip;

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
