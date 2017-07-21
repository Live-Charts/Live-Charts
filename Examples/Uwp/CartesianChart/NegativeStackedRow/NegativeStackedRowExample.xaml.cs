using LiveCharts;
using LiveCharts.Uwp;
using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.NegativeStackedRow
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NegativeStackedRowExample : Page
    {
        public NegativeStackedRowExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Title = "Male",
                    Values = new ChartValues<double> {.5, .7, .8, .8, .6, .2, .6}
                },
                new StackedRowSeries
                {
                    Title = "Female",
                    Values = new ChartValues<double> {-.5, -.7, -.8, -.8, -.6, -.2, -.6}
                }
            };

            Labels = new[] { "0-20", "20-35", "35-45", "45-55", "55-65", "65-70", ">70" };
            Formatter = value => Math.Abs(value).ToString("P");

            DataContext = this;

        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
