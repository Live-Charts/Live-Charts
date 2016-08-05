using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Components;

namespace Wpf.CartesianChart.UIElements
{
    public partial class UiElementsExample : UserControl
    {
        public UiElementsExample()
        {
            InitializeComponent();
        }

        private void ChartOnDataClick(object sender, ChartPoint chartPoint)
        {
            var asPixels = Chart.ConvertToPixels(chartPoint.AsPoint());
            MessageBox.Show("You clicked (" + chartPoint.X + ", " + chartPoint.Y + ") in pixels (" +
                            asPixels.X + ", " + asPixels.Y + ")");
        }

        private void ChartMouseMove(object sender, MouseEventArgs e)
        {
            var point = Chart.ConvertToChartValues(e.GetPosition(Chart));

            X.Text = point.X.ToString("N");
            Y.Text = point.Y.ToString("N");
        }
    }
}
