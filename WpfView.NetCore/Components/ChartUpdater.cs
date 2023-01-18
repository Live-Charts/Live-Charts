//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Diagnostics;
using System.Windows.Threading;
using LiveCharts.Dtos;
using LiveCharts.Wpf.Charts.Base;

namespace LiveCharts.Wpf.Components
{
    internal class ChartUpdater : LiveCharts.ChartUpdater
    {
        public ChartUpdater(TimeSpan frequency)
        {
            Timer = new DispatcherTimer {Interval = frequency};

            Timer.Tick += OnTimerOnTick;
            Freq = frequency;
        }

        public DispatcherTimer Timer { get; set; }
        private bool RequiresRestart { get; set; }
        private TimeSpan Freq { get; set; }

        public override void Run(bool restartView = false, bool updateNow = false)
        {
            if (Timer == null)
            {
                Timer = new DispatcherTimer {Interval = Freq};
                Timer.Tick += OnTimerOnTick;
                IsUpdating = false;
            }

            if (updateNow)
            {
                UpdaterTick(restartView, true);
                return;
            }

            RequiresRestart = restartView || RequiresRestart;
            if (IsUpdating) return;

            IsUpdating = true;
            Timer.Start();
        }

        public override void UpdateFrequency(TimeSpan freq)
        {
            Timer.Interval = freq;
        }

        public void OnTimerOnTick(object sender, EventArgs args)
        {
            UpdaterTick(RequiresRestart, false);
        }

        private void UpdaterTick(bool restartView, bool force)
        {
            var wpfChart = (Chart) Chart.View;
            
            if (!force && !wpfChart.IsVisible && !wpfChart.IsMocked) return;

            Chart.ControlSize = wpfChart.IsMocked
                ? wpfChart.Model.ControlSize
                : new CoreSize(wpfChart.ActualWidth, wpfChart.ActualHeight);

            Timer.Stop();
            Update(restartView, force);
            IsUpdating = false;

            RequiresRestart = false;
            
            wpfChart.ChartUpdated();
            wpfChart.PrepareScrolBar();
        }
    }
}
