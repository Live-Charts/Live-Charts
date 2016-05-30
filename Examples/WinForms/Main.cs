using System;
using System.Windows.Forms;
using Winforms.Cartesian.Customized_Series;
using Winforms.Cartesian.DateTime;
using Winforms.Cartesian.FullyResponsive;
using Winforms.Cartesian.Inverted_Series;
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

        private void button4_Click(object sender, EventArgs e)
        {
            new InvertedSeries().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new DateTimeExample().ShowDialog();
        }
    }
}
