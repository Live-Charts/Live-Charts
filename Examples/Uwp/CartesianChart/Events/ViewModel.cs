using System.Diagnostics;
using LiveCharts;
using LiveCharts.Events;

namespace UWP.CartesianChart.Events
{
    public class ViewModel
    {
        public ViewModel()
        {
            DataClickCommand = new MyCommand<ChartPoint>
            {
                ExecuteDelegate = p => Debug.WriteLine(
                    "[COMMAND] you clicked " + p.X + ", " + p.Y)
            };
            DataHoverCommand = new MyCommand<ChartPoint>
            {
                ExecuteDelegate = p => Debug.WriteLine(
                    "[COMMAND] you hovered over " + p.X + ", " + p.Y)
            };
            UpdaterTickCommand = new MyCommand<LiveCharts.Uwp.CartesianChart>
            {
                ExecuteDelegate = c => Debug.WriteLine("[COMMAND] Chart was updated!")
            };
            RangeChangedCommand = new MyCommand<RangeChangedEventArgs>
            {
                ExecuteDelegate = e => Debug.WriteLine("[COMMAND] axis range changed")
            };
        }

        public MyCommand<ChartPoint> DataHoverCommand { get; set; }
        public MyCommand<ChartPoint> DataClickCommand { get; set; }
        public MyCommand<LiveCharts.Uwp.CartesianChart> UpdaterTickCommand { get; set; }
        public MyCommand<RangeChangedEventArgs> RangeChangedCommand { get; set; }
    }
}