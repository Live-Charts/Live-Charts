using System;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.Inverted_Series
{
    public partial class InvertedSeries : Form
    {
        public InvertedSeries()
        {
            InitializeComponent();

            cartesianChart1.Series.Add(new VerticalLineSeries
            {
                Values = new ChartValues<double> { 3, 5, 2, 6, 2, 7, 1 }
            });

            cartesianChart1.Series.Add(new RowSeries
            {
                Values = new ChartValues<double> {6, 2, 6, 3, 2, 7, 2}
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Separator = new Separator { Step = 1}
            });

            cartesianChart1.AxisX.Add(new Axis
            {
                MinValue = 0
            });

            cartesianChart1.DataTooltip.SelectionMode = TooltipSelectionMode.SharedYValues;
        }

        private void InvertedSeries_Load(object sender, EventArgs e)
        {

        }
    }
}
