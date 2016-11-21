using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Energy_Predictions
{
    /// <summary>
    /// Interaction logic for EnergyPredictionExample.xaml
    /// </summary>
    public partial class EnergyPredictionExample : UserControl
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

        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ListBox, (DependencyObject)e.OriginalSource) as ListBoxItem;
            if (item == null) return;

            var series = (StackedAreaSeries) item.Content;
            series.Visibility = series.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }
    }
}
