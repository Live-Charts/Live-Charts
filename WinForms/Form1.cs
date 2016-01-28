using System;
using System.Windows.Forms;
using LiveCharts;
using WinForms.BarExamples.BasicLine;
using WinForms.BarExamples.Mvvm;
using WinForms.BarExamples.Rotated;
using WinForms.LineExamples.Inverted;
using WinForms.LineExamples.Mvvm;
using WinForms.LineExamples.Simple;
using WinForms.PieExamples.Doughnut;
using WinForms.PieExamples.Pie;
using WinForms.Scatter.Simple;
using WinForms.StackedBar.Simle;

namespace WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            var sample = new BasicBar();
            sample.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var sample = new RotatedBar();
            sample.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var sample = new MvvmBar();
            sample.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var sample = new SimpleLine();
            sample.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var sample = new RotatedLine();
            sample.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var sample = new MvvmLine();
            sample.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var sample = new PieSample();
            sample.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var sample = new DoughnutSample();
            sample.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var sample = new ScatterSample();
            sample.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var sample = new StackedSample();
            sample.ShowDialog();
        }
    }
}
