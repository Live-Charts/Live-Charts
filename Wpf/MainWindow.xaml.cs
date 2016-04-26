using System.Windows;
using System.Windows.Media;
using LiveChartsCore;
using LiveChartsDesktop;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {0, 2, 4, 8, 16, 32, 64},
                    Stroke = Brushes.Green,
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush {Color = Colors.Green, Opacity = .3}
                }
            };
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}
