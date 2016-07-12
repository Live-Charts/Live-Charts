using System;
using System.Windows.Controls;
using LiveCharts;

namespace Wpf.PieChart
{
    public partial class PieChartExample : UserControl
    {
        public PieChartExample()
        {
            InitializeComponent();

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }
    }
}
