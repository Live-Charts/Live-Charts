using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.Labels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LabelsExample : Page
    {
        public LabelsExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(8),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(0),
                        new ObservableValue(1),
                    },
                    DataLabels = true,
                    LabelPoint = point => point.X + "K ," + point.Y
                }
            };

            Labels = new[]
            {
                "Shea Ferriera",
                "Maurita Powel",
                "Scottie Brogdon",
                "Teresa Kerman",
                "Nell Venuti",
                "Anibal Brothers",
                "Anderson Dillman"
            };

            Formatter = value => value + ".00K items";

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                foreach (var observable in series.Values.Cast<ObservableValue>())
                {
                    observable.Value = r.Next(0, 10);
                }
            }
        }

        private async void Chart_OnDataClick(object sender, ChartPoint point)
        {
            //point instance contains many useful information...
            //sender is the shape that called the event.
            var dialog = new MessageDialog("You clicked " + point.X + ", " + point.Y);
            await dialog.ShowAsync();


        }
    }
}
