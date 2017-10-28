using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.StackedArea
{
    public partial class StackedAreaExample : Form
    {
        public StackedAreaExample()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
            {
                new StackedAreaSeries
                {
                    Title = "Africa",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new System.DateTime(1950, 1, 1), .228),
                        new DateTimePoint(new System.DateTime(1960, 1, 1), .285),
                        new DateTimePoint(new System.DateTime(1970, 1, 1), .366),
                        new DateTimePoint(new System.DateTime(1980, 1, 1), .478),
                        new DateTimePoint(new System.DateTime(1990, 1, 1), .629),
                        new DateTimePoint(new System.DateTime(2000, 1, 1), .808),
                        new DateTimePoint(new System.DateTime(2010, 1, 1), 1.031),
                        new DateTimePoint(new System.DateTime(2013, 1, 1), 1.110)
                    },
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "N & S America",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new System.DateTime(1950, 1, 1), .339),
                        new DateTimePoint(new System.DateTime(1960, 1, 1), .424),
                        new DateTimePoint(new System.DateTime(1970, 1, 1), .519),
                        new DateTimePoint(new System.DateTime(1980, 1, 1), .618),
                        new DateTimePoint(new System.DateTime(1990, 1, 1), .727),
                        new DateTimePoint(new System.DateTime(2000, 1, 1), .841),
                        new DateTimePoint(new System.DateTime(2010, 1, 1), .942),
                        new DateTimePoint(new System.DateTime(2013, 1, 1), .972)
                    },
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Asia",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new System.DateTime(1950, 1, 1), 1.395),
                        new DateTimePoint(new System.DateTime(1960, 1, 1), 1.694),
                        new DateTimePoint(new System.DateTime(1970, 1, 1), 2.128),
                        new DateTimePoint(new System.DateTime(1980, 1, 1), 2.634),
                        new DateTimePoint(new System.DateTime(1990, 1, 1), 3.213),
                        new DateTimePoint(new System.DateTime(2000, 1, 1), 3.717),
                        new DateTimePoint(new System.DateTime(2010, 1, 1), 4.165),
                        new DateTimePoint(new System.DateTime(2013, 1, 1), 4.298)
                    },
                    LineSmoothness = 0
                },
                new StackedAreaSeries
                {
                    Title = "Europe",
                    Values = new ChartValues<DateTimePoint>
                    {
                        new DateTimePoint(new System.DateTime(1950, 1, 1), .549),
                        new DateTimePoint(new System.DateTime(1960, 1, 1), .605),
                        new DateTimePoint(new System.DateTime(1970, 1, 1), .657),
                        new DateTimePoint(new System.DateTime(1980, 1, 1), .694),
                        new DateTimePoint(new System.DateTime(1990, 1, 1), .723),
                        new DateTimePoint(new System.DateTime(2000, 1, 1), .729),
                        new DateTimePoint(new System.DateTime(2010, 1, 1), .740),
                        new DateTimePoint(new System.DateTime(2013, 1, 1), .742)
                    },
                    LineSmoothness = 0
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = val => new System.DateTime((long)val).ToString("yyyy")
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = val => val.ToString("N") + " M"
            });
        }
    }
}
