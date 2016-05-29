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
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Winforms.Cartesian.Customized_Series;
using Winforms.Cartesian.FullyResponsive;
using Winforms.Cartesian.Labels;

namespace Winforms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void LabelsOnClick(object sender, EventArgs e)
        {
            new Labels().ShowDialog();
        }

        private void FullyResponsiveOnClick(object sender, EventArgs e)
        {
            new FullyResponsive().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new CustomizedSeries().ShowDialog();
        }
    }
}
