using System;
using System.Windows;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.DynamicVisibility
{
    public partial class DynamicVisiblityExample : Form
    {
        public DynamicVisiblityExample()
        {
            InitializeComponent();

            MariaSeries = new ColumnSeries {Title = "Maria", Values = new ChartValues<double> {2, 4, 1, 6}};
            CharlesSeries = new ColumnSeries {Title = "Charles", Values = new ChartValues<double> {5, 3, 7, 8}};
            JohnSeries = new ColumnSeries {Title = "John", Values = new ChartValues<double> {8, 3, 1, 7}};

            cartesianChart1.Series = new SeriesCollection
            {
                MariaSeries,
                CharlesSeries,
                JohnSeries
            };

        }

        public ColumnSeries MariaSeries { get; set; }
        public ColumnSeries CharlesSeries { get; set; }
        public ColumnSeries JohnSeries { get; set; }

        private void MariaCheckedChanged(object sender, EventArgs e)
        {
            MariaSeries.Visibility = MariaSeries.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void JohnCheckedChanged(object sender, EventArgs e)
        {
            JohnSeries.Visibility = JohnSeries.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void CharlesCheckedChanged(object sender, EventArgs e)
        {
            CharlesSeries.Visibility = CharlesSeries.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}
