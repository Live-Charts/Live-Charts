using LiveCharts;
using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.SharedTooltip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SharedTooltipExample : Page
    {
        public SharedTooltipExample()
        {
            InitializeComponent();

            LabelPoint = x => "A long label so it overflows " + x;

            DataContext = this;
        }

        public Func<ChartPoint, string> LabelPoint { get; set; }

        public IChartValues ChartValues { get; set; } = new ChartValues<int>(new int[] { 1, 2, 3, 4, 5 });
    }
}
