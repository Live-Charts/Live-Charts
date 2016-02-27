using System.Windows.Forms;
using LiveCharts;

namespace WinForms.BarExamples.BasicLine
{
    public partial class BasicBar : Form
    {
        public BasicBar()
        {
            InitializeComponent();

            barChart1.LegendLocation = LegendLocation.Left;

            barChart1.Series.Add(new BarSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> {3, 5, 8, 12, 8, 3}
            });

            barChart1.Series.Add(new BarSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> {4, 2, 10, 11, 9, 4}
            });

            //It supports line series too!
            barChart1.Series.Add(new LineSeries
            {
                Title = "A Line Series",
                Values = new ChartValues<double> {4, 2, 10, 11, 9, 4}
            });
           
        }

        private void BasicBar_Load(object sender, System.EventArgs e)
        {

        }
    }
}
