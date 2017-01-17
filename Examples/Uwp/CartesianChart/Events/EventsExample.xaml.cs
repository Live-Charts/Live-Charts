using System.Diagnostics;
using Windows.Devices.Input;
using Windows.UI.Xaml.Controls;
using LiveCharts;
using LiveCharts.Events;
using LiveCharts.Uwp;

namespace UWP.CartesianChart.Events
{
    public partial class EventsExample : Page
    {
        public EventsExample()
        {
            InitializeComponent();

            DataContext = new ViewModel();
        }

        public ChartValues<double> Values { get; set; } = new ChartValues<double> {4, 6, 5, 3, 5};

        private void ChartOnDataClick(object sender, ChartPoint p)
        {
            var asPixels = Chart.ConvertToPixels(p.AsPoint());
            Debug.WriteLine("[EVENT] You clicked (" + p.X + ", " + p.Y + ") in pixels (" +
                            asPixels.X + ", " + asPixels.Y + ")");
        }

        private void Chart_OnDataHover(object sender, ChartPoint p)
        {
            Debug.WriteLine("[EVENT] you hovered over " + p.X + ", " + p.Y);
        }

        private void ChartOnUpdaterTick(object sender)
        {
            Debug.WriteLine("[EVENT] chart was updated");
        }

        private void Axis_OnRangeChanged(RangeChangedEventArgs eventargs)
        {
            Debug.WriteLine("[EVENT] axis range changed");
        }
    }
}
