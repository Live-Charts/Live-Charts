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
using LiveCharts.CoreComponents;

namespace WinForms.StackedBar.Simle
{
    public partial class StackedSample : Form
    {
        public StackedSample()
        {
            InitializeComponent();
        }

        private void StackedSample_Load(object sender, EventArgs e)
        {
            stackedBarChart1.Series.Add(new StackedBarSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 10, 5, 7 ,2}
            });
            stackedBarChart1.Series.Add(new StackedBarSeries
            {
                Title = "Another Series",
                Values = new ChartValues<double> { 6, 6, 4, 6 }
            });
        }
    }
}
