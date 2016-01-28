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

namespace WinForms.LineExamples.Simple
{
    public partial class SimpleLine : Form
    {
        public SimpleLine()
        {
            InitializeComponent();
        }

        private void SimpleLine_Load(object sender, EventArgs e)
        {
            lineChart1.LegendLocation = LegendLocation.Left;

            lineChart1.Series.Add(new LineSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 3, 5, 8, 12, 8, 3 }
            });

            lineChart1.Series.Add(new LineSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 4, 2, 10, 11, 9, 4 }
            });


            lineChart1.AxisX.Labels = new List<string>
            {
                "Day 1",
                "Day 2",
                "Day 3",
                "Day 4",
                "Day 5",
                "Day 6"
            };
        }
    }
}
