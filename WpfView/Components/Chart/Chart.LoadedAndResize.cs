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
using System.Windows;
using System.Windows.Threading;

namespace LiveCharts.Wpf.Components.Chart
{
    public abstract partial class Chart
    {
        private DispatcherTimer ResizeTimer { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            IsControlLoaded = true;

            Model.ChartControlSize = new CoreSize(ActualWidth, ActualHeight);

            Model.DrawMargin.Left = 0;
            Model.DrawMargin.Top = 0;
            Model.DrawMargin.Width = ActualWidth;
            Model.DrawMargin.Height = ActualHeight;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            ResizeTimer.Stop();
            ResizeTimer.Start();
        }

        private void OnResizeTimerOnTick(object sender, EventArgs args)
        {
#if DEBUG
            Debug.WriteLine("ChartResized");
#endif
            Model.ChartControlSize = new CoreSize(ActualWidth, ActualHeight);

            Model.DrawMargin.Left = 0;
            Model.DrawMargin.Top = 0;
            Model.DrawMargin.Width = ActualWidth;
            Model.DrawMargin.Height = ActualHeight;

            Model.Updater.Run();
            ResizeTimer.Stop();
        }
    }
}
