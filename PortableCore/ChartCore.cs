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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace LiveChartsCore
{
    public abstract class ChartCore
    {
        protected ChartCore(IChartView view)
        {
            View = view;

            TooltipHidder = new LvcTimer
            {
                Interval = view.TooltipTimeout
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
        
        public List<AxisCore> AxisX { get; set; }
        public List<AxisCore> AxisY { get; set; }

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
                IntializeSeries(series.Model, force);

            Series.CollectionChanged -= OnSeriesCollectionChanged;
            Series.CollectionChanged += OnSeriesCollectionChanged;

            Updater.Run();
        }

        public void IntializeSeries(SeriesCore series, bool force = false)
        {
            //This should not be necessary anymore.
            //View.InitializeSeries(series.View);
            //var observable = series.Values as INotifyCollectionChanged;
            //if (observable == null) return;
            //observable.CollectionChanged -= OnValuesCollectionChanged;
            //observable.CollectionChanged += OnValuesCollectionChanged;
        }

        public virtual void PrepareAxes()
        {
            var ax = AxisX as IList;
            var ay = AxisY as IList;

            if (ax == null || ay == null) return;

            InitializeComponents();

            for (var index = 0; index < ax.Count; index++)
            {
                var view = ax[index] as IAxisView;
                if (view == null) continue;
                var xi = view.Model;

                xi.MaxLimit = xi.MaxValue ??
                              Series.Where(series => series.Model.Values != null && series.Model.ScalesXAt == index)
                                  .Select(series => series.Model.Values.MaxChartPoint.X).DefaultIfEmpty(0).Max();
                xi.MinLimit = xi.MinValue ??
                              Series.Where(series => series.Model.Values != null && series.Model.ScalesXAt == index)
                                  .Select(series => series.Model.Values.MinChartPoint.X).DefaultIfEmpty(0).Min();
            }

            for (var index = 0; index < ay.Count; index++)
            {
                var view = ay[index] as IAxisView;
                if (view == null) continue;
                var yi = view.Model;

                yi.MaxLimit = yi.MaxValue ??
                              Series.Where(series => series.Model.Values != null && series.Model.ScalesYAt == index)
                                  .Select(series => series.Model.Values.MaxChartPoint.Y).DefaultIfEmpty(0).Max();
                yi.MinLimit = yi.MinValue ??
                              Series.Where(series => series.Model.Values != null && series.Model.ScalesYAt == index)
                                  .Select(series => series.Model.Values.MinChartPoint.Y).DefaultIfEmpty(0).Min();
            }

            PivotZoomingAxis = AxisTags.X;
        }

        public void CalculateComponentsAndMargin()
        {
            var curSize = new LvcRectangle(0, 0, ChartControlSize.Width, ChartControlSize.Height);

            curSize = PlaceLegend(curSize);

            double t = curSize.Top,
                b = 0d,
                bm = 0d,
                l = curSize.Left,
                r = 0d;

            foreach (var yi in AxisY)
            {
                var titleSize = yi.View.UpdateTitle(this, -90d);
                var biggest = yi.PrepareChart(AxisTags.Y, this);

                var x = curSize.Left;
                var merged = yi.IsMerged ? 0 : biggest.Width + 2;
                if (yi.Position == AxisPosition.LeftBottom)
                {
                    yi.View.SetTitleLeft(x);
                    yi.View.LabelsReference = x + titleSize.Height + merged;
                    curSize.Left = curSize.Left + titleSize.Height + merged;
                    curSize.Width -= (titleSize.Height + merged);
                }
                else
                {
                    yi.View.SetTitleLeft(x + curSize.Width - titleSize.Height);
                    yi.View.LabelsReference = x + curSize.Width - titleSize.Height - merged;
                    curSize.Width -= (titleSize.Height + merged);
                }

                var top = yi.IsMerged ? 0 : biggest.Height*.5;
                if (t < top) t = top;

                var bot = yi.IsMerged ? 0 : biggest.Height*.5;
                if (b < bot) b = bot;

                if (yi.IsMerged && bm < biggest.Height)
                    bm = biggest.Height;
            }

            if (t > 0)
            {
                curSize.Top = t;
                curSize.Height -= t;
            }
            if (b > 0 && !(AxisX.Count > 0))
                curSize.Height = curSize.Height - b;

            foreach (var xi in AxisX)
            {
                var titleSize = xi.View.UpdateTitle(this);
                var biggest = xi.PrepareChart(AxisTags.X, this);
                var top = curSize.Top;
                var merged = xi.IsMerged ? 0 : biggest.Height;
                if (xi.Position == AxisPosition.LeftBottom)
                {
                    xi.View.SetTitleTop(top + curSize.Height - titleSize.Height);
                    xi.View.LabelsReference = top + b - (xi.IsMerged ? bm : 0) +
                                         (curSize.Height - (titleSize.Height + merged + b)) -
                                         (xi.IsMerged ? b : 0);
                    curSize.Height -= (titleSize.Height + merged + b);
                }
                else
                {
                    xi.View.SetTitleTop(top - t);
                    xi.View.LabelsReference = (top - t) + titleSize.Height + (xi.IsMerged ? bm : 0);
                    curSize.Top = curSize.Top + titleSize.Height + merged;
                    curSize.Height -= (titleSize.Height + merged);
                }

                var left = xi.IsMerged ? 0 : biggest.Width*.5;
                if (l < left) l = left;

                var right = xi.IsMerged ? 0 : biggest.Width*.5;
                if (r < right) r = right;
            }

            if (curSize.Left < l)
            {
                var cor = l - curSize.Left;
                curSize.Left = l;
                curSize.Width -= cor;
                foreach (var yi in AxisY.Where(x => x.Position == AxisPosition.LeftBottom))
                {
                    yi.View.SetTitleLeft(yi.View.GetTitleLeft() + cor);
                    yi.View.LabelsReference += cor;
                }
            }
            var rp = ChartControlSize.Width - curSize.Left - curSize.Width;
            if (r > rp)
            {
                var cor = r - rp;
                curSize.Width -= cor;
                foreach (var yi in AxisY.Where(x => x.Position == AxisPosition.RightTop))
                {
                    yi.View.SetTitleLeft(yi.View.GetTitleLeft() - cor);
                    yi.View.LabelsReference -= cor;
                }
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                if (HasUnitaryPoints)
                    yi.View.UnitWidth = ChartFunctions.GetUnitWidth(AxisTags.Y, this, index);
                yi.UpdateSeparators(AxisTags.Y, this, index);
                yi.View.SetTitleTop(curSize.Top + curSize.Height*.5 + yi.View.GetLabelSize().Width*.5);
            }

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                if (HasUnitaryPoints)
                    xi.View.UnitWidth = ChartFunctions.GetUnitWidth(AxisTags.X, this, index);
                xi.UpdateSeparators(AxisTags.X, this, index);
                xi.View.SetTitleLeft(curSize.Left + curSize.Width*.5 - xi.View.GetLabelSize().Width*.5);
            }

            DrawMargin.Top = curSize.Top;
            DrawMargin.Left = curSize.Left;
            DrawMargin.Width = curSize.Width;
            DrawMargin.Height = curSize.Height;
        }

        public LvcRectangle PlaceLegend(LvcRectangle drawMargin)
        {
            var legendSize = View.LoadLegend();

            const int padding = 10;

            switch (View.LegendLocation)
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
            Updater.Run(restartAnimations);
        }

        public void ZoomIn(LvcPoint pivot)
        {
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

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in AxisX)
                {
                    var max = xi.MaxValue ?? xi.MaxLimit;
                    var min = xi.MinValue ?? xi.MinLimit;
                    var l = max - min;
                    var rMin = (pivot.X - min) / l;
                    var rMax = 1 - rMin;

                    xi.MinValue = min + rMin * xi.S;
                    xi.MaxValue = max - rMax * xi.S;
                }
            }
            else
            {
                foreach (var xi in AxisX)
                {
                    xi.MinValue = null;
                    xi.MaxValue = null;
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in AxisY)
                {
                    var max = yi.MaxValue ?? yi.MaxLimit;
                    var min = yi.MinValue ?? yi.MinLimit;
                    var l = max - min;
                    var rMin = (pivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    yi.MinValue = min + rMin * yi.S;
                    yi.MaxValue = max - rMax * yi.S;
                }
            }
            else
            {
                foreach (var yi in AxisY)
                {
                    yi.MinValue = null;
                    yi.MaxValue = null;
                }
            }

            Updater.Run();
        }

        public void ZoomOut(LvcPoint pivot)
        {
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

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in AxisX)
                {
                    var max = xi.MaxValue ?? xi.MaxLimit;
                    var min = xi.MinValue ?? xi.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.X - min) / l;
                    var rMax = 1 - rMin;

                    xi.MinValue = min - rMin * xi.S;
                    xi.MaxValue = max + rMax * xi.S;
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in AxisY)
                {
                    var max = yi.MaxValue ?? yi.MaxLimit;
                    var min = yi.MinValue ?? yi.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    yi.MinValue = min - rMin * yi.S;
                    yi.MaxValue = max + rMax * yi.S;
                }
            }

            Updater.Run();
        }

        public void ClearZoom()
        {
            foreach (var xi in AxisX)
            {
                xi.MinValue = null;
                xi.MaxValue = null;
            }

            foreach (var yi in AxisY)
            {
                yi.MinValue = null;
                yi.MaxValue = null;
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
                foreach (var series in args.OldItems.Cast<SeriesCore>())
                    series.View.Erase();

            if (args.NewItems != null)
                foreach (var series in args.NewItems.Cast<SeriesCore>())
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