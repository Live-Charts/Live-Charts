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
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;

namespace LiveCharts
{
    public class ChartUpdater : IChartUpdater
    {
        public ChartCore Chart { get; set; }
        public bool IsUpdating { get; set; }
        public bool RestartViewRequested { get; set; }

        private void InitializeSeriesView(ISeriesView seriesView)
        {
            Chart.View.EnsureElementBelongsToCurrentView(seriesView);
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
            if (!Chart.View.IsMocked)
                if (!Chart.View.IsControlLoaded) return;

            if (restartsAnimations)
                Chart.View.ActualSeries.ForEach(s =>
                {
                    if (s.ActualValues == null) return;
                    s.ActualValues.GetPoints(s).ForEach(p =>
                    {
                        p.View.RemoveFromView(Chart);
                        p.View = null;
                    });
                });

            Chart.AxisX = Chart.View.MapXAxes(Chart);
            Chart.AxisY = Chart.View.MapYAxes(Chart);

            foreach (var series in Chart.View.ActualSeries)
            {
                InitializeSeriesView(series);
                series.ActualValues.GetLimits(series);
                series.InitializeColors();
                series.DrawSpecializedElements();
            }

            Chart.PrepareAxes();
            Chart.RunSpecializedChartComponents();

            foreach (var series in Chart.View.ActualSeries)
            {
                Debug.WriteLine("view: " + series.Values.Trackers[series].GetHashCode());

                series.OnSeriesUpdateStart();
                series.ActualValues.InitializeGarbageCollector(series);
                series.Model.Update();
                series.ActualValues.CollectGarbage(series);
                series.OnSeriesUpdatedFinish();
                series.PlaceSpecializedElements();
            }
#if DEBUG
            Debug.WriteLine("<< -- Chart UI Updated | Canvas: {0} | DrawMargin: {1} -- >>",
                Chart.View.GetCanvasElements(), Chart.View.GetDrawMarginElements());
#endif
        }
       
    }
}