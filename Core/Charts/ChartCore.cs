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
                var xi = AxisX[index];

                xi.TopLimit = double.IsNaN(xi.MaxValue) ?
                              View.ActualSeries
                                  .Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.GetTracker(series).XLimit.Max)
                                  .DefaultIfEmpty(0).Max() : xi.MaxValue;
                xi.BotLimit = double.IsNaN(xi.MinValue) ?
                              View.ActualSeries
                                  .Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.GetTracker(series).XLimit.Min)
                                  .DefaultIfEmpty(0).Min() : xi.MinValue;
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];

                yi.TopLimit = double.IsNaN(yi.MaxValue) ?
                              View.ActualSeries
                                  .Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.GetTracker(series).YLimit.Max)
                                  .DefaultIfEmpty(0).Max() : yi.MaxValue;
                yi.BotLimit = double.IsNaN(yi.MinValue) ?
                              View.ActualSeries
                                  .Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.GetTracker(series).YLimit.Min)
                                  .DefaultIfEmpty(0).Min() : yi.MinValue;
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
                var yi = AxisY[index];
                var titleSize = yi.View.UpdateTitle(this, -90d);
                var biggest = yi.PrepareChart(AxisOrientation.Y, this);

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

                var uw = yi.EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(AxisOrientation.Y, this, index)/2 : 0;

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
                var yi = AxisY[index];
                yi.UpdateSeparators(AxisOrientation.Y, this, index);
                yi.View.SetTitleTop(curSize.Top + curSize.Height*.5 + yi.View.GetLabelSize().Width*.5);
            }

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
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

            pivot = new CorePoint(
                ChartFunctions.FromPlotArea(pivot.X, AxisOrientation.X, this),
                ChartFunctions.FromPlotArea(pivot.Y, AxisOrientation.Y, this));

            var speed = View.ZoomingSpeed < 0.1 ? 0.1 : (View.ZoomingSpeed > 0.95 ? 0.95 : View.ZoomingSpeed);

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in AxisX)
                {
                    var max = double.IsNaN(xi.MaxValue) ? xi.TopLimit : xi.MaxValue;
                    var min = double.IsNaN(xi.MinValue) ? xi.BotLimit : xi.MinValue;
                    var l = max - min;

                    var rMin = (pivot.X - min) / l;
                    var rMax = 1 - rMin;
                    
                    var target = l*speed;
                    if (target < xi.View.MinRange) return;
                    var mint = pivot.X - target*rMin;
                    var maxt = pivot.X + target*rMax; 
                    xi.View.SetRange(mint, maxt);
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in AxisY)
                {
                    var max = double.IsNaN(yi.MaxValue) ? yi.TopLimit : yi.MaxValue;
                    var min = double.IsNaN(yi.MinValue) ? yi.BotLimit : yi.MinValue;
                    var l = max - min;
                    var rMin = (pivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * speed;
                    if (target < yi.View.MinRange) return;
                    var mint = pivot.Y - target * rMin;
                    var maxt = pivot.Y + target * rMax;
                    yi.View.SetRange(mint, maxt);
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

            pivot = new CorePoint(
                ChartFunctions.FromPlotArea(pivot.X, AxisOrientation.X, this),
                ChartFunctions.FromPlotArea(pivot.Y, AxisOrientation.Y, this));

            var speed = View.ZoomingSpeed < 0.1 ? 0.1 : (View.ZoomingSpeed > 0.95 ? 0.95 : View.ZoomingSpeed);

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in AxisX)
                {
                    var max = double.IsNaN(xi.MaxValue) ? xi.TopLimit : xi.MaxValue;
                    var min = double.IsNaN(xi.MinValue) ? xi.BotLimit : xi.MinValue;
                    var l = max - min;
                    var rMin = (pivot.X - min) / l;
                    var rMax = 1 - rMin;

                    var target = l*(1/speed);
                    if (target > xi.View.MaxRange) return;
                    var mint = pivot.X - target * rMin;
                    var maxt = pivot.X + target * rMax;
                    xi.View.SetRange(mint, maxt);
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in AxisY)
                {
                    var max = double.IsNaN(yi.MaxValue) ? yi.TopLimit : yi.MaxValue;
                    var min = double.IsNaN(yi.MinValue) ? yi.BotLimit : yi.MinValue;
                    var l = max - min;
                    var rMin = (pivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    var target = l * (1 / speed);
                    if (target > yi.View.MaxRange) return;
                    var mint = pivot.Y - target * rMin;
                    var maxt = pivot.Y + target * rMax;
                    yi.View.SetRange(mint, maxt);
                }
            }
        }

        /// <summary>
        /// Clears the zoom.
        /// </summary>
        public void ClearZoom()
        {
            foreach (var xi in AxisX) xi.View.SetRange(double.NaN, double.NaN);
            foreach (var yi in AxisY) yi.View.SetRange(double.NaN, double.NaN);
        }

        /// <summary>
        /// Drags the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public void Drag(CorePoint delta)
        {
            if (View.Zoom == ZoomingOptions.None) return;

            if (View.Zoom == ZoomingOptions.X || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var xi in AxisX)
                {
                    xi.View.SetRange((double.IsNaN(xi.MinValue) ? xi.BotLimit : xi.MinValue) + delta.X,
                        (double.IsNaN(xi.MaxValue) ? xi.TopLimit : xi.MaxValue) + delta.X);
                }
            }

            if (View.Zoom == ZoomingOptions.Y || View.Zoom == ZoomingOptions.Xy)
            {
                foreach (var yi in AxisY)
                {
                    yi.View.SetRange((double.IsNaN(yi.MinValue) ? yi.BotLimit : yi.MinValue) + delta.Y,
                        (double.IsNaN(yi.MaxValue) ? yi.TopLimit : yi.MaxValue) + delta.Y);
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
            var stackedColumns = stackables.SelectMany(x => x.ActualValues.GetPoints(x))
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
                if (mode == StackMode.Percentage)
                {
                    AxisX[stackIndex].BotLimit = 0;
                    AxisX[stackIndex].TopLimit = 1;
                }
                else
                {
                    if (mostLeft < AxisX[stackIndex].BotLimit)
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        AxisX[stackIndex].BotLimit = mostLeft == 0
                            ? 0
                            : ((int) (mostLeft/AxisX[stackIndex].S) - 1)*AxisX[stackIndex].S;
                    if (mostRight > AxisX[stackIndex].TopLimit)
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        AxisX[stackIndex].TopLimit = mostRight == 0
                            ? 0
                            : ((int) (mostRight/AxisX[stackIndex].S) + 1)*AxisX[stackIndex].S;
                }
            }

            if (stackAt == AxisOrientation.Y)
            {
                if (mode == StackMode.Percentage)
                {
                    AxisY[stackIndex].BotLimit = 0;
                    AxisY[stackIndex].TopLimit = 1;
                }
                else
                {
                    if (mostLeft < AxisY[stackIndex].BotLimit)
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        AxisY[stackIndex].BotLimit = mostLeft == 0
                            ? 0
                            : ((int) (mostLeft/AxisY[stackIndex].S) - 1)*AxisY[stackIndex].S;
                    if (mostRight > AxisY[stackIndex].TopLimit)
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        AxisY[stackIndex].TopLimit = mostRight == 0
                            ? 0
                            : ((int) (mostRight/AxisY[stackIndex].S) + 1)*AxisY[stackIndex].S;
                }
            }
        }
        #endregion
    }
}