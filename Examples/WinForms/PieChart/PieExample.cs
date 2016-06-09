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
using LiveCharts.Wpf;

namespace Winforms.PieChart
{
    public partial class PieExample : Form
    {
        public PieExample()
        {
            InitializeComponent();

            pieChart1.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Chrome",
                    Values = new ChartValues<double> {8},
                    PushOut = 15
                },
                new PieSeries
                {
                    Title = "Mozilla",
                    Values = new ChartValues<double> {6}
                },
                new PieSeries
                {
                    Title = "Opera",
                    Values = new ChartValues<double> {10}
                },
                new PieSeries
                {
                    Title = "Explorer",
                    Values = new ChartValues<double> {4}
                }
            };
        }
    }
}
