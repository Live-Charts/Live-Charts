using System;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.DataPagination
{
    public partial class DataPaginationExample : Form
    {
        public DataPaginationExample()
        {
            InitializeComponent();

            var values = new ChartValues<double>();

            var r = new Random();
            for (var i = 0; i < 100; i++)
            {
                values.Add(r.Next(0, 10));
            }

            cartesianChart1.Series.Add(new LineSeries
            {
                Values = values
            });
            
            cartesianChart1.AxisX.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 25
            });
        }

        private void PreviousOnClick(object sender, EventArgs e)
        {
            cartesianChart1.AxisX[0].MinValue -= 25;
            cartesianChart1.AxisX[0].MaxValue -= 25;
        }

        private void NextOnClick(object sender, EventArgs e)
        {
            cartesianChart1.AxisX[0].MinValue += 25;
            cartesianChart1.AxisX[0].MaxValue += 25;
        }

        private void CustomZoomOnClick(object sender, EventArgs e)
        {
            cartesianChart1.AxisX[0].MinValue = 5;
            cartesianChart1.AxisX[0].MaxValue = 10;
        }
    }
}
