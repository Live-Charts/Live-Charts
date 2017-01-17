using System;
using LiveCharts;
using LiveCharts.Events;

namespace Wpf.CartesianChart.Events
{
    public class ViewModel
    {
        public ViewModel()
        {
            DataClickCommand = new MyCommand<ChartPoint>
            {
                ExecuteDelegate = p => Console.WriteLine(
                    "[COMMAND] you clicked " + p.X + ", " + p.Y)
            };
            DataHoverCommand = new MyCommand<ChartPoint>
            {
                ExecuteDelegate = p => Console.WriteLine(
                    "[COMMAND] you hovered over " + p.X + ", " + p.Y)
            };
            UpdaterTickCommand = new MyCommand<LiveCharts.Wpf.CartesianChart>
            {
                ExecuteDelegate = c => Console.WriteLine("[COMMAND] Chart was updated!")
            };
            RangeChangedCommand = new MyCommand<RangeChangedEventArgs>
            {
                ExecuteDelegate = e => Console.WriteLine("[COMMAND] Axis range changed")
            };
        }

        public MyCommand<ChartPoint> DataHoverCommand { get; set; }
        public MyCommand<ChartPoint> DataClickCommand { get; set; }
        public MyCommand<LiveCharts.Wpf.CartesianChart> UpdaterTickCommand { get; set; }
        public MyCommand<RangeChangedEventArgs> RangeChangedCommand { get; set; }
    }
}