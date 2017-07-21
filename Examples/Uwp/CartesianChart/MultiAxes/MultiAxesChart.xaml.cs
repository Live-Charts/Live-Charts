using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Uwp;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.MultiAxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MultiAxesChart : Page
    {
        public MultiAxesChart()
        {
            this.InitializeComponent();
        }

        public ISeparatorView CleanSeparator { get; set; } = DefaultAxes.CleanSeparator;

        public IChartValues LineValues1 { get; set; } = new ChartValues<int>(new int[] { 1, 5, 3, 5, 3 });
        public IChartValues LineValues2 { get; set; } = new ChartValues<int>(new int[] { 20, 30, 70, 20, 10 });
        public IChartValues LineValues3 { get; set; } = new ChartValues<int>(new int[] { 600, 300, 200, 600, 800 });
    }
}
