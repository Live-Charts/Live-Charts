using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.CoreComponents;

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
                Values = new ChartValues<double> { 3, 5, 8, 12, 8 ,3}
            });

            barChart1.Series.Add(new BarSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 4, 2, 10, 11, 9, 4 }
            });

            //It supports line series too!
            barChart1.Series.Add(new LineSeries
            {
                Title = "A Line Series",
                Values = new ChartValues<double> { 4, 2, 10, 11, 9, 4 },
                Fill = Brushes.Transparent
            });

            barChart1.AxisX.Labels = new List<string>
            {
                "Day 1",
                "Day 2",
                "Day 3",
                "Day 4",
                "Day 5",
                "Day 6"
            };
        }

        private void BasicBar_Load(object sender, System.EventArgs e)
        {

        }
    }
}
