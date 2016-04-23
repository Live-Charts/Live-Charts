using System.Windows;
using LiveChartsCore;

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
                new LiveChartsDesktop.LineSeries
                {
                    Values = new ChartValues<double> {1, 2, 3}
                }
            };
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}
