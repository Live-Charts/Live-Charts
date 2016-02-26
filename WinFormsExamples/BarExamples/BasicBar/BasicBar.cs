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
                Values = new ChartValues<double> {4, 2, 10, 11, 9, 4},
                Fill = Brushes.Transparent
            });

            //Styling
            barChart1.AxisY = new Axis
            {
                Title = "Sold Items Change",
                IsEnabled = true,
                Color = Colors.LimeGreen,
                StrokeThickness = 4,
                Foreground = Brushes.MediumSeaGreen,
                FontSize = 13,
                FontFamily = new FontFamily("Arial"),
                Separator = new Separator
                {
                    Color = Colors.ForestGreen,
                    StrokeThickness = 2,
                    IsEnabled = true
                }
            };
            barChart1.AxisX = new Axis
            {
                Title = "Month",
                Labels = new List<string>
                {
                    "Jan","Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"
                },
                IsEnabled = true,
                Color = Colors.CornflowerBlue,
                StrokeThickness = 4,
                Foreground = Brushes.DeepSkyBlue,
                Separator = new Separator
                {
                    Color = Colors.DodgerBlue,
                    StrokeThickness = 2,
                    IsEnabled = true
                }
            };
        }

        private void BasicBar_Load(object sender, System.EventArgs e)
        {

        }
    }
}
