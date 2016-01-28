using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace WinForms.Scatter.Simple
{
    public partial class ScatterSample : Form
    {
        public ScatterSample()
        {
            InitializeComponent();
        }

        private void ScatterChart_Load(object sender, EventArgs e)
        {
            scatterChart1.Series.Add(new ScatterSeries
            {
                Title = "Sin",
                Values = new ChartValues<Point>
                {
                    new Point(0, Math.Sin(0)),
                    new Point(45, Math.Sin(45)),
                    new Point(90, Math.Sin(90)),
                    new Point(135, Math.Sin(135)),
                    new Point(180, Math.Sin(180)),
                    new Point(225, Math.Sin(225)),
                    new Point(270, Math.Sin(270)),
                    new Point(315, Math.Sin(315)),
                    new Point(360, Math.Sin(360))
                }
            });
        }
    }
}
