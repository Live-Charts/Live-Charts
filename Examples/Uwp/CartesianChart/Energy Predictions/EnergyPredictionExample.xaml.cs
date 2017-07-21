using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using LiveCharts;
using LiveCharts.Uwp;

namespace UWP.CartesianChart.Energy_Predictions
{
    /// <summary>
    /// Interaction logic for EnergyPredictionExample.xaml
    /// </summary>
    public partial class EnergyPredictionExample
    {
        public EnergyPredictionExample()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {20, 30, 35, 45, 65, 85},
                    Title = "Electricity"
                },
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {10, 12, 18, 20, 38, 40},
                    Title = "Water"
                },
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {5, 8, 12, 15, 22, 25},
                    Title = "Solar"
                },
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {10, 12, 18, 20, 38, 40},
                    Title = "Gas"
                }
            };

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var series = (StackedAreaSeries) ((ContentPresenter) sender).Content;
            if (series != null)
                series.Visibility = series.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
        }
    }
}
