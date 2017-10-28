using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.MultiAxes
{
    public partial class MultipleAxesExample : Form
    {
        public MultipleAxesExample()
        {
            InitializeComponent();

            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> {1, 5, 3, 5, 3},
                ScalesYAt = 0
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { 20, 30, 70, 20, 10 },
                ScalesYAt = 1
            });
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { 600, 300, 200, 600, 800 },
                ScalesYAt = 2
            });

            //now we add the 3 axes

            cartesianChart1.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.DodgerBlue,
                Title = "Blue Axis"
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.IndianRed,
                Title = "Red Axis",
                Position = AxisPosition.RightTop
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.DarkOliveGreen,
                Title = "Green Axis",
                Position = AxisPosition.RightTop
            });
        }
    }
}
