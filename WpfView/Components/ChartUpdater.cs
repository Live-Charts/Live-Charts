//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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

//Todo: this should be in the Core, the only reason why this is here
//is becuase there is no a multiplatform timer class
//so we need to build our own.
//that works in .net 4.0 to newest.

//the alternative now, is using the timer of each platform
//not a big problem because this class is really easy and small.

namespace LiveCharts.Wpf.Components
{
    public class ChartUpdater : LiveCharts.ChartUpdater
    {
        public ChartUpdater(TimeSpan frequency)
        {
            Timer = new DispatcherTimer {Interval = frequency};

            Timer.Tick += (sender, args) =>
            {
                UpdaterTick();
            };
        }

        public DispatcherTimer Timer { get; set; }

        public override void Run(bool restartView = false, bool updateNow = false)
        {
#if DEBUG
            Debug.WriteLine("Updater run requested...");
#endif
            if (updateNow)
            {
#if DEBUG
                Debug.WriteLine("Updater was forced.");
#endif
                UpdaterTick();
                return;
            }

            if (IsUpdating) return;

            IsUpdating = true;
            Timer.Start();
        }

        public override void UpdateFrequency(TimeSpan freq)
        {
            Timer.Interval = freq;
        }

        private void UpdaterTick()
        {
            Timer.Stop();
            Update();
            IsUpdating = false;

#if DEBUG
            Debug.WriteLine("Chart is updated");
#endif
        }
    }
}
