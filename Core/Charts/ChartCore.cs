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
using System.Linq;

namespace LiveCharts.Charts
{
    public abstract class ChartCore
    {
        private readonly SeriesCollection _dummyCollection = new SeriesCollection();

        #region Constructors
        protected ChartCore(IChartView view, IChartUpdater updater)
        {
            View = view;
            Updater = updater;
            DrawMargin = new CoreRectangle();
            DrawMargin.SetHeight += view.SetDrawMarginHeight;
            DrawMargin.SetWidth += view.SetDrawMarginWidth;
            DrawMargin.SetTop += view.SetDrawMarginTop;
            DrawMargin.SetLeft += view.SetDrawMarginLeft;
        }

        static ChartCore()
        {
            Configurations = new Charting();
        }

        #endregion

        #region Properties 

        public static Charting Configurations { get; set; }
        public bool SeriesInitialized { get; set; }
        public IChartView View { get; set; }
        public IChartUpdater Updater { get; set; }
        public CoreSize ControlSize { get; set; }
        public CoreRectangle DrawMargin { get; set; }
        public bool HasUnitaryPoints { get; set; }
        public bool RequiresHoverShape
        {
            get { return View != null && (View.HasTooltip || View.HasDataClickEventAttached || View.Hoverable); }
        }

        public List<AxisCore> AxisX { get; set; }
        public List<AxisCore> AxisY { get; set; }

        public CoreLimit Value1CoreLimit { get; set; }
        public CoreLimit Value2CoreLimit { get; set; }
        public CoreLimit Value3CoreLimit { get; set; }

        public int CurrentColorIndex { get; set; }

        public AxisTags PivotZoomingAxis { get; set; }
        public CorePoint PanOrigin { get; set; }

        private bool IsZooming
        {
            get
            {
                var animationsSpeed = View.DisableAnimations ? 0 : View.AnimationsSpeed.TotalMilliseconds;
                return (DateTime.Now - RequestedZoomAt).TotalMilliseconds < animationsSpeed;
            }
        }

        private DateTime RequestedZoomAt { get; set; }

        public IEnumerable<ISeriesView> ActualSeries
        {
            get
            {
                return (View.Series ?? Enumerable.Empty<ISeriesView>())
                    .Where(x => x.IsSeriesVisible);
            }
        }

        #endregion

        #region Public Methods

        public virtual void PrepareAxes()
        {
            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];

                xi.MaxLimit = xi.MaxValue ??
                              (View.Series ?? new SeriesCollection())
                                  .Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.Limit1.Max).DefaultIfEmpty(0).Max();
                xi.MinLimit = xi.MinValue ??
                              (View.Series ?? new SeriesCollection())
                                  .Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.Limit1.Min).DefaultIfEmpty(0).Min();
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];

                yi.MaxLimit = yi.MaxValue ??
                              (View.Series ?? new SeriesCollection())
                                  .Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.Limit2.Max).DefaultIfEmpty(0).Max();
                yi.MinLimit = yi.MinValue ??
                              (View.Series ?? new SeriesCollection())
                                  .Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.Limit2.Min).DefaultIfEmpty(0).Min();
            }
        }

        public virtual void RunSpecializedChartComponents()
        {
            
        }

        public void CalculateComponentsAndMargin()
        {
            var curSize = new CoreRectangle(0, 0, ControlSize.Width, ControlSize.Height);

            curSize = PlaceLegend(curSize);

            const double padding = 4;

            for (int index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                var titleSize = yi.View.UpdateTitle(this, -90d);
                var biggest = yi.PrepareChart(AxisTags.Y, this);

                var x = curSize.Left;

                if (yi.Position == AxisPosition.LeftBottom)
                {
                    yi.View.SetTitleLeft(x);
                    curSize.Left += titleSize.Height + biggest.Width + padding;
                    curSize.Width -= (titleSize.Height + biggest.Width + padding);
                    yi.Tab = curSize.Left;
                }
                else
                {
                    yi.View.SetTitleLeft(x + curSize.Width - titleSize.Height);
                    curSize.Width -= (titleSize.Height + biggest.Width + padding);
                    yi.Tab = curSize.Left + curSize.Width;
                }

                var uw = yi.EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(AxisTags.Y, this, index)/2 : 0;

                var topE = biggest.Top - uw;
                if (topE> curSize.Top)
                {
                    var dif = topE - curSize.Top;
                    curSize.Top += dif;
                    curSize.Height -= dif;
                }

                var botE = biggest.Bottom - uw;
                if (botE > ControlSize.Height - (curSize.Top + curSize.Height))
                {
                    var dif = botE - (ControlSize.Height - (curSize.Top + curSize.Height));
                    curSize.Height -= dif;
                }
            }

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                var titleSize = xi.View.UpdateTitle(this);
                var biggest = xi.PrepareChart(AxisTags.X, this);
                var top = curSize.Top;

                if (xi.Position == AxisPosition.LeftBottom)
                {
                    xi.View.SetTitleTop(top + curSize.Height - titleSize.Height);
                    curSize.Height -= (titleSize.Height + biggest.Height);
                    xi.Tab = curSize.Top + curSize.Height;
                }
                else
                {
                    xi.View.SetTitleTop(top);
                    curSize.Top += titleSize.Height + biggest.Height;
                    curSize.Height -= (titleSize.Height + biggest.Height);
                    xi.Tab = curSize.Top;
                }

                //Notice the unit width is not exact at this point...
                var uw = xi.EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(AxisTags.X, this, index)/2 : 0;

                var leftE = biggest.Left - uw > 0 ? biggest.Left - uw : 0;
                if (leftE > curSize.Left)
                {
                    var dif = leftE - curSize.Left;
                    curSize.Left += dif;
                    curSize.Width -= dif;
                    foreach (var correctedAxis in AxisY
                        .Where(correctedAxis => correctedAxis.Position == AxisPosition.LeftBottom))
                    {
                        correctedAxis.Tab += dif;
                    }
                }

                var rightE = biggest.Right - uw > 0 ? biggest.Right - uw : 0;
                if (rightE > ControlSize.Width - (curSize.Left + curSize.Width))
                {
                    var dif = rightE - (ControlSize.Width - (curSize.Left + curSize.Width));
                    curSize.Width -= dif;
                    foreach (var correctedAxis in AxisY
                        .Where(correctedAxis => correctedAxis.Position == AxisPosition.RightTop))
                    {
                        correctedAxis.Tab -= dif;
                    }
                }
            }


            //if (curSize.Left < l)
            //{
            //    var cor = l - curSize.Left;
            //    curSize.Left = l;
            //    curSize.Width -= cor;
            //    foreach (var yi in AxisY.Where(x => x.Position == AxisPosition.LeftBottom))
            //    {
            //        yi.View.SetTitleLeft(yi.View.GetTitleLeft() + cor);
            //    }
            //}
            //var rp = ChartControlSize.Width - curSize.Left - curSize.Width;
            //if (r > rp)
            //{
            //    var cor = r - rp;
            //    curSize.Width -= cor;
            //    foreach (var yi in AxisY.Where(x => x.Position == AxisPosition.RightTop))
            //    {
            //        yi.View.SetTitleLeft(yi.View.GetTitleLeft() - cor);
            //    }
            //}

            DrawMargin.Top = curSize.Top;
            DrawMargin.Left = curSize.Left;
            DrawMargin.Width = curSize.Width;
            DrawMargin.Height = curSize.Height;

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
        }

        public CoreRectangle PlaceLegend(CoreRectangle drawMargin)
        {
            var legendSize = View.LoadLegend();

            const int padding = 10;

            switch (View.LegendLocation)
            {
                case LegendLocation.None:
                    View.HideLegend();
                    break;
                case LegendLocation.Top:
                    drawMargin.Top += legendSize.Height;
                    drawMargin.Height -= legendSize.Height;
                    View.ShowLegend(new CorePoint(ControlSize.Width * .5 - legendSize.Width * .5, 0));
                    break;
                case LegendLocation.Bottom:
                    var bot = new CorePoint(ControlSize.Width*.5 - legendSize.Width*.5,
                        ControlSize.Height - legendSize.Height);
                    drawMargin.Height -= legendSize.Height;
                    View.ShowLegend(new CorePoint(bot.X, ControlSize.Height - legendSize.Height));
                    break;
                case LegendLocation.Left:
                    drawMargin.Left = drawMargin.Left + legendSize.Width;
                    View.ShowLegend(new CorePoint(0, ControlSize.Height*.5 - legendSize.Height*.5));
                    break;
                case LegendLocation.Right:
                    drawMargin.Width -= legendSize.Width + padding;
                    View.ShowLegend(new CorePoint(ControlSize.Width - legendSize.Width,
                        ControlSize.Height*.5 - legendSize.Height*.5));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return drawMargin;
        }

        public void ZoomIn(CorePoint pivot)
        {
            View.HideTooltop();

            if (IsZooming) return;

            RequestedZoomAt = DateTime.Now;

            pivot = new CorePoint(
                ChartFunctions.FromDrawMargin(pivot.X, AxisTags.X, this),
                ChartFunctions.FromDrawMargin(pivot.Y, AxisTags.Y, this));

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in AxisX)
                {
                    var max = xi.MaxValue ?? xi.MaxLimit;
                    var min = xi.MinValue ?? xi.MinLimit;
                    var l = max - min;
                    var rMin = (pivot.X - min) / l;
                    var rMax = 1 - rMin;

                    xi.View.MinValue = min + rMin * xi.S;
                    xi.View.MaxValue = max - rMax * xi.S;
                }
            }
            else
            {
                foreach (var xi in AxisX)
                {
                    xi.View.MinValue = null;
                    xi.View.MaxValue = null;
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

                    yi.View.MinValue = min + rMin * yi.S;
                    yi.View.MaxValue = max - rMax * yi.S;
                }
            }
            else
            {
                foreach (var yi in AxisY)
                {
                    yi.View.MinValue = null;
                    yi.View.MaxValue = null;
                }
            }

            Updater.Run(false, true);
        }

        public void ZoomOut(CorePoint pivot)
        {
            View.HideTooltop();

            if (IsZooming) return;

            RequestedZoomAt = DateTime.Now;

            var dataPivot = new CorePoint(
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

                    xi.View.MinValue = min - rMin * xi.S;
                    xi.View.MaxValue = max + rMax * xi.S;
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

                    yi.View.MinValue = min - rMin * yi.S;
                    yi.View.MaxValue = max + rMax * yi.S;
                }
            }

            Updater.Run(false, true);
        }

        public void ClearZoom()
        {
            foreach (var xi in AxisX)
            {
                xi.View.MinValue = null;
                xi.View.MaxValue = null;
            }

            foreach (var yi in AxisY)
            {
                yi.View.MinValue = null;
                yi.View.MaxValue = null;
            }

            Updater.Run();
        }

        public void Drag(CorePoint delta)
        {
            if (PivotZoomingAxis == AxisTags.None) return;

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in AxisX)
                {
                    xi.View.MaxValue = (xi.MaxValue ?? xi.MaxLimit) + delta.X;
                    xi.View.MinValue = (xi.MinValue ?? xi.MinLimit) + delta.X;
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in AxisY)
                {
                    yi.View.MaxValue = (yi.MaxValue ?? yi.MaxLimit) + delta.Y;
                    yi.View.MinValue = (yi.MinValue ?? yi.MinLimit) + delta.Y;
                }
            }
            
            Updater.Run(false, true);
        }

        #endregion

        #region Protected

        protected void StackPoints(IEnumerable<ISeriesView> stackables, AxisTags stackAt, int stackIndex,
            StackMode mode = StackMode.Values)
        {
            var stackedColumns = stackables.Select(x => x.ActualValues.Points.ToArray()).ToArray();

            var maxI = stackedColumns.Select(x => x.Length).DefaultIfEmpty(0).Max();

            for (var i = 0; i < maxI; i++)
            {
                var cols = stackedColumns
                    .Select(x => x.Length > i
                        ? new StackedSum(Pull(x[i], stackAt))
                        : new StackedSum()).ToArray();

                var sum = new StackedSum
                {
                    Left = cols.Select(x => x.Left).DefaultIfEmpty(0).Sum(),
                    Right = cols.Select(x => x.Right).DefaultIfEmpty(0).Sum()
                };

                if (stackAt == AxisTags.X)
                {
                    if (mode == StackMode.Percentage)
                    {
                        AxisX[stackIndex].MinLimit = 0;
                        AxisX[stackIndex].MaxLimit = 1;
                    }
                    else
                    {
                        if (sum.Left < AxisX[stackIndex].MinLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            AxisX[stackIndex].MinLimit = sum.Left == 0
                                ? 0
                                : ((int)(sum.Left / AxisX[stackIndex].S) - 1) * AxisX[stackIndex].S;
                        if (sum.Right > AxisX[stackIndex].MaxLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            AxisX[stackIndex].MaxLimit = sum.Right == 0
                                ? 0
                                : ((int)(sum.Right / AxisX[stackIndex].S) + 1) * AxisX[stackIndex].S;
                    }
                }
                if (stackAt == AxisTags.Y)
                {
                    if (mode == StackMode.Percentage)
                    {
                        AxisY[stackIndex].MinLimit = 0;
                        AxisY[stackIndex].MaxLimit = 1;
                    }
                    else
                    {
                        if (sum.Left < AxisY[stackIndex].MinLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            AxisY[stackIndex].MinLimit = sum.Left == 0
                                ? 0
                                : ((int)(sum.Left / AxisY[stackIndex].S) - 1) * AxisY[stackIndex].S;
                        if (sum.Right > AxisY[stackIndex].MaxLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            AxisY[stackIndex].MaxLimit = sum.Right == 0
                                ? 0
                                : ((int)(sum.Right / AxisY[stackIndex].S) + 1) * AxisY[stackIndex].S;
                    }
                }

                var lastLeft = 0d;
                var lastRight = 0d;
                var leftPart = 0d;
                var rightPart = 0d;

                foreach (var col in stackedColumns)
                {
                    if (i >= col.Length) continue;
                    var point = col[i];
                    var pulled = Pull(point, stackAt);

                    if (pulled <= 0)
                    {
                        point.From = lastLeft;
                        point.To = lastLeft + pulled;
                        point.Sum = sum.Left;
                        point.Participation = (point.To - point.From) / point.Sum;
                        point.Participation = double.IsNaN(point.Participation)
                            ? 0
                            : point.Participation;
                        leftPart += point.Participation;
                        point.StackedParticipation = leftPart;

                        lastLeft = point.To;
                    }
                    else
                    {
                        point.From = lastRight;
                        point.To = lastRight + pulled;
                        point.Sum = sum.Right;
                        point.Participation = (point.To - point.From)/point.Sum;
                        point.Participation = double.IsNaN(point.Participation)
                            ? 0
                            : point.Participation;
                        rightPart += point.Participation;
                        point.StackedParticipation = rightPart;

                        lastRight = point.To;
                    }
                }
            }
        }

        protected static double Pull(ChartPoint point, AxisTags source)
        {
            return source == AxisTags.Y ? point.Y : point.X;
        }

        #endregion
    }
}