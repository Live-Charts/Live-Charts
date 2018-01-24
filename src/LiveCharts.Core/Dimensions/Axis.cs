using System;
using System.Collections.Generic;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a cartesian linear axis.
    /// </summary>
    public class Axis : Plane
    {
        private readonly Dictionary<double, ISeparator> _activeSeparators =
            new Dictionary<double, ISeparator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        public Axis()
            : base(LiveChartsSelectors.DefaultPlane)
        {
            Step = double.NaN;
            Position = AxisPositions.Auto;
        }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        public double Step { get; set; }

        /// <summary>
        /// Gets the actual step.
        /// </summary>
        /// <value>
        /// The actual step.
        /// </value>
        public double ActualStep { get; internal set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public AxisPositions Position { get; set; }

        internal Margin CalculateAxisMargin(ChartModel chart)
        {
            #region note

            // we'll ask the axis to generate a label every 5px to estimate its size
            // this method is not perfect, but it is should have a good performance
            // and also should return a trust-able size of the axis.
            // if the display is not the desired by the user, they shall specify
            // explicitly the ChartView.DrawMargin property.

            // why can't we measure the real axis?
            // The CalculateStep() method is based on the size of the DrawMargin
            // the DrawMargin size is affected by the axis' size, the more space the 
            // axis requires, the smaller the DrawMargin rectangle will be.
            // then we have a circular reference, the labels in our axis will
            // determine the axis size, the labels will be printed every Axis.ActualStep
            // interval, we can't know the labels if we don't have a defined DrawMargin,
            // and we can't have a defined DrawMargin if there are no labels.

            // ToDo:
            // then, if the user defines the Step property, we should be able
            // to calculate exactly the size don't we?
            // ... this is not supported for now.
            #endregion

            var space = chart.DrawAreaSize;
            if (!(PlaneType == PlaneTypes.X || PlaneType == PlaneTypes.Y))
            {
                throw new LiveChartsException(
                    $"An axis of type '{PlaneType}' can not be used in a Cartesian Chart", 130);
            }
            var dimension = PlaneType == PlaneTypes.X ? space.Width : space.Height;
            const int step = 5; // ToDo: or use the real if it is defined?
            int l = 0, r = 0, t = 0, b = 0;
            for (var i = 0; i < dimension; i += step)
            {
                var label = EvaluateAxisLabel(i, space);

                var li = label.Location.X - label.Margin.Left;
                if (li < 0 && l < -li) l = -li;

                var ri = label.Location.X + label.Margin.Right;
                if (ri > space.Width && r < ri - space.Width) r = ri - space.Width;

                var ti = label.Location.Y - label.Margin.Top;
                if (ti < 0 && t < ti) t = -ti;

                var bi = label.Location.Y + label.Margin.Bottom;
                if (bi > space.Height && b < bi - space.Height) b = bi - space.Height;
            }

            return new Margin(t, r, b, l);
        }

        internal void DrawSeparators(ChartModel chart)
        {
            ActualStep = GetActualAxisStep(chart);

            var from = Math.Ceiling(ActualMinValue / ActualStep) * ActualStep;
            var to = Math.Floor(ActualMaxValue / ActualStep) * ActualStep;

            var tolerance = ActualStep * .1;

            for (var i = from; i < to; i += ActualStep)
            {
                var iui = chart.ScaleToUi(i, this);
                var label = EvaluateAxisLabel(i, chart.DrawAreaSize);
                var key = Math.Round(i / tolerance) * tolerance;

                if (!_activeSeparators.TryGetValue(key, out var separator))
                {
                    separator = LiveChartsSettings.Current.UiProvider.AxisSeparatorProvider();
                    _activeSeparators.Add(key, separator);
                }
                chart.RegisterResource(separator);
                if (PlaneType == PlaneTypes.X)
                {
                    separator.Move(new Point(iui, 0), new Point(iui, chart.DrawAreaSize.Height), label, false, this, chart.View);
                }
                else
                {
                    separator.Move(new Point(0, iui), new Point(chart.DrawAreaSize.Width, iui), label, false, this, chart.View);
                }
                chart.RegisterResource(separator);
            }
        }

        /// <inheritdoc cref="Plane.OnDispose"/>
        protected override void OnDispose(IChartView chart)
        {
            foreach (var separator in _activeSeparators)
            {
                separator.Value.Dispose(chart);
            }
            _activeSeparators.Clear();
        }
        
        /// <inheritdoc />
        protected override IPlaneLabelControl DefaultLabelProvider()
        {
            return LiveChartsSettings.Current.UiProvider.AxisLabelProvider();
        }

        private double GetActualAxisStep(ChartModel chart)
        {
            if (!double.IsNaN(Step))
            {
                return Step;
            }

            var range = ActualMaxValue - ActualMinValue;
            range = range <= 0 ? 1 : range;

            const double cleanFactor = 35d;

            //ToDO: Improve this according to current labels!
            var separations = PlaneType == PlaneTypes.Y
                ? Math.Round(chart.DrawAreaSize.Height / cleanFactor, 0)
                : Math.Round(chart.DrawAreaSize.Width / cleanFactor, 0);

            separations = separations < 2 ? 2 : separations;

            var minimum = range / separations;
            var magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));

            var residual = minimum / magnitude;
            double tick;
            if (residual > 5)
            {
                tick = 10 * magnitude;
            }
            else if (residual > 2)
            {
                tick = 5 * magnitude;
            }
            else if (residual > 1)
            {
                tick = 2 * magnitude;
            }
            else
            {
                tick = magnitude;
            }

            if (Labels != null)
            {
                return tick < 1 ? 1 : tick;
            }

            return tick;
        }

        private AxisLabelModel EvaluateAxisLabel(double value, Size drawMargin)
        {
            const double toRadians = Math.PI / 180;
            var angle = ActualLabelsRotation;

            var labelModel = LabelProvider().Measure(FormatValue(value));

            var xw = Math.Abs(Math.Cos(angle * toRadians) * labelModel.Width);  // width's    horizontal    component
            var yw = Math.Abs(Math.Sin(angle * toRadians) * labelModel.Width);  // width's    vertical      component
            var xh = Math.Abs(Math.Sin(angle * toRadians) * labelModel.Height); // height's   horizontal    component
            var yh = Math.Abs(Math.Cos(angle * toRadians) * labelModel.Height); // height's   vertical      component

            // You can find more information about the cases at 
            // appendix/labels.2.png

            double x, y, xo, yo, l, t;

            if (PlaneType == PlaneTypes.X && Position == AxisPositions.Bottom)
            {
                // case 1
                if (angle < 0)
                {
                    xo = -xw - .5 * xh;
                    yo = -yw;
                    l = -1 * xo;
                    t = 0;
                }
                // case 2
                else
                {
                    xo = .5 * xh;
                    yo = 0;
                    l = .5 * xh;
                    t = 0;
                }
                x = value;
                y = drawMargin.Height;
            }
            else if (PlaneType == PlaneTypes.X && Position == AxisPositions.Top)
            {
                // case 3
                if (angle < 0)
                {
                    xo = -.5 * xh;
                    yo = yh;
                    l = -1 * xo;
                    t = yh + yw;
                }
                // case 4
                else
                {
                    xo = -xw + .5 * xh;
                    yo = yh + yw;
                    l = xw + .5 * xh;
                    t = yh + yw;
                }
                x = value;
                y = 0;
            }
            else if (PlaneType == PlaneTypes.Y && Position == AxisPositions.Left)
            {
                // case 6
                if (angle < 0)
                {
                    xo = -xh - xw;
                    yo = -yw + .5 * yh;
                    l = xh + xw;
                    t = .5 * yh;
                }
                // case 5
                else
                {
                    xo = -xw;
                    yo = yw;
                    l = xh + xw;
                    t = yw;
                }
                x = 0;
                y = value;
            }
            else if (PlaneType == PlaneTypes.Y && Position == AxisPositions.Right)
            {
                // case 8
                if (angle < 0)
                {
                    xo = 0;
                    yo = .5 * yh;
                    l = 0;
                    t = .5 * yh + yw;
                }
                // case 7
                else
                {
                    xo = .5 * xh;
                    yo = .5 * yh;
                    l = 0;
                    t = yo;
                }
                x = drawMargin.Width;
                y = value;
            }
            else
            {
                throw new LiveChartsException(
                    $"An axis of type '{PlaneType}' can not be positioned at '{Position}'", 120);
            }

            return new AxisLabelModel(
                new Point(x, y),
                new Point(xo, yo),
                new Margin(
                    t,
                    xw + xh - l,
                    yw + yh - t,
                    l));
        }
    }
}