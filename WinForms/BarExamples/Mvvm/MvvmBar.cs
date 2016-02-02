using System;
using System.Windows.Forms;
using LiveCharts;
using WinForms.LineExamples.Mvvm;

namespace WinForms.BarExamples.Mvvm
{
    public partial class MvvmBar : Form
    {
        public MvvmBar()
        {
            InitializeComponent();
        }

        public SeriesModel Model { get; set; }

        private void MvvmBar_Load(object sender, EventArgs e)
        {
            Model = new SeriesModel();
            barChart1.Series = Model;
            barChart1.LegendLocation = LegendLocation.Left;
            //if you need a custom tooltip dasly you need to use wpf for now
            barChart1.DataTooltip = new LineCustomTooltip();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Model.AddPoint();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Model.RemovePoint();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Model.AddRandomSeries();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Model.RemoveSeries();
        }
    }
}
