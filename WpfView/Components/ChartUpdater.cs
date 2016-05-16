using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace LiveCharts.Wpf.Components
{
    public class ChartUpdater : LiveCharts.ChartUpdater
    {
        public ChartUpdater(TimeSpan frequency)
        {
            Timer = new DispatcherTimer {Interval = frequency};

            Timer.Tick += (sender, args) =>
            {
                Update();
                IsUpdating = false;
                Timer.Stop();
            };
        }

        public DispatcherTimer Timer { get; set; }

        public override void Run(bool restartView = false)
        {
            if (IsUpdating) return;

            IsUpdating = true;
            Timer.Start();
        }

        public override void UpdateFrequency(TimeSpan freq)
        {
            Timer.Interval = freq;
        }
    }
}
