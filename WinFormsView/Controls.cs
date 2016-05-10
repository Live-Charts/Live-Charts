using System.Windows.Forms.Integration;

namespace WinFormsView
{
    public class CartesianChart : ElementHost
    {
        public CartesianChart()
        {
            Child = new LiveCharts.Wpf.CartesianChart();
        }
    }
}
