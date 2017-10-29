//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;

namespace LiveCharts.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ChartCore
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="updater">The updater.</param>
        protected ChartCore(IChartView view, ChartUpdater updater)
        {
            View = view;
            Updater = updater;
            DrawMargin = new CoreRectangle();
            DrawMargin.SetHeight += view.SetDrawMarginHeight;
            DrawMargin.SetWidth += view.SetDrawMarginWidth;
            DrawMargin.SetTop += view.SetDrawMarginTop;
            DrawMargin.SetLeft += view.SetDrawMarginLeft;
        }

        /// <summary>
        /// Initializes the <see cref="ChartCore"/> class.
        /// </summary>
        static ChartCore()
        {
            Configurations = new Charting();
        }

        #endregion

        #region Properties 

        /// <summary>
        /// Gets or sets the configurations.
        /// </summary>
        /// <value>
        /// The configurations.
        /// </value>
        public static Charting Configurations { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [series initialized].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [series initialized]; otherwise, <c>false</c>.
        /// </value>
        public bool SeriesInitialized { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [are components loaded].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [are components loaded]; otherwise, <c>false</c>.
        /// </value>
        public bool AreComponentsLoaded { get; set; }
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public IChartView View { get; set; }
        /// <summary>
        /// Gets or sets the updater.
        /// </summary>
        /// <value>
        /// The updater.
        /// </value>
        public ChartUpdater Updater { get; set; }
        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        /// <value>
        /// The size of the control.
        /// </value>
        public CoreSize ControlSize { get; set; }
        /// <summary>
        /// Gets or sets the draw margin.
        /// </summary>
        /// <value>
        /// The draw margin.
        /// </value>
        public CoreRectangle DrawMargin { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has unitary points.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has unitary points; otherwise, <c>false</c>.
        /// </value>
        public bool HasUnitaryPoints { get; set; }
        /// <summary>
        /// Gets a value indicating whether [requires hover shape].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [requires hover shape]; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresHoverShape
        {
            get
            {
                return View != null &&
                       (View.HasTooltip || View.HasDataClickEventAttached || View.Hoverable);
            }
        }

        /// <summary>
        /// Gets or sets the axis x.
        /// </summary>
        /// <value>
        /// The axis x.
        /// </value>
        public List<AxisCore> AxisX { get; set; }
        /// <summary>
        /// Gets or sets the axis y.
        /// </summary>
        /// <value>
        /// The axis y.
        /// </value>
        public List<AxisCore> AxisY { get; set; }

        /// <summary>
        /// Gets or sets the x limit.
        /// </summary>
        /// <value>
        /// The x limit.
        /// </value>
        public CoreLimit XLimit { get; set; }
        /// <summary>
        /// Gets or sets the y limit.
        /// </summary>
        /// <value>
        /// The y limit.
        /// </value>
        public CoreLimit YLimit { get; set; }
        /// <summary>
        /// Gets or sets the w limit.
        /// </summary>
        /// <value>
        /// The w limit.
        /// </value>
        public CoreLimit WLimit { get; set; }

        /// <summary>
        /// Gets or sets the index of the current color.
        /// </summary>
        /// <value>
        /// The index of the current color.
        /// </value>
        public int CurrentColorIndex { get; set; }

        /// <summary>
        /// Gets or sets the pan origin.
        /// </summary>
        /// <value>
        /// The pan origin.
        /// </value>
        public CorePoint PanOrigin { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Prepares the axes.
        /// </summary>
        public virtual void PrepareAxes()
        {
            for (var index = 0; index < AxisX.Count; index++)
            {
                SetAxisLimits(
                    AxisX[index],
                    View.ActualSeries
                        // ReSharper disable once AccessToModifiedClosure
                        .Where(series => series.Values != null && series.ScalesXAt == index).ToArray(),
                    AxisOrientation.X);
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                SetAxisLimits(
                    AxisY[index],
                    View.ActualSeries
                        // ReSharper disable once AccessToModifiedClosure
                        .Where(series => series.Values != null && series.ScalesYAt == index).ToArray(),
                    AxisOrientation.Y);
            }
        }

        /// <summary>
        /// Runs the specialized chart components.
        /// </summary>
        public virtual void RunSpecializedChartComponents()
        {
            
        }

        /// <summary>
        /// Calculates the components and margin.
        /// </summary>
        public void CalculateComponentsAndMargin()
        {
            var curSize = new CoreRectangle(0, 0, ControlSize.Width, ControlSize.Height);

            curSize = PlaceLegend(curSize);

            const double padding = 4;

            for (int index = 0; index < AxisY.Count; index++)
            {
                var ax = AxisY[index];
                var titleSize = ax.View.UpdateTitle(this, -90d);
                var biggest = ax.PrepareChart(AxisOrientation.Y, this);

                var x = curSize.Left;

                if (ax.Position == AxisPosition.LeftBottom)
                {
                    ax.View.SetTitleLeft(x);
                    curSize.Left += titleSize.Height + biggest.Width + padding;
                    curSize.Width -= (titleSize.Height + biggest.Width + padding);
                    ax.Tab = curSize.Left;
                }
                else
                {
                    ax.View.SetTitleLeft(x + curSize.Width - titleSize.Height);
                    curSize.Width -= (titleSize.Height + biggest.Width + padding);
                    ax.Tab = curSize.Left + curSize.Width;
                }

                var uw = ax.EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(AxisOrientation.Y, this, index)/2 : 0;

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
                var biggest = xi.PrepareChart(AxisOrientation.X, this);
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
                var uw = xi.EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(AxisOrientation.X, this, index)/2 : 0;

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

            DrawMargin.Top = curSize.Top;
            DrawMargin.Left = curSize.Left;
            DrawMargin.Width = curSize.Width;
            DrawMargin.Height = curSize.Height;

            for (var index = 0; index < AxisY.Count; index++)
            {
                var ax = AxisY[index];
                var pr = ChartFunctions.FromPlotArea(ax.MaxPointRadius, AxisOrientation.Y, this, index) -
                         ChartFunctions.FromPlotArea(0, AxisOrientation.Y, this, index);
                if (!double.IsNaN(pr))
                {
                    ax.BotLimit += pr;
                    ax.TopLimit -= pr;
                }
                ax.UpdateSeparators(AxisOrientation.Y, this, index);
                ax.View.SetTitleTop(curSize.Top + curSize.Height*.5 + ax.View.GetLabelSize().Width*.5);
            }

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                var pr = ChartFunctions.FromPlotArea(xi.MaxPointRadius, AxisOrientation.X, this, index) -
                         ChartFunctions.FromPlotArea(0, AxisOrientation.X, this, index);
                if (!double.IsNaN(pr))
                {
                    xi.BotLimit -= pr;
                    xi.TopLimit += pr;
                }
                xi.UpdateSeparators(AxisOrientation.X, this, index);
                xi.View.SetTitleLeft(curSize.Left + curSize.Width*.5 - xi.View.GetLabelSize().Width*.5);
            }
        }

        /// <summary>
        /// Places the legend.
        /// </summary>
        /// <param name="drawMargin">The draw margin.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Zooms the in.
        /// </summary>
        /// <param name="pivot">The pivot.</param>
        public void ZoomIn(CorePoint pivot)
        {
            if (AxisX == null || AxisY == null) return;

            View.HideTooltip();

            var speed = View.ZoomingSpeed < 0.1 ? 0.1 : (View.ZoomingSpeed > 0.95 ? 0.95 : View.ZoomingSpeed);

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                for (var index = 0; index < AxisX.Count; index++)
                {
                    var xi = AxisX[index];

                    var px = ChartFunctions.FromPlotArea(pivot.X, AxisOrientation.X, this, index);

                    var max = double.IsNaN(xi.View.MaxValue) ? xi.TopLimit : xi.View.MaxValue;
                    var min = double.IsNaN(xi.View.MinValue) ? xi.BotLimit : xi.View.MinValue;
                    var l = max - min;

                    var rMin = (px - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * speed;
                    if (target < xi.View.MinRange) return;
                    var mint = px - target * rMin;
                    var maxt = px + target * rMax;
                    xi.View.SetRange(mint, maxt);
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                for (var index = 0; index < AxisY.Count; index++)
                {
                    var ax = AxisY[index];

                    var py = ChartFunctions.FromPlotArea(pivot.Y, AxisOrientation.Y, this, index);

                    var max = double.IsNaN(ax.View.MaxValue) ? ax.TopLimit : ax.View.MaxValue;
                    var min = double.IsNaN(ax.View.MinValue) ? ax.BotLimit : ax.View.MinValue;
                    var l = max - min;
                    var rMin = (py - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * speed;
                    if (target < ax.View.MinRange) return;
                    var mint = py - target * rMin;
                    var maxt = py + target * rMax;
                    ax.View.SetRange(mint, maxt);
                }
            }
        }

        /// <summary>
        /// Zooms the out.
        /// </summary>
        /// <param name="pivot">The pivot.</param>
        public void ZoomOut(CorePoint pivot)
        {
            View.HideTooltip();

            var speed = View.ZoomingSpeed < 0.1 ? 0.1 : (View.ZoomingSpeed > 0.95 ? 0.95 : View.ZoomingSpeed);

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                for (var index = 0; index < AxisX.Count; index++)
                {
                    var xi = AxisX[index];

                    var px = ChartFunctions.FromPlotArea(pivot.X, AxisOrientation.X, this, index);

                    var max = double.IsNaN(xi.View.MaxValue) ? xi.TopLimit : xi.View.MaxValue;
                    var min = double.IsNaN(xi.View.MinValue) ? xi.BotLimit : xi.View.MinValue;
                    var l = max - min;
                    var rMin = (px - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * (1 / speed);
                    if (target > xi.View.MaxRange) return;
                    var mint = px- target * rMin;
                    var maxt = px + target * rMax;
                    xi.View.SetRange(mint, maxt);
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                for (var index = 0; index < AxisY.Count; index++)
                {
                    var ax = AxisY[index];

                    var py = ChartFunctions.FromPlotArea(pivot.Y, AxisOrientation.Y, this, index);
                    
                    var max = double.IsNaN(ax.View.MaxValue) ? ax.TopLimit : ax.View.MaxValue;
                    var min = double.IsNaN(ax.View.MinValue) ? ax.BotLimit : ax.View.MinValue;
                    var l = max - min;
                    var rMin = (py - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * (1 / speed);
                    if (target > ax.View.MaxRange) return;
                    var mint = py - target * rMin;
                    var maxt = py + target * rMax;
                    ax.View.SetRange(mint, maxt);
                }
            }
        }

        /// <summary>
        /// Clears the zoom.
        /// </summary>
        public void ClearZoom()
        {
            foreach (var xi in AxisX) xi.View.SetRange(double.NaN, double.NaN);
            foreach (var ax in AxisY) ax.View.SetRange(double.NaN, double.NaN);
        }

        /// <summary>
        /// Drags the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public void Drag(CorePoint delta)
        {
            if (View.Pan == PanningOptions.Unset && View.Zoom == ZoomingOptions.None ||
                View.Pan == PanningOptions.None) return;

            var px = View.Pan == PanningOptions.Unset &&
                     (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy);
            px = px || View.Pan == PanningOptions.X || View.Pan == PanningOptions.Xy;

            if (px)
            {
                for (var index = 0; index < AxisX.Count; index++)
                {
                    var xi = AxisX[index];
                    var dx = ChartFunctions.FromPlotArea(delta.X, AxisOrientation.X, this, index) -
                             ChartFunctions.FromPlotArea(0, AxisOrientation.X, this, index);

                    xi.View.SetRange((double.IsNaN(xi.View.MinValue) ? xi.BotLimit : xi.View.MinValue) + dx,
                        (double.IsNaN(xi.View.MaxValue) ? xi.TopLimit : xi.View.MaxValue) + dx);
                }
            }

            var py = View.Pan == PanningOptions.Unset &&
                     (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy);
            py = py || View.Pan == PanningOptions.Y || View.Pan == PanningOptions.Xy;
            if (py)
            {
                for (var index = 0; index < AxisY.Count; index++)
                {
                    var ax = AxisY[index];
                    var dy = ChartFunctions.FromPlotArea(delta.Y, AxisOrientation.Y, this, index) -
                             ChartFunctions.FromPlotArea(0, AxisOrientation.Y, this, index);

                    ax.View.SetRange((double.IsNaN(ax.View.MinValue) ? ax.BotLimit : ax.View.MinValue) + dy,
                        (double.IsNaN(ax.View.MaxValue) ? ax.TopLimit : ax.View.MaxValue) + dy);
                }
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// Stacks the points.
        /// </summary>
        /// <param name="stackables">The stackables.</param>
        /// <param name="stackAt">The stack at.</param>
        /// <param name="stackIndex">Index of the stack.</param>
        /// <param name="mode">The mode.</param>
        protected void StackPoints(IEnumerable<ISeriesView> stackables, AxisOrientation stackAt, int stackIndex,
            StackMode mode = StackMode.Values)
        {
            var groupedStackables = stackables.GroupBy(s => s is IGroupedStackedSeriesView ? (s as IGroupedStackedSeriesView).Grouping : 0).ToList();

            foreach (var groupedStack in groupedStackables)
            {
                var stackedColumns = groupedStack.SelectMany(x => x.ActualValues.GetPoints(x))
                .GroupBy(x => stackAt == AxisOrientation.X ? x.Y : x.X);

                double mostLeft = 0, mostRight = 0;

                foreach (var column in stackedColumns)
                {
                    double sumLeft = 0, sumRight = 0;

                    foreach (var item in column)
                    {
                        var s = stackAt == AxisOrientation.X ? item.X : item.Y;
                        if (s < 0)
                            sumLeft += s;
                        else
                            sumRight += s;
                    }

                    var lastLeft = 0d;
                    var lastRight = 0d;
                    var leftPart = 0d;
                    var rightPart = 0d;

                    foreach (var point in column)
                    {
                        var pulled = stackAt == AxisOrientation.X ? point.X : point.Y;

                        //notice using (pulled < 0) or (pulled <= 0) could cause an issue similar to
                        //https://github.com/beto-rodriguez/Live-Charts/issues/231
                        //from that issue I changed <= to <
                        //only because it is more common to use positive values than negative
                        //you could face a similar issue if you are stacking only negative values
                        //a work around is forcing (pulled < 0) to be true,
                        //instead of using zero values, use -0.000000001/

                        if (pulled < 0)
                        {
                            point.From = lastLeft;
                            point.To = lastLeft + pulled;
                            point.Sum = sumLeft;
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
                            point.Sum = sumRight;
                            point.Participation = (point.To - point.From) / point.Sum;
                            point.Participation = double.IsNaN(point.Participation)
                                ? 0
                                : point.Participation;
                            rightPart += point.Participation;
                            point.StackedParticipation = rightPart;

                            lastRight = point.To;
                        }
                    }

                    if (sumLeft < mostLeft) mostLeft = sumLeft;
                    if (sumRight > mostRight) mostRight = sumRight;
                }

                if (stackAt == AxisOrientation.X)
                {
                    var ax = AxisX[stackIndex];

                    if (mode == StackMode.Percentage)
                    {
                        if (double.IsNaN(ax.MinValue)) ax.BotLimit = 0;
                        if (double.IsNaN(ax.MaxValue)) ax.TopLimit = 1;
                    }
                    else
                    {
                        if (mostLeft < ax.BotLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            if (double.IsNaN(ax.MinValue))
                                ax.BotLimit = mostLeft == 0.0
                                    ? 0.0
                                    : Math.Floor(mostLeft/ax.S)*ax.S;
                        if (mostRight > ax.TopLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            if (double.IsNaN(ax.MaxValue))
                                ax.TopLimit = mostRight == 0.0
                                    ? 0.0
                                    : (Math.Floor(mostRight/ax.S) + 1.0) *ax.S;
                    }
                }

                if (stackAt == AxisOrientation.Y)
                {
                    var ay = AxisY[stackIndex];

                    if (mode == StackMode.Percentage)
                    {
                        if (double.IsNaN(ay.MinValue)) ay.BotLimit = 0;
                        if (double.IsNaN(ay.MaxValue)) ay.TopLimit = 1;
                    }
                    else
                    {
                        if (mostLeft < ay.BotLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            if (double.IsNaN(ay.MinValue))
                                ay.BotLimit = mostLeft == 0.0
                                    ? 0.0
                                    : Math.Floor(mostLeft/ay.S)*ay.S;
                        if (mostRight > ay.TopLimit)
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            if (double.IsNaN(ay.MaxValue))
                                ay.TopLimit = mostRight == 0.0
                                    ? 0.0
                                    : (Math.Floor(mostRight/ay.S) + 1.0) *ay.S;
                    }
                }
            }
        }
        #endregion

        #region Privates
        private static void SetAxisLimits(AxisCore ax, IList<ISeriesView> series, AxisOrientation orientation)
        {
            var first = new CoreLimit();
            var firstR = 0d;

            if (series.Count > 0)
            {
                first = orientation == AxisOrientation.X
                    ? series[0].Values.GetTracker(series[0]).XLimit
                    : series[0].Values.GetTracker(series[0]).YLimit;
                var view = series[0] as IAreaPoint;
                firstR = view != null ? view.GetPointDiameter() : 0;
            }
            
            //                     [ max, min, pointRadius ]
            var boundries = new[] {first.Max, first.Min, firstR};

            for (var index = 1; index < series.Count; index++)
            {
                var seriesView = series[index];
                var tracker = seriesView.Values.GetTracker(seriesView);
                var limit = orientation == AxisOrientation.X ? tracker.XLimit : tracker.YLimit;
                var view = seriesView as IAreaPoint;
                var radius = view != null ? view.GetPointDiameter() : 0;

                if (limit.Max > boundries[0]) boundries[0] = limit.Max;
                if (limit.Min < boundries[1]) boundries[1] = limit.Min;
                if (radius > boundries[2]) boundries[2] = radius;
            }

            ax.TopSeriesLimit = boundries[0];
            ax.BotSeriesLimit = boundries[1];

            ax.TopLimit = double.IsNaN(ax.MaxValue) ? boundries[0] : ax.MaxValue;
            ax.BotLimit = double.IsNaN(ax.MinValue) ? boundries[1] : ax.MinValue;

            ax.MaxPointRadius = boundries[2];
        }
        #endregion
    }
}