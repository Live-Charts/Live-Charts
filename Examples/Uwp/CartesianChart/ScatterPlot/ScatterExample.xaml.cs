using LiveCharts;
using LiveCharts.Defaults;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.ScatterPlot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScatterExample : Page
    {
        public ScatterExample()
        {
            InitializeComponent();

            var r = new Random();
            ValuesA = new ChartValues<ObservablePoint>();
            ValuesB = new ChartValues<ObservablePoint>();
            ValuesC = new ChartValues<ObservablePoint>();

            for (var i = 0; i < 20; i++)
            {
                ValuesA.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
                ValuesB.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
                ValuesC.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
            }

            DataContext = this;
        }

        public ChartValues<ObservablePoint> ValuesA { get; set; }
        public ChartValues<ObservablePoint> ValuesB { get; set; }
        public ChartValues<ObservablePoint> ValuesC { get; set; }

        private void RandomizeOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            for (var i = 0; i < 20; i++)
            {
                ValuesA[i].X = r.NextDouble() * 10;
                ValuesA[i].Y = r.NextDouble() * 10;
                ValuesB[i].X = r.NextDouble() * 10;
                ValuesB[i].Y = r.NextDouble() * 10;
                ValuesC[i].X = r.NextDouble() * 10;
                ValuesC[i].Y = r.NextDouble() * 10;
            }
        }
    }
}
