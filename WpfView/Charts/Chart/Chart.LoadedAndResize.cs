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

using System.Diagnostics;
using System.Windows;

namespace LiveCharts.Wpf.Charts.Chart
{
    public abstract partial class Chart
    {
        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            IsControlLoaded = true;

            Model.ChartControlSize = new CoreSize(ActualWidth, ActualHeight);

            //forcing the update twice make the chart scale better initially
            //ToDo: scale nicely the first time, with only one call.
            //this happens because of labels size, canvas size, and many other things that are not
            //rendered yet!

            Model.Updater.Run(false, true);
            Model.Updater.Run(false, true);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
#if DEBUG
            Debug.WriteLine("ChartResized");
#endif
            Model.ChartControlSize = new CoreSize(ActualWidth, ActualHeight);

            Model.Updater.Run();
        }
    }
}
