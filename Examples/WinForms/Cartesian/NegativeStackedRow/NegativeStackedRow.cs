using System;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.NegativeStackedRow
{
    public partial class NegativeStackedRow : Form
    {
        public NegativeStackedRow()
        {
            InitializeComponent();

            cartesianChart1.Series  = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Title = "Male",
                    Values = new ChartValues<double> {.5, .7, .8, .8, .6, .2, .6}
                },
                new StackedRowSeries
                {
                    Title = "Female",
                    Values = new ChartValues<double> {-.5, -.7, -.8, -.8, -.6, -.2, -.6}
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Age Range",
                Labels = new[] {"0-20", "20-35", "35-45", "45-55", "55-65", "65-70", ">70"}
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = value => Math.Abs(value).ToString("P")
            });
            
            var tooltip = new DefaultTooltip
            {
                SelectionMode = TooltipSelectionMode.SharedYValues
            };

            cartesianChart1.DataTooltip = tooltip;

        }
    }
}
