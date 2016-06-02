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
using LiveCharts.Charts;
using LiveCharts.Helpers;

namespace LiveCharts
{
    public class ChartUpdater : IChartUpdater
    {
        public ChartCore Chart { get; set; }
        public bool IsUpdating { get; set; }
        public bool RestartViewRequested { get; set; }

        private void InitializeSeriesParams(ISeriesView seriesView)
        {
            if (!seriesView.IsInVisualTree)
                Chart.View.AddToView(seriesView);

            seriesView.Model.Chart = Chart;
            seriesView.Values.Series = seriesView.Model;
            seriesView.Model.SeriesCollection = Chart.View.Series;
            seriesView.Model.SeriesCollection.Chart = Chart;
        }

        public virtual void Run(bool restartView = false, bool updateNow = false)   
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateFrequency(TimeSpan freq)
        {
            throw new NotImplementedException();
        }

        protected void Update(bool restartsAnimations = false)   
        {
            if (!Chart.View.IsControlLoaded || Chart.View.Series == null) return;

            if (restartsAnimations)
                Chart.View.Series.ForEach(s => s.Values.Points.ForEach(p =>
                {
                    p.View.RemoveFromView(Chart);
                    p.View = null;
                }));

            Chart.AxisX = Chart.View.MapXAxes(Chart);
            Chart.AxisY = Chart.View.MapYAxes(Chart);

            foreach (var series in Chart.View.Series)
            {
                InitializeSeriesParams(series);
                series.Values.GetLimits();
            }

            Chart.PrepareAxes();

            foreach (var series in Chart.View.Series)
            {
                series.OnSeriesUpdateStart();
                series.Values.InitializeGarbageCollector();
                series.Model.Update();
                series.Values.CollectGarbage();
                series.OnSeriesUpdatedFinish();
            }
#if DEBUG
            Debug.WriteLine("<<Chart UI Updated>>");
            Chart.View.CountElements();
#endif
        }
       
    }
}