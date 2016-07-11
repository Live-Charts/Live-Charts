using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.Basic_Stacked_Bar
{
    public partial class BasicsStackedRowExample : Form
    {
        public BasicsStackedRowExample()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
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
            cartesianChart1.Series.Add(new StackedRowSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Percentage,
                DataLabels = true,
                LabelPoint = p => p.X.ToString()
            });

            //adding values also updates and animates
            cartesianChart1.Series[2].Values.Add(4d);

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Browser",
                Labels = new[] {"Chrome", "Mozilla", "Opera", "IE"}
            });
            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = val => val.ToString("P")
            });

            var tooltip = new DefaultTooltip {SelectionMode = TooltipSelectionMode.SharedYValues};

            cartesianChart1.DataTooltip = tooltip;

        }
    }
}
