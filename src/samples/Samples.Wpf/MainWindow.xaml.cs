using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts.Core.Abstractions;
using LiveCharts.Wpf;

namespace Samples.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Series = new ObservableCollection<IChartSeries>
            {
                new ColumnSeries<double>
                {
                    Values = new List<double>() {3, 2, 1}
                }
            };
        }

        public ObservableCollection<IChartSeries> Series { get; set; }
    }
}
