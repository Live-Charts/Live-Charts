using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a cartesian linear axis.
    /// </summary>
    public class Axis : Plane
    {
        private readonly Dictionary<double, ICartesianAxisSeparator> _activeSeparators =
            new Dictionary<double, ICartesianAxisSeparator>();
        private Func<IPlaneLabelControl> _separatorProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        public Axis()
        {
            Step = float.NaN;
            StepStart = float.NaN;
            Position = AxisPosition.Auto;
            XSeparatorStyle =
                new SeparatorStyle(Color.FromArgb(255, 250, 250, 250), Color.FromArgb(50, 240, 240, 240), 1);
            YSeparatorStyle = SeparatorStyle.Empty;
            XAlternativeSeparatorStyle = SeparatorStyle.Empty;
            YSeparatorStyle = SeparatorStyle.Empty;
            Charting.BuildFromSettings(this);
        }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        public float Step { get; set; }

        /// <summary>
        /// Gets the actual step.
        /// </summary>
        /// <value>
        /// The actual step.
        /// </value>
        public float ActualStep { get; internal set; }

        /// <summary>
        /// Gets or sets the step start.
        /// </summary>
        /// <value>
        /// The step start.
        /// </value>
        public float StepStart { get; set; }

        /// <summary>
        /// Gets the actual step start.
        /// </summary>
        /// <value>
        /// The actual step start.
        /// </value>
        public float ActualStepStart { get; internal set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public AxisPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the x axis separator style.
        /// </summary>
        /// <value>
        /// The x axis separator style.
        /// </value>
        public SeparatorStyle XSeparatorStyle { get; set; }

        /// <summary>
        /// Gets or sets the x axis alternative separator style.
        /// </summary>
        /// <value>
        /// The x axis alternative separator style.
        /// </value>
        public SeparatorStyle XAlternativeSeparatorStyle { get; set; }

        /// <summary>
        /// Gets or sets the y axis separator style.
        /// </summary>
        /// <value>
        /// The y axis separator style.
        /// </value>
        public SeparatorStyle YSeparatorStyle { get; set; }

        /// <summary>
        /// Gets or sets the y axis alternative separator style.
        /// </summary>
        /// <value>
        /// The y axis alternative separator style.
        /// </value>
        public SeparatorStyle YAlternativeSeparatorStyle { get; set; }

        /// <summary>
        /// Gets or sets the separator provider.
        /// </summary>
        /// <value>
        /// The separator provider.
        /// </value>
        public Func<IPlaneLabelControl> LabelProvider
        {
            get => _separatorProvider ?? DefaultLabelProvider;
            set
            {
                _separatorProvider = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var d = Dimension == 0 ? "X" : "Y";
            return $"{d} at {Position}, from {FormatValue(ActualMinValue)} to {FormatValue(ActualMaxValue)} @ {FormatValue(ActualStep)}";
        }

        internal Margin CalculateAxisMargin(ChartModel sizeVector)
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

            var space = sizeVector.DrawAreaSize;
            if (!(Dimension == 0 || Dimension == 1))
            {
                throw new LiveChartsException(
                    $"A Cartesian chart is not able to handle dimension {Dimension}.", 130);
            }
            var dimension = space[Dimension];
            var step = 5; // ToDo: or use the real if it is defined?

            var unit = ActualPointWidth?[Dimension] ?? 0f;

            float l = 0f, r = 0f, t = 0f, b = 0f;
            for (var i = 0f; i < dimension; i += step)
            {
                var label = EvaluateAxisLabel(sizeVector.ScaleFromUi(i, this, space), space, unit, sizeVector);

                var li = label.Location.X - label.Margin.Left;
                if (li < 0 && l < -li) l = -li;

                var ri = label.Location.X + label.Margin.Right;
                if (ri > space[0] && r < ri - space[0]) r = ri - space[0];

                var ti = label.Location.Y - label.Margin.Top;
                if (ti < 0 && t < ti) t = -ti;

                var bi = label.Location.Y + label.Margin.Bottom;
                if (bi > space[1] && b < bi - space[1]) b = bi - space[1];
            }

            return new Margin(t, r, b, l);
        }

        internal void DrawSeparators(ChartModel chart)
        {
            ActualStep = GetActualAxisStep(chart);
            ActualStepStart = GetActualStepStart(chart);

            var from = Math.Ceiling(ActualMinValue / ActualStep) * ActualStep;
            var to = Math.Floor(ActualMaxValue / ActualStep) * ActualStep;
            var unit = ActualPointWidth?[Dimension] ?? 0;

            var tolerance = ActualStep * .1f;
            var stepSize = Math.Abs(chart.ScaleToUi(ActualStep, this) - chart.ScaleToUi(0, this));
            var alternate = false;

            for (var i = (float) from; i < to + unit; i += ActualStep)
            {
                alternate = !alternate;
                var iui = chart.ScaleToUi(i, this);
                var label = EvaluateAxisLabel(i, chart.DrawAreaSize, unit, chart);
                var key = Math.Round(i / tolerance) * tolerance;

                if (!_activeSeparators.TryGetValue(key, out var separator))
                {
                    separator = Charting.Current.UiProvider.GetNewAxisSeparator();
                    _activeSeparators.Add(key, separator);
                }

                if (Dimension == 0)
                {
                    var w = iui + stepSize > chart.DrawAreaSize[0] ? 0 : stepSize;
                    var xModel = new RectangleF(new PointF(iui, 0), new SizeF(w, chart.DrawAreaSize[1]));
                    separator.Move(
                        new CartesianAxisSeparatorArgs
                        {
                            SeparatorFrom = xModel, // ToDo: this probably wont look wood if the axis range changed, in that case it should mode from the previous range position to the new one..
                            SeparatorTo = xModel,
                            AxisLabelViewModel = label,
                            Disposing = false,
                            Style = alternate ? XAlternativeSeparatorStyle : XSeparatorStyle,
                            ChartView = chart.View
                        });
                }
                else
                {
                    var h = iui + stepSize > chart.DrawAreaSize[1] ? 0 : stepSize;
                    var yModel = new RectangleF(new PointF(0, iui), new SizeF(chart.DrawAreaSize[0], h));
                    separator.Move(
                        new CartesianAxisSeparatorArgs
                        {
                            SeparatorFrom = yModel, // ToDo: this probably wont look wood if the axis range changed, in that case it should mode from the previous range position to the new one..
                            SeparatorTo = yModel,
                            AxisLabelViewModel = label,
                            Disposing = false,
                            Style = alternate ? YAlternativeSeparatorStyle : YSeparatorStyle,
                            ChartView = chart.View
                        });
                }

                chart.RegisterResource(separator);
            }

            // remove unnecessary elements from cache
            foreach (var separator in _activeSeparators.ToArray())
            {
                if (separator.Value.UpdateId != chart.UpdateId)
                {
                    _activeSeparators.Remove(separator.Key);
                }
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
            return Charting.Current.UiProvider.GetNewAxisLabel();
        }

        private float GetActualStepStart(ChartModel chart)
        {
            if (!float.IsNaN(StepStart))
            {
                return StepStart;
            }

            var unit = ActualPointWidth?[Dimension] ?? 0;

            return ActualMinValue + unit / 2;
        }

        private float GetActualAxisStep(ChartModel chart)
        {
            if (!float.IsNaN(Step))
            {
                return Step;
            }

            var unit = ActualPointWidth?[Dimension] ?? 0;

            var range = ActualMaxValue + unit - ActualMinValue;
            range = range <= 0 ? 1 : range;

            const double cleanFactor = 50d;

            //ToDO: Improve this according to current labels!
            var separations = Math.Round(chart.DrawAreaSize[Dimension] / cleanFactor, 0);

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
                return (float) (tick < 1 ? 1 : tick);
            }

            return (float) tick;
        }

        private AxisLabelViewModel EvaluateAxisLabel(float value, float[] drawMargin, float axisPointWidth, ChartModel chart)
        {
            const double toRadians = Math.PI / 180;
            var angle = ActualLabelsRotation;

            var content = FormatValue(value); // FormatValue(value - .5 * unit);
            var labelSize = LabelProvider().Measure(content);

            var xw = Math.Abs(Math.Cos(angle * toRadians) * labelSize.Width);  // width's    horizontal    component
            var yw = Math.Abs(Math.Sin(angle * toRadians) * labelSize.Width);  // width's    vertical      component
            var xh = Math.Abs(Math.Sin(angle * toRadians) * labelSize.Height); // height's   horizontal    component
            var yh = Math.Abs(Math.Cos(angle * toRadians) * labelSize.Height); // height's   vertical      component

            // You can find more information about the cases at 
            // appendix/labels.2.png

            double x, y, xo, yo, l, t;

            switch (Dimension)
            {
                case 0 when Position == AxisPosition.Bottom:
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
                    x = chart.ScaleToUi(value, this, drawMargin);
                    y = drawMargin[1];
                    break;
                case 0 when Position == AxisPosition.Top:
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
                    x = chart.ScaleToUi(value, this, drawMargin);
                    y = 0;
                    break;
                case 1 when Position == AxisPosition.Left:
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
                    y = chart.ScaleToUi(value, this, drawMargin);
                    break;
                case 1 when Position == AxisPosition.Right:
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
                    x = drawMargin[0];
                    y = chart.ScaleToUi(value, this, drawMargin);
                    break;
                default:
                    var d = Dimension == 0 ? "X" : "Y";
                    throw new LiveChartsException(
                        $"An axis at dimension '{d}' can not be positioned at '{Position}'", 120);
            }

            // correction by axis point unit
            if (axisPointWidth > 0f)
            {
                var uiPw = chart.ScaleToUi(axisPointWidth, this) - chart.ScaleToUi(0, this);

                if (Dimension == 0)
                {
                    xo += uiPw *.5f;
                }
                else
                {
                    yo += uiPw * .5f;
                }
            }

            // correction by rotation
            // ReSharper disable once InvertIf
            if (Math.Abs(ActualLabelsRotation) < 0.001)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (Dimension == 0)
                {
                    xo = xo - xw * .5f;
                }
                else if (Dimension == 1)
                {
                    yo = yo - yh * .5f;
                }
            }

            return new AxisLabelViewModel(
                new PointF((float) x, (float) y),
                new PointF((float) xo, (float) yo),
                new Margin(
                    (float) t,
                    (float) (xw + xh - l),
                    (float) (yw + yh - t),
                    (float) l),
                content,
                labelSize);
        }
    }
}