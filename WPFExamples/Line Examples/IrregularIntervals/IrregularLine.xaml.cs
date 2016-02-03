using System.Windows;
using System.Windows.Media;
using LiveCharts;

namespace ChartsTest.Line_Examples.IrregularIntervals
{
    /// <summary>
    /// Interaction logic for IrregularLine.xaml
    /// </summary>
    public partial class IrregularLine
    {
        public IrregularLine()
        {
            InitializeComponent();

            //we create a configuration to map our values type, in this case System.Windows.Point
            var config = new SeriesConfiguration<Point>()
                .X(point => point.X) // we use point.X as the X of our chart (you don't say!)
                .Y(point => point.Y); // we use point.Y as the Y of our chart -.-"

            //we pass the config to the SeriesCollection constructor, or you can use Series.Setup(config)
            Series = new SeriesCollection(config)
            {
                new LineSeries
                {
                    Values = new ChartValues<Point>
                    {
                        new Point(1, 10),
                        new Point(2, 15),
                        new Point(4, 29),
                        new Point(8, 38),
                        new Point(16, 45),
                        new Point(32, 55),
                        new Point(64, 62),
                        new Point(128, 76),
                        new Point(256, 95)
                    },
                    Fill = Brushes.Transparent
                }
            };
            
            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

        private void IrregularLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is only too see animation everytime you change the view.
            Chart.ClearAndPlot();
        }
    }
}
