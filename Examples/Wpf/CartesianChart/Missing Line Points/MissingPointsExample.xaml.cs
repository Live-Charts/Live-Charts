using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Missing_Line_Points
{
    public partial class MissingPointsExample : UserControl
    {
        public MissingPointsExample()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new LineSeries
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
                }
            };

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

    }
}
