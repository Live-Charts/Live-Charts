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
using System.Globalization;
using System.Linq;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    public class AxisCore
    {
        private double _topLimit;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public AxisCore(IAxisView view)
        {
            View = view;
            CleanFactor = 3;
            Cache = new Dictionary<double, SeparatorElementCore>();
            LastAxisMax = null;
            LastAxisMin = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the chart.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public ChartCore Chart { get; set; }
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public IAxisView View { get; set; }
        /// <summary>
        /// Gets or sets the labels.
        /// </summary>
        /// <value>
        /// The labels.
        /// </value>
        public IList<string> Labels { get; set; }
        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public List<AxisSectionCore> Sections { get; set; }
        /// <summary>
        /// Gets or sets the label formatter.
        /// </summary>
        /// <value>
        /// The label formatter.
        /// </value>
        public Func<double, string> LabelFormatter { get; set; }
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show labels].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show labels]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLabels { get; set; }
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public double MaxValue { get; set; }
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public double MinValue { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [disable animations].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable animations]; otherwise, <c>false</c>.
        /// </value>
        public bool DisableAnimations { get; set; }
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public AxisPosition Position { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is merged.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is merged; otherwise, <c>false</c>.
        /// </value>
        public bool IsMerged { get; set; }
        /// <summary>
        /// Gets a value indicating whether [evaluates unit width].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [evaluates unit width]; otherwise, <c>false</c>.
        /// </value>
        public bool EvaluatesUnitWidth { get; internal set; }
        /// <summary>
        /// Gets the current separators.
        /// </summary>
        /// <value>
        /// The current separators.
        /// </value>
        public IEnumerable<SeparatorElementCore> CurrentSeparators
        {
            get { return Cache.Values; }
        }
        /// <summary>
        /// Gets or sets the separator.
        /// </summary>
        /// <value>
        /// The separator.
        /// </value>
        public SeparatorConfigurationCore Separator { get; set; }
        /// <summary>
        /// Gets the s.
        /// </summary>
        /// <value>
        /// The s.
        /// </value>
        public double S { get; internal set; }

        #endregion

        #region Internal Properties

        internal double Tab { get; set; }

        internal double TopLimit
        {
            get { return _topLimit; }
            set
            {
                _topLimit = value;
            }
        }

        internal double BotLimit { get; set; }
        internal double TopSeriesLimit { get; set; }
        internal double BotSeriesLimit { get; set; }
        internal double MaxPointRadius { get; set; }
        internal double Magnitude { get; set; }
        internal double CleanFactor { get; set; }
        internal Dictionary<double, SeparatorElementCore> Cache { get; set; }
        internal double? LastAxisMax { get; set; }
        internal double? LastAxisMin { get; set; }
        internal CoreRectangle LastPlotArea { get; set; }
        internal int GarbageCollectorIndex { get; set; }
        internal double PreviousTop { get; set; }
        internal double PreviousBot { get; set; }
        protected internal double FirstSeparator { get; protected set; }
        protected internal double LastSeparator { get; protected set; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the formatter.
        /// </summary>
        /// <returns></returns>
        public virtual Func<double, string> GetFormatter()
        {
            return Formatter;
        }

        /// <summary>
        /// Clears the separators.
        /// </summary>
        public void ClearSeparators()
        {
            foreach (var separator in Cache)
            {
                separator.Value.View.Clear(Chart.View);
            }
            Cache = new Dictionary<double, SeparatorElementCore>();
        }

        #endregion

        #region Internal Methods

        internal virtual void CalculateSeparator(ChartCore chart, AxisOrientation source)
        {
            var range = TopLimit - BotLimit;
            range = range <= 0 ? 1 : range;

            //ToDO: Improve this according to current labels!
            var separations = source == AxisOrientation.Y
                ? Math.Round(chart.ControlSize.Height / ((12) * CleanFactor), 0) // at least 3 font 12 labels per separator.
                : Math.Round(chart.ControlSize.Width / (50 * CleanFactor), 0); // at least 150 pixels per separator.

            separations = separations < 2 ? 2 : separations;

            var minimum = range / separations;
            Magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));

            if (!double.IsNaN(Separator.Step))
            {
                S = Separator.Step;
                return;
            }

            var residual = minimum / Magnitude;
            double tick;
            if (residual > 5)
                tick = 10 * Magnitude;
            else if (residual > 2)
                tick = 5 * Magnitude;
            else if (residual > 1)
                tick = 2 * Magnitude;
            else
                tick = Magnitude;

            S = tick;

            if (Labels != null) S = S < 1 ? 1 : S;
        }

        internal virtual CoreMargin PrepareChart(AxisOrientation source, ChartCore chart)
        {
            if (!(Math.Abs(TopLimit - BotLimit) > S * .01) || !ShowLabels) return new CoreMargin();

            CalculateSeparator(chart, source);

            var f = GetFormatter();

            var currentMargin = new CoreMargin();
            var tolerance = S / 10;

            InitializeGarbageCollector();

            var m = !double.IsNaN(View.BarUnit)
                ? View.BarUnit
                : (!double.IsNaN(View.Unit)
                    ? View.Unit
                    : Magnitude);

            var u = !double.IsNaN(View.BarUnit)
                ? View.BarUnit
                : (!double.IsNaN(View.Unit)
                    ? View.Unit
                    : 1);

            if (TopLimit <= 0 && BotLimit < 0)
            {
                var l = TopLimit - (EvaluatesUnitWidth ? u : 0);
                LastSeparator = l;
                for (var i = l; i >= Math.Truncate(BotLimit / m) * m; i -= S)
                {
                    FirstSeparator = i;
                    DrawSeparator(i, tolerance, currentMargin, f, source);
                }
            }
            else
            {
                var l = Math.Truncate(BotLimit / m) * m;
                FirstSeparator = l;
                for (var i = l; i <= TopLimit - (EvaluatesUnitWidth ? u : 0); i += S)
                {
                     LastSeparator = i;
                    DrawSeparator(i, tolerance, currentMargin, f, source);
                }
            }

            return currentMargin;
        }

        internal void UpdateSeparators(AxisOrientation source, ChartCore chart, int axisIndex)
        {
            foreach (var element in Cache.Values.ToArray())
            {
                if (element.GarbageCollectorIndex < GarbageCollectorIndex)
                {
                    element.State = SeparationState.Remove;
                    Cache.Remove(element.Key);
                }

                var toLine = ChartFunctions.ToPlotArea(element.Value, source, chart, axisIndex);

                var direction = source == AxisOrientation.X ? 1 : -1;

                toLine += EvaluatesUnitWidth ? direction * ChartFunctions.GetUnitWidth(source, chart, this) / 2 : 0;
                var toLabel = toLine + element.View.LabelModel.GetOffsetBySource(source);

                if (IsMerged)
                {
                    const double padding = 4;

                    if (source == AxisOrientation.Y)
                    {
                        if (toLabel + element.View.LabelModel.ActualHeight >
                            chart.DrawMargin.Top + chart.DrawMargin.Height)
                            toLabel -= element.View.LabelModel.ActualHeight + padding;
                    }
                    else
                    {
                        if (toLabel + element.View.LabelModel.ActualWidth >
                            chart.DrawMargin.Left + chart.DrawMargin.Width)
                            toLabel -= element.View.LabelModel.ActualWidth + padding;
                    }
                }

                var labelTab = Tab;
                labelTab += element.View.LabelModel.GetOffsetBySource(source.Invert());

                switch (element.State)
                {
                    case SeparationState.Remove:
                        if (!chart.View.DisableAnimations && !View.DisableAnimations)
                        {
                            element.View.Move(chart, this, source, axisIndex, toLabel, toLine, labelTab);
                            element.View.FadeOutAndRemove(chart);
                        }
                        else
                        {
                            element.View.Remove(chart);
                        }
                        break;
                    case SeparationState.Keep:
                        if (!chart.View.DisableAnimations && !View.DisableAnimations)
                        {
                            if (element.IsNew)
                            {
                                var toLinePrevious = FromPreviousState(element.Value, source, chart);
                                toLinePrevious += EvaluatesUnitWidth ? ChartFunctions.GetUnitWidth(source, chart, this) / 2 : 0;
                                var toLabelPrevious = toLinePrevious + element.View.LabelModel.GetOffsetBySource(source);
                                element.View.Place(chart, this, source, axisIndex, toLabelPrevious,
                                    toLinePrevious, labelTab);
                                element.View.FadeIn(this, chart);
                            }
                            element.View.Move(chart, this, source, axisIndex, toLabel, toLine, labelTab);
                        }
                        else
                        {
                            element.View.Place(chart, this, source, axisIndex, toLabel, toLine, labelTab);
                        }
                        break;
                    case SeparationState.InitialAdd:
                        element.View.Place(chart, this, source, axisIndex, toLabel, toLine, labelTab);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            LastAxisMax = TopLimit;
            LastAxisMin = BotLimit;
            LastPlotArea = new CoreRectangle(chart.DrawMargin.Left, chart.DrawMargin.Top,
                chart.DrawMargin.Width, chart.DrawMargin.Height);
        }

        internal double FromPreviousState(double value, AxisOrientation source, ChartCore chart)
        {
            if (LastAxisMax == null) return 0;

            var p1 = new CorePoint();
            var p2 = new CorePoint();

            if (source == AxisOrientation.Y)
            {
                p1.X = LastAxisMax ?? 0;
                p1.Y = LastPlotArea.Top;

                p2.X = LastAxisMin ?? 0;
                p2.Y = LastPlotArea.Top + LastPlotArea.Height;
            }
            else
            {
                p1.X = LastAxisMax ?? 0;
                p1.Y = LastPlotArea.Width + LastPlotArea.Left;

                p2.X = LastAxisMin ?? 0;
                p2.Y = LastPlotArea.Left;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            var d = m * (value - p1.X) + p1.Y;

            return d;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Initializes the garbage collector.
        /// </summary>
        protected void InitializeGarbageCollector()
        {
            if (GarbageCollectorIndex == int.MaxValue)
            {
                foreach (var value in Cache.Values)
                {
                    value.GarbageCollectorIndex = 0;
                }
                GarbageCollectorIndex = 0;
            }

            GarbageCollectorIndex++;
        }
        #endregion

        #region Private Methods

        private void DrawSeparator(double i, double tolerance, CoreMargin currentMargin, Func<double, string> f, AxisOrientation source)
        {
            if (i < BotLimit) return;
           
            SeparatorElementCore asc;

            var key = Math.Round(i / tolerance) * tolerance;
            if (!Cache.TryGetValue(key, out asc))
            {
                asc = new SeparatorElementCore { IsNew = true };
                Cache[key] = asc;
            }
            else
            {
                asc.IsNew = false;
            }

            View.RenderSeparator(asc, Chart);

            asc.Key = key;
            asc.Value = i;
            asc.GarbageCollectorIndex = GarbageCollectorIndex;

            var labelsMargin = asc.View.UpdateLabel(f(i), this, source);

            currentMargin.Width = labelsMargin.TakenWidth > currentMargin.Width
                ? labelsMargin.TakenWidth
                : currentMargin.Width;
            currentMargin.Height = labelsMargin.TakenHeight > currentMargin.Height
                ? labelsMargin.TakenHeight
                : currentMargin.Height;

            currentMargin.Left = labelsMargin.Left > currentMargin.Left
                ? labelsMargin.Left
                : currentMargin.Left;
            currentMargin.Right = labelsMargin.Right > currentMargin.Right
                ? labelsMargin.Right
                : currentMargin.Right;

            currentMargin.Top = labelsMargin.Top > currentMargin.Top
                ? labelsMargin.Top
                : currentMargin.Top;
            currentMargin.Bottom = labelsMargin.Bottom > currentMargin.Bottom
                ? labelsMargin.Bottom
                : currentMargin.Bottom;

            if (LastAxisMax == null)
            {
                asc.State = SeparationState.InitialAdd;
                return;
            }

            asc.State = SeparationState.Keep;
        }

        private string Formatter(double x)
        {
            if (Labels == null)
            {
                return LabelFormatter == null
                    ? x.ToString(CultureInfo.InvariantCulture)
                    : LabelFormatter(x);
            }

            if (x < 0) x *= -1;
            return Labels.Count > x && x >= 0
                ? Labels[checked((int)x)]
                : "";
        }

        #endregion
    }
}