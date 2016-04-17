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
using System.Collections.Specialized;
using System.Linq;

namespace LiveChartsCore
{
    public class Chart : IChartModel
    {
        public Chart(IChartView view)
        {
            View = view;

            TooltipTimeoutTimer = new Timer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            ResizeTimer = new Timer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            ResizeTimer.Tick += () => Update(false);
        }

        public bool SeriesInitialized { get; set; }
        public IChartView View { get; set; }
        public Size ChartSize { get; set; }
        public Size DrawMargin { get; set; }
        public SeriesCollection Series { get; set; }
        public bool HasUnitaryPoints { get; set; }
        public bool Invert { get; set; }
        public AxisTags PivotZoomingAxis { get; set; }
        public Timer TooltipTimeoutTimer { get; set; }
        public Timer ResizeTimer { get; set; }

        public Point PanOrigin { get; set; }

        public bool IsDragging { get; set; }
        public bool IsZooming { get; set; }

        public void IntializeSeries(bool force = false)
        {
            if (SeriesInitialized & !force) return;

            SeriesInitialized = true;

            foreach (var series in Series)
                IntializeSeries(series, force);

            Series.CollectionChanged -= OnSeriesCollectionChanged;
            Series.CollectionChanged += OnSeriesCollectionChanged;

            Update();
        }

        public void IntializeSeries(ISeriesModel series, bool force = false)
        {
            View.InitializeSeries(series.View);
            var observable = series.Values as INotifyCollectionChanged;
            if (observable == null) return;
            observable.CollectionChanged -= OnValuesCollectionChanged;
            observable.CollectionChanged += OnValuesCollectionChanged;
        }

        public void AddVisual(object visual)
        {
            throw new NotImplementedException();
        }

        public void Update(bool restartAnimations = true)
        {
            foreach (var point in Series
                .SelectMany(series => series.Values.Points))
                point.UpdateView();
        }

        public void ZoomIn(Point pivot)
        {
            throw new NotImplementedException();
        }

        public void ZoomOut(Point pivot)
        {
            throw new NotImplementedException();
        }



        private void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
                View.Erase();

            if (args.OldItems != null)
                foreach (var series in args.OldItems.Cast<ISeriesModel>())
                    series.View.Erase();

            if (args.NewItems != null)
                foreach (var series in args.NewItems.Cast<ISeriesModel>())
                    IntializeSeries(series, true);

            Update();
        }

        private void OnValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Update();
        }
    }
}