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

namespace LiveChartsCore
{
    public class ChartUpdater : IChartUpdater
    {
        public ChartUpdater(ChartCore chart)
        {
            Chart = chart;
        }

        public ChartCore Chart { get; private set; }

        public void Run(bool restartsAnimations = false)
        {
            if (!Chart.View.IsControlLoaded || Chart.Series == null) return;

            foreach (var series in Chart.Series)
            {
                InitializeSeriesParams(series);
                series.InitializeView();
                series.Model.Values.GetLimits();
            }

            Chart.PrepareAxes();

            foreach (var series in Chart.Series)
            {
                series.Model.Values.InitializeGarbageCollector();
                series.Model.Update();
                series.CloseView();
                series.Model.Values.CollectGarbage();
            }
#if DEBUG
            Debug.WriteLine("<<Chart UI Updated>>");
#endif
        }

        private void InitializeSeriesParams(ISeriesView seriesView)
        {
            seriesView.Model.Chart = Chart;
            seriesView.Model.Values.Series = seriesView.Model;
            seriesView.Model.SeriesCollection = Chart.Series;
        }

        public void Cancel()
        {
#if DEBUG
            Debug.WriteLine("Chart update cancelation requested!");
#endif
        }
    }
}