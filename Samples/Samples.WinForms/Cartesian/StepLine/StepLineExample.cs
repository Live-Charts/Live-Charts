using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.StepLine
{
    public partial class StepLineExample : Form
    {
        public StepLineExample()
        {
            InitializeComponent();

            cartesianChart1.Series.Add(new StepLineSeries
            {
                Values = new ChartValues<double> { 9, 6, 5, 7, 8, 9, 7, 6, 7, 5 }
            });

            cartesianChart1.Series.Add(new StepLineSeries
            {
                Values = new ChartValues<double> {1, 4, 3, 1, 4, 2, 1, 2, 3, 5},
                AlternativeStroke = Brushes.Transparent,
                StrokeThickness = 3,
                PointGeometry = null
            });
        }
    }
}
