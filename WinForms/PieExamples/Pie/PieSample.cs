using System;
using System.Windows.Forms;
using LiveCharts;

namespace WinForms.PieExamples.Pie
{
    public partial class PieSample : Form
    {
        public PieSample()
        {
            InitializeComponent();
        }

        private void PieSample_Load(object sender, EventArgs e)
        {
            //pie only supports one series
            pieChart1.LegendLocation = LegendLocation.Left;
            pieChart1.Series.Add(new PieSeries
            {
                Values = new double[] { 5, 10, 3, 7}.AsChartValues()
            });
        }
    }
}
