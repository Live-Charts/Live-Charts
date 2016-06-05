using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.Financial
{
    public partial class FinancialExample : Form
    {
        public FinancialExample()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
            {
                new OhlcSeries
                {
                    Values = new ChartValues<OhlcPoint>
                    {
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33)
                    }
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {30, 32, 35, 30, 28},
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };

            //based on https://github.com/beto-rodriguez/Live-Charts/issues/166 
            //The Ohcl point X property is zero based indexed.
            //this means the first point is 0, second 1, third 2.... and so on
            //then you can use the Axis.Labels properties to map the chart X with a label in the array.
            //for more info see (mapped labels section) 
            //http://lvcharts.net/#/examples/v1/labels-wpf?path=WPF-Components-Labels

            cartesianChart1.AxisX.Add(new Axis
            {
                Labels = new[]
                {
                    System.DateTime.Now.ToString("dd MMM"),
                    System.DateTime.Now.AddDays(1).ToString("dd MMM"),
                    System.DateTime.Now.AddDays(2).ToString("dd MMM"),
                    System.DateTime.Now.AddDays(3).ToString("dd MMM"),
                    System.DateTime.Now.AddDays(4).ToString("dd MMM")
                }
            });

        }
    }
}
