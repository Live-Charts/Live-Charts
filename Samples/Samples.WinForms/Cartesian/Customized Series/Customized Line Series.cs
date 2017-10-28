using System;
using System.Drawing;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.Customized_Series
{
    public partial class CustomizedSeries : Form
    {
        public CustomizedSeries()
        {
            InitializeComponent();
        }

        private void CustomizedLineSeries_Load(object sender, EventArgs e)
        {
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { 3, 4, 6, 3, 2, 6 },
                StrokeThickness = 4,
                StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 2 }),
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 185, 69)),
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 0,
                PointGeometrySize = 0
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { 5, 3, 5, 7, 3, 9 },
                StrokeThickness = 2,
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(28, 142, 196)),
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 1,
                PointGeometrySize = 15,
                PointForeground =
                    new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49))
            });

            cartesianChart1.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49));

            cartesianChart1.AxisX.Add(new Axis
            {
                IsMerged = true,
                Separator = new Separator
                {
                    StrokeThickness = 1,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 2 }),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                IsMerged = true,
                Separator = new Separator
                {
                    StrokeThickness = 1.5,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 4 }),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }
            });
        }
    }
}
