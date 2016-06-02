using System;
using System.Windows.Forms;
using Winforms.Cartesian.BasicLine;
using Winforms.Cartesian.Customized_Series;
using Winforms.Cartesian.DateTime;
using Winforms.Cartesian.FullyResponsive;
using Winforms.Cartesian.Inverted_Series;
using Winforms.Cartesian.Irregular_Intervals;
using Winforms.Cartesian.Labels;
using Winforms.Cartesian.LogarithmScale;
using Winforms.Cartesian.MissingPoints;
using Winforms.Cartesian.MultiAxes;
using Winforms.Cartesian.Sections;
using Winforms.Cartesian.StackedArea;
using Winforms.Cartesian.Zooming_and_Panning;

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

        private void button6_Click(object sender, EventArgs e)
        {
            new ZoomingAndPanningExample().ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new SectionsExample().ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new IrregularIntervalsExample().ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new BasicLineExample().ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new LogarithmSacale().ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            new MultipleAxesExample().ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            new StackedAreaExample().ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            new MissingPoint().ShowDialog();
        }
    }
}
