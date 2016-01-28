using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;

namespace WinForms.PieExamples.Doughnut
{
    public partial class DoughnutSample : Form
    {
        public DoughnutSample()
        {
            InitializeComponent();
        }

        private void DoughnutSample_Load(object sender, EventArgs e)
        {
            pieChart1.InnerRadius = 100;
            pieChart1.Series.Add(new PieSeries
            {
                Values = new double[] { 5, 10, 3, 7 }.AsChartValues()
            });
        }
    }
}
