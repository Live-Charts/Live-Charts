using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace WinForms.LineExamples.Inverted
{
    public partial class RotatedLine : Form
    {
        public RotatedLine()
        {
            InitializeComponent();
        }

        private void RotatedBar_Load(object sender, EventArgs e)
        {
            lineChart1.LegendLocation = LegendLocation.Right;
            lineChart1.Invert = true;

            var config = new SeriesConfiguration<double>().X(val => val);
            lineChart1.Series = new SeriesCollection(config);

            lineChart1.Series.Add(new LineSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 3, 5, 8, 12, 8, 3 }
            });

            lineChart1.Series.Add(new LineSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 4, 2, 10, 11, 9, 4 }
            });

            lineChart1.AxisY[0].Labels = new List<string>
            {
                "Day 1",
                "Day 2",
                "Day 3",
                "Day 4",
                "Day 5",
                "Day 6"
            };
        }
    }
}
