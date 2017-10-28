using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.MissingPoints
{
    public partial class MissingPoint : Form
    {
        public MissingPoint()
        {
            InitializeComponent();

            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double>
                {
                    4,
                    5,
                    7,
                    8,
                    double.NaN,
                    5,
                    2,
                    8,
                    double.NaN,
                    6,
                    2
                }
            });

        }
    }
}
