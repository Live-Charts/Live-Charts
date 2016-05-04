using System;
using System.Windows.Threading;

namespace LiveCharts.Wpf
{
    public class ChartUpdater : LiveCharts.ChartUpdater
    {
        public ChartUpdater()
        {
            Timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(50)};
            FrecuencyUpdate += () =>
            {
                Timer.Interval = Frequency;
            };
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
    }
}
