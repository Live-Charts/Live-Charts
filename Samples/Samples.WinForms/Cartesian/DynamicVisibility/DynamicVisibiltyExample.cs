using System.Windows;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.DynamicVisibility
{
    public partial class DynamicVisibiltyExample : Form
    {
        public DynamicVisibiltyExample()
        {
            InitializeComponent();

            MariaSeries = new LineSeries
            {
                Values = new ChartValues<double> {4, 6, 2, 7, 6}
            };
            CharlesSeries = new LineSeries
            {
                Values = new ChartValues<double> {6, 2, 8, 3, 5}
            };
            JohnSeries = new LineSeries
            {
                Values = new ChartValues<double> {8, 2, 4, 7, 2}
            };

            cartesianChart1.Series = new SeriesCollection
            {
                MariaSeries,
                CharlesSeries,
                JohnSeries
            };

        }

        public LineSeries MariaSeries { get; set; }
        public LineSeries CharlesSeries { get; set; }
        public LineSeries JohnSeries { get; set; }

        private void ToggleMariaSeries(object sender, System.EventArgs e)
        {
            MariaSeries.Visibility = MariaSeries.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        private void ToggleCharlesSeries(object sender, System.EventArgs e)
        {
            CharlesSeries.Visibility = CharlesSeries.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        private void ToggleJohnSeries(object sender, System.EventArgs e)
        {
            JohnSeries.Visibility = JohnSeries.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }
    }
}
