using LiveCharts;
using LiveCharts.Uwp;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.Missing_Line_Points
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MissingPointsExample : Page
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
