using LiveCharts;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.LineExample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LineExample : Page
    {
        public LineExample()
        {
            InitializeComponent();

            Values = Values = new ChartValues<float>
            {
                3,
                4,
                6,
                3,
                2,
                6
            };

            DataContext = this;
        }

        public ChartValues<float> Values { get; set; }
    }
}
