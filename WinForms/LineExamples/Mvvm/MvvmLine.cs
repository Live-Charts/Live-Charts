using System.Windows.Forms;
using LiveCharts;

namespace WinForms.LineExamples.Mvvm
{
    public partial class MvvmLine : Form
    {
        public MvvmLine()
        {
            InitializeComponent();
        }

        public SeriesModel Model { get; set; }

        private void MvvmBar_Load(object sender, System.EventArgs e)
        {
            Model = new SeriesModel();
            lineChart1.Series = Model;
            lineChart1.LegendLocation = LegendLocation.Right;
            //if you need a custom tooltip dasly you need to use wpf for now
            lineChart1.DataToolTip = new LineCustomTooltip();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Model.AddPoint();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Model.RemovePoint();
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Model.AddRandomSeries();
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            Model.RemoveSeries();
        }
    }
}
