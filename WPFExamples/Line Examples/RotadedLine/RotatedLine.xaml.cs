using System;
using System.Windows;
using LiveCharts;

namespace ChartsTest.Line_Examples.RotadedLine
{
    /// <summary>
    /// Interaction logic for RotatedLine.xaml
    /// </summary>
    public partial class RotatedLine
    {
        public RotatedLine()
        {
            InitializeComponent();

            //we just need to create a configuration to map X, and Y
            var config = new SeriesConfiguration<double>()
                .X(value => value) // use stored value
                .Y((value, index) => index); //this line is not necessary, this is the default config, it is just to explain how LiveCharts works

            SeriesCollection =
                new SeriesCollection(config)
                {
                    new LineSeries
                    {
                        Title = "inverted series",
                        Values = new [] {10, 15, 18, 20, 15, double.NaN, double.NaN, -2, -1, 2, 3, 6}.AsChartValues(),
                        DataLabels = true
                    }
                };

            Formatter = val => "Day " + (val + 1);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to force redraw everytime this view loads
            Chart.Update();
        }
    }
}
