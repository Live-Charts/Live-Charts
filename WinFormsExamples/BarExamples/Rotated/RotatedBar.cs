using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace WinForms.BarExamples.Rotated
{
    public partial class RotatedBar : Form
    {
        public RotatedBar()
        {
            InitializeComponent();
        }

        private void RotatedBar_Load(object sender, EventArgs e)
        {
            barChart1.Invert = true;
            barChart1.LegendLocation = LegendLocation.Top;

            var config = new SeriesConfiguration<double>().X(val => val);
            barChart1.Series = new SeriesCollection(config);

            barChart1.Series.Add(new BarSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 3, 5, 8, 12, 8, 3 }
            });

            barChart1.Series.Add(new BarSeries
            {
                Title = "A Series",
                Values = new ChartValues<double> { 4, 2, 10, 11, 9, 4 }
            });

            barChart1.AxisY.Labels = new List<string>
            {
                "Day 1",
                "Day 2",
                "Day 3",
                "Day 4",
                "Day 5",
                "Day 6"
            };
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
