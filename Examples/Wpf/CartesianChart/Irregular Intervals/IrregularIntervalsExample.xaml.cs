using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Irregular_Intervals
{

    public partial class IrregularIntervalsExample : UserControl
    {
        public IrregularIntervalsExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(0, 10),
                        new ObservablePoint(4, 7),
                        new ObservablePoint(5, 3),
                        new ObservablePoint(7, 6),
                        new ObservablePoint(10, 8)
                    },
                    PointGeometrySize = 15
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(0, 2),
                        new ObservablePoint(2, 5),
                        new ObservablePoint(3, 6),
                        new ObservablePoint(6, 8),
                        new ObservablePoint(10, 5)
                    },
                    PointGeometrySize = 15
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(0, 4),
                        new ObservablePoint(5, 5),
                        new ObservablePoint(7, 7),
                        new ObservablePoint(9, 10),
                        new ObservablePoint(10, 9)
                    },
                    PointGeometrySize = 15
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}
