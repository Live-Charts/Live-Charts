using LiveCharts;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.Inverted_Series
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvertedExample : Page
    {
        public InvertedExample()
        {
            InitializeComponent();

            Values1 = new ChartValues<double> { 3, 5, 2, 6, 2, 7, 1 };

            Values2 = new ChartValues<double> { 6, 2, 6, 3, 2, 7, 2 };

            DataContext = this;
        }

        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }
    }
}
