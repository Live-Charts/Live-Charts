using LiveCharts;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.StepLine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StepLineExample : Page
    {
        public StepLineExample()
        {
            this.InitializeComponent();
        }

        public IChartValues ChartValues1 { get; set; } = new ChartValues<int>(new int[] { 9, 6, 5, 7, 8, 9, 7, 6, 7, 5 });
        public IChartValues ChartValues2 { get; set; } = new ChartValues<int>(new int[] { 1, 4, 3, 1, 4, 2, 1, 2, 3, 5 });
    }
}
