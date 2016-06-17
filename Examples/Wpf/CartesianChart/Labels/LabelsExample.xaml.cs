using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Labels
{
    /// <summary>
    /// Interaction logic for BarExample.xaml
    /// </summary>
    public partial class LabelsExample : UserControl
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

        private void Chart_OnDataClick(object sender, ChartPoint point)
        {
            //point instance contains many useful information...
            //sender is the shape that called the event.

            MessageBox.Show("You clicked " + point.X + ", " + point.Y);
          
        }
    }
}
