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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace LiveChartsCore
{
    public abstract class Chart : IChartModel
    {
        protected Chart(IChartView view)
        {
            View = view;

            TooltipTimeout = TimeSpan.FromMilliseconds(800);
            TooltipHidder = new LvcTimer
            {
                Interval = TooltipTimeout
            };
            TooltipHidder.Tick += () =>
            {
                View.HideTooltop();
            };

            Updater = Motor.GetUpdater(this);

            DrawMargin = new LvcRectangle();
            DrawMargin.SetHeight += view.SetDrawMarginHeight;
            DrawMargin.SetWidth += view.SetDrawMarginWidth;
            DrawMargin.SetTop += view.SetDrawMarginTop;
            DrawMargin.SetLeft += view.SetDrawMarginLeft;
        }

        public bool SeriesInitialized { get; set; }
        public IChartView View { get; set; }
        public IChartUpdater Updater { get; set; }
        public LvcSize ChartControlSize { get; set; }
        public LvcRectangle DrawMargin { get; set; }
        public SeriesCollection Series { get; set; }
        public bool HasUnitaryPoints { get; set; }
        public bool Invert { get; set; }
        public object AxisX { get; set; }
        public object AxisY { get; set; }
        public TimeSpan TooltipTimeout { get; set; }
        public ZoomingOptions Zoom { get; set; }
        public LegendLocation LegendLocation { get; set; }
        public bool DisableAnimatons { get; set; }
        public TimeSpan AnimationsSpeed { get; set; }

        public AxisTags PivotZoomingAxis { get; set; }
        public LvcTimer TooltipHidder { get; set; }

        public LvcPoint PanOrigin { get; set; }

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

            Updater.Run();
        }

        public void IntializeSeries(ISeriesModel series, bool force = false)
        {
            View.InitializeSeries(series.View);
            var observable = series.Values as INotifyCollectionChanged;
            if (observable == null) return;
            observable.CollectionChanged -= OnValuesCollectionChanged;
            observable.CollectionChanged += OnValuesCollectionChanged;
        }

        public virtual void PrepareAxes()
        {
            var ax = AxisX as List<IAxisView>;
            var ay = AxisY as List<IAxisView>;

            if (ax == null || ay == null) return;

            InitializeComponents();

            for (var index = 0; index < ax.Count; index++)
            {
                var xi = ax[index].Model;
                xi.MaxLimit = xi.MaxValue ??
                              Series.Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.MaxChartPoint.X).DefaultIfEmpty(0).Max();
                xi.MinLimit = xi.MinValue ??
                              Series.Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.MinChartPoint.X).DefaultIfEmpty(0).Min();
            }

            for (var index = 0; index < ay.Count; index++)
            {
                var yi = ay[index].Model;
                yi.MaxLimit = yi.MaxValue ??
                              Series.Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.MaxChartPoint.Y).DefaultIfEmpty(0).Max();
                yi.MinLimit = yi.MinValue ??
                              Series.Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.MinChartPoint.Y).DefaultIfEmpty(0).Min();
            }

            PivotZoomingAxis = Invert ? AxisTags.Y : AxisTags.X;
        }

        public void CalculateComponentsAndMargin()
        {
            var ax = AxisX as List<IAxisView>;
            var ay = AxisY as List<IAxisView>;

            if (ax == null || ay == null) return;

            var curSize = new LvcRectangle(0, 0, ChartControlSize.Width, ChartControlSize.Width);

            curSize = PlaceLegend(curSize);

            double t = curSize.Top,
                b = 0d,
                bm = 0d,
                l = curSize.Left,
                r = 0d;

            foreach (var yi in ay)
            {
                var titleSize = yi.UpdateTitle(-90d);
                var biggest = yi.Model.PrepareChart(AxisTags.Y, this);

                var x = curSize.Left;
                var merged = yi.Model.IsMerged ? 0 : biggest.Width + 2;
                if (yi.Model.Position == AxisPosition.LeftBottom)
                {
                    yi.SetTitleLeft(x);
                    yi.LabelsReference = x + titleSize.Height + merged;
                    curSize.Left = curSize.Left + titleSize.Height + merged;
                    curSize.Width -= (titleSize.Height + merged);
                }
                else
                {
                    yi.SetTitleLeft(x + curSize.Width - titleSize.Height);
                    yi.LabelsReference = x + curSize.Width - titleSize.Height - merged;
                    curSize.Width -= (titleSize.Height + merged);
                }

                var top = yi.Model.IsMerged ? 0 : biggest.Height*.5;
                if (t < top) t = top;

                var bot = yi.Model.IsMerged ? 0 : biggest.Height*.5;
                if (b < bot) b = bot;

                if (yi.Model.IsMerged && bm < biggest.Height)
                    bm = biggest.Height;
            }

            if (t > 0)
            {
                curSize.Top = t;
                curSize.Height -= t;
            }
            if (b > 0 && !ax.Any())
                curSize.Height = curSize.Height - b;

            foreach (var xi in ax)
            {
                var titleSize = xi.UpdateTitle();
                var biggest = xi.Model.PrepareChart(AxisTags.X, this);
                var top = curSize.Top;
                var merged = xi.Model.IsMerged ? 0 : biggest.Height;
                if (xi.Model.Position == AxisPosition.LeftBottom)
                {
                    xi.SetTitleTop(top + curSize.Height - titleSize.Height);
                    xi.LabelsReference = top + b - (xi.Model.IsMerged ? bm : 0) +
                                         (curSize.Height - (titleSize.Height + merged + b)) -
                                         (xi.Model.IsMerged ? b : 0);
                    curSize.Height -= (titleSize.Height + merged + b);
                }
                else
                {
                    xi.SetTitleTop(top - t);
                    xi.LabelsReference = (top - t) + titleSize.Height + (xi.Model.IsMerged ? bm : 0);
                    curSize.Top = curSize.Top + titleSize.Height + merged;
                    curSize.Height -= (titleSize.Height + merged);
                }

                var left = xi.Model.IsMerged ? 0 : biggest.Width*.5;
                if (l < left) l = left;

                var right = xi.Model.IsMerged ? 0 : biggest.Width*.5;
                if (r < right) r = right;
            }

            if (curSize.Left < l)
            {
                var cor = l - curSize.Left;
                curSize.Left = l;
                curSize.Width -= cor;
                foreach (var yi in ay.Where(x => x.Model.Position == AxisPosition.LeftBottom))
                {
                    yi.SetTitleLeft(yi.GetTitleLeft() + cor);
                    yi.LabelsReference += cor;
                }
            }
            var rp = ChartControlSize.Width - curSize.Left - curSize.Width;
            if (r > rp)
            {
                var cor = r - rp;
                curSize.Width -= cor;
                foreach (var yi in ay.Where(x => x.Model.Position == AxisPosition.RightTop))
                {
                    yi.SetTitleLeft(yi.GetTitleLeft() - cor);
                    yi.LabelsReference -= cor;
                }
            }

            for (var index = 0; index < ay.Count; index++)
            {
                var yi = ay[index];
                if (HasUnitaryPoints && Invert)
                    yi.UnitWidth = ChartFunctions.GetUnitWidth(AxisTags.Y, this, index);
                yi.Model.UpdateSeparators(AxisTags.Y, this, index);
                yi.SetTitleTop(curSize.Top + curSize.Height*.5 + yi.GetLabelSize().Width*.5);
            }

            for (var index = 0; index < ax.Count; index++)
            {
                var xi = ax[index];
                if (HasUnitaryPoints && !Invert)
                    xi.UnitWidth = ChartFunctions.GetUnitWidth(AxisTags.X, this, index);
                xi.Model.UpdateSeparators(AxisTags.X, this, index);
                xi.SetTitleLeft(curSize.Left + curSize.Width*.5 - xi.GetLabelSize().Width*.5);
            }
        }

        public LvcRectangle PlaceLegend(LvcRectangle drawMargin)
        {
            var legendSize = View.LoadLegend();

            const int padding = 10;

            switch (LegendLocation)
            {
                case LegendLocation.None:
                    View.HideLegend();
                    break;
                case LegendLocation.Top:
                    var top = new LvcPoint(ChartControlSize.Width * .5 - legendSize.Width * .5, 0);
                    var y = drawMargin.Top;
                    drawMargin.Top = y + top.Y + legendSize.Height + padding;
                    drawMargin.Height -= legendSize.Height - padding;
                    View.ShowLegend(top);
                    break;
                case LegendLocation.Bottom:
                    var bot = new LvcPoint(ChartControlSize.Width*.5 - legendSize.Width*.5,
                        ChartControlSize.Height - legendSize.Height);
                    drawMargin.Height -= legendSize.Height;
                    View.ShowLegend(new LvcPoint(bot.X, ChartControlSize.Height - legendSize.Height));
                    break;
                case LegendLocation.Left:
                    drawMargin.Left = drawMargin.Left + legendSize.Width;
                    View.ShowLegend(new LvcPoint(0, ChartControlSize.Height*.5 - legendSize.Height*.5));
                    break;
                case LegendLocation.Right:
                    drawMargin.Width -= legendSize.Width + padding;
                    View.ShowLegend(new LvcPoint(ChartControlSize.Width - legendSize.Width,
                        ChartControlSize.Height*.5 - legendSize.Height*.5));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return drawMargin;
        }

        protected virtual void OnDataMouseEnter()
        {
            Debug.WriteLine("Data mouse entered!");
        }

        protected virtual void OnDataMouseLeave()
        {
            Debug.WriteLine("Data Mouse Leaved! bye bye");
        }

        protected virtual void OnDataMouseDown()
        {
            Debug.WriteLine("Data Mouse Down!");
        }

        protected virtual LvcPoint GetTooltipPosition()
        {
            return new LvcPoint();
        }



        public void Update(bool restartAnimations = true)
        {
            foreach (var series in Series)
            {
                series.Update();
            }
        }

        public void ZoomIn(LvcPoint pivot)
        {
            var ax = AxisX as List<IAxisView>;
            var ay = AxisY as List<IAxisView>;

            if (ax == null || ay == null) return;

            View.HideTooltop();

            if (IsZooming) return;

            IsZooming = true;

            var zoomingSpeed = View.GetZoomingSpeed();

            if (zoomingSpeed == null)
            {
                IsZooming = false;
            }
            else
            {
                var t = new LvcTimer { Interval = zoomingSpeed.Value };
                t.Tick += () =>
                {
                    IsZooming = false;
                    t.Stop();
                };
                t.Start();
            }

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in ax)
                {
                    var max = xi.Model.MaxValue ?? xi.Model.MaxLimit;
                    var min = xi.Model.MinValue ?? xi.Model.MinLimit;
                    var l = max - min;
                    var rMin = (pivot.X - min) / l;
                    var rMax = 1 - rMin;

                    xi.Model.MinValue = min + rMin * xi.Model.S;
                    xi.Model.MaxValue = max - rMax * xi.Model.S;
                }
            }
            else
            {
                foreach (var xi in ax)
                {
                    xi.Model.MinValue = null;
                    xi.Model.MaxValue = null;
                }
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in ay)
                {
                    var max = yi.Model.MaxValue ?? yi.Model.MaxLimit;
                    var min = yi.Model.MinValue ?? yi.Model.MinLimit;
                    var l = max - min;
                    var rMin = (pivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    yi.Model.MinValue = min + rMin * yi.Model.S;
                    yi.Model.MaxValue = max - rMax * yi.Model.S;
                }
            }
            else
            {
                foreach (var yi in ay)
                {
                    yi.Model.MinValue = null;
                    yi.Model.MaxValue = null;
                }
            }

            Updater.Run();
        }

        public void ZoomOut(LvcPoint pivot)
        {
            var ax = AxisX as List<IAxisView>;
            var ay = AxisY as List<IAxisView>;

            if (ax == null || ay == null) return;

            View.HideTooltop();

            if (IsZooming) return;

            IsZooming = true;

            var zoomingSpeed = View.GetZoomingSpeed();

            if (zoomingSpeed == null)
            {
                IsZooming = false;
            }
            else
            {
                var t = new LvcTimer { Interval = zoomingSpeed.Value };
                t.Tick += () =>
                {
                    IsZooming = false;
                    t.Stop();
                };
                t.Start();
            }

            var dataPivot = new LvcPoint(
                ChartFunctions.FromDrawMargin(pivot.X, AxisTags.X, this),
                ChartFunctions.FromDrawMargin(pivot.Y, AxisTags.Y, this));

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in ax)
                {
                    var max = xi.Model.MaxValue ?? xi.Model.MaxLimit;
                    var min = xi.Model.MinValue ?? xi.Model.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.X - min) / l;
                    var rMax = 1 - rMin;

                    xi.Model.MinValue = min - rMin * xi.Model.S;
                    xi.Model.MaxValue = max + rMax * xi.Model.S;
                }
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in ay)
                {
                    var max = yi.Model.MaxValue ?? yi.Model.MaxLimit;
                    var min = yi.Model.MinValue ?? yi.Model.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    yi.Model.MinValue = min - rMin * yi.Model.S;
                    yi.Model.MaxValue = max + rMax * yi.Model.S;
                }
            }

            Updater.Run();
        }

        public void ClearZoom()
        {
            var ax = AxisX as List<IAxisView>;
            var ay = AxisY as List<IAxisView>;

            if (ax == null || ay == null) return;

            foreach (var xi in ax)
            {
                xi.Model.MinValue = null;
                xi.Model.MaxValue = null;
            }

            foreach (var yi in ay)
            {
                yi.Model.MinValue = null;
                yi.Model.MaxValue = null;
            }

            Updater.Run();
        }

        public void TooltipHideStartCount()
        {
            TooltipHidder.Stop();
            TooltipHidder.Start();
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

            Updater.Run();
        }

        private void OnValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Updater.Run();
        }


        private void InitializeComponents()
        {
            //sets the chart of each component, 
            //adds tooltip to chart
            //Calculates the limit of each chart.
        }

        private void InitializeSeries()
        {
            
        }

        private void PrepareCanvas()
        {
            //intialize series, add elements to visual tree to support bidings...
            //set the plot area...   
        }
    }
}