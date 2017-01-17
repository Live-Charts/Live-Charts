using LiveCharts;
using System;
using Windows.UI.Xaml.Controls;
using LiveCharts.Uwp;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.UIElements
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UIElementsExample : Page
    {
        public UIElementsExample()
        {
            InitializeComponent();

            DataContext = this;
        }

        public IChartValues Values1 { get; set; } = new ChartValues<int>(new [] { 4, 6, 6, 3, 5 });
        public IChartValues Values2 { get; set; } = new ChartValues<int>(new [] { 2, 6, 8, 9, 4 });
        public string[] Labels { get; set; } = { "Jan", "Feb", "Mar", "Abr", "May" };
    }
}
