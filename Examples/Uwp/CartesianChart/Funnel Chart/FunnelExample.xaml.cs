using Windows.UI.Xaml.Controls;
using LiveCharts;

namespace UWP.CartesianChart.Funnel_Chart
{
    /// <summary>
    /// Interaction logic for FunnelExample.xaml
    /// </summary>
    public partial class FunnelExample : Page
    {
        public FunnelExample()
        {
            InitializeComponent();

            V1 = new ChartValues<double> {100, 85, 50, 35, 5, 3};
            V2 = new ChartValues<double> {-100, -85, -50, -35, -5, 3};
            V3 = new ChartValues<double> {110, 94, 60, 40, 10, 10};
            V4 = new ChartValues<double> {-110, -94, -60, -40, -10, -10};
            V5 = new ChartValues<double> {120, 104, 70, 50, 15, 15};
            V6 = new ChartValues<double> {-120, -104, -70, -50, -15, -15};

            DataContext = this;
        }

        public ChartValues<double> V1 { get; set; }
        public ChartValues<double> V2 { get; set; }
        public ChartValues<double> V3 { get; set; }
        public ChartValues<double> V4 { get; set; }
        public ChartValues<double> V5 { get; set; }
        public ChartValues<double> V6 { get; set; }
    }
}
