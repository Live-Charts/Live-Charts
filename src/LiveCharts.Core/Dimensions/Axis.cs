#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Dimensions;
using LiveCharts.Core.Interaction.Events;
#if NET45 || NET46
using Font = LiveCharts.Core.Drawing.Styles.Font;
#endif
#endregion

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a cartesian linear axis.
    /// </summary>
    public class Axis : Plane
    {
        internal Dictionary<ChartModel, Dictionary<double, PlaneSeparator>> DependentCharts =
            new Dictionary<ChartModel, Dictionary<double, PlaneSeparator>>();

        private IPlaneViewProvider _planeViewProvider;
        private double _step;
        private double _stepStart;
        private AxisPosition _position;
        private ShapeStyle _xSeparatorStyle;
        private ShapeStyle _xAlternativeSeparatorStyle;
        private ShapeStyle _ySeparatorStyle;
        private ShapeStyle _yAlternativeSeparatorStyle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        public Axis()
        {
            Step = double.NaN;
            StepStart = double.NaN;
            MaxRange = double.MaxValue;
            MinRange = double.MinValue;
            Position = AxisPosition.Auto;
            SharedAxes = new List<Axis>();
            XSeparatorStyle =
                new ShapeStyle(
                    Charting.Settings.UiProvider.GetNewSolidColorBrush(255,250,250,250),
                    Charting.Settings.UiProvider.GetNewSolidColorBrush(50, 240, 240, 240),
                    1,
                    null);
            YSeparatorStyle = null;
            XAlternativeSeparatorStyle = null;
            YSeparatorStyle = null;
            YSeparatorStyle = null;
            Charting.BuildFromTheme(this);
        }

        /// <summary>
        /// Gets or sets the step, the space between every separator in the plane units,
        /// use double.NaN to let the library calculate this property for you based on the
        /// chart size and the plane units.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        public double Step
        {
            get => _step;
            set
            {
                _step = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the used <see cref="Step"/>, if <see cref="Step"/> is double.NaN, 
        /// the library will calculate this property and will expose the calculated value here,
        /// if <see cref="Step"/> is not double.NaN, <see cref="ActualStep"/> is equals
        /// to <see cref="Step"/> property.
        /// </summary>
        /// <value>
        /// The actual step.
        /// </value>
        public double ActualStep { get; internal set; }

        /// <summary>
        /// Gets or sets the step start, it is the value where the first separator is drawn, 
        /// the next separators will be drawn based on this property and the <see cref="ActualStep"/> property
        /// ( first separator at st + 0 * s, second at st + 1 * s, third at st + 2 * s ... n at st + (n-1) * s )
        /// where st is <see cref="ActualStepStart"/> and s is <see cref="ActualStep"/>, use double.NaN to let
        /// the library calculate this value for you.
        /// </summary>
        /// <value>
        /// The step start.
        /// </value>
        public double StepStart
        {
            get => _stepStart;
            set
            {
                _stepStart = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the used <see cref="StepStart"/>, if <see cref="StepStart"/> is double.NaN, 
        /// the library will calculate this property and will expose the calculated value here,
        /// if <see cref="StepStart"/> is not double.NaN, <see cref="ActualStepStart"/> is equals
        /// to <see cref="StepStart"/> property.
        /// </summary>
        /// <value>
        /// The actual step start.
        /// </value>
        public double ActualStepStart { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum range, it represents the upper limit for the zooming, the
        /// user will not be able to zoom further this range, see <see cref="Plane.ActualRange"/>.
        /// </summary>
        /// <value>
        /// The maximum range.
        /// </value>
        public double MaxRange { get; set; }

        /// <summary>
        /// Gets or sets the minimum range, it represents the lower limit for the zooming, the
        /// user will not be able to zoom further this range, see <see cref="Plane.ActualRange"/>.
        /// </summary>
        /// <value>
        /// The minimum range.
        /// </value>
        public double MinRange { get; set; }

        internal AxisPosition ActualPosition { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public AxisPosition Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the shares with collection, add an <see cref="Axis"/> here so the the space
        /// between all the elements in this collection will be computed together, then
        /// the space taken by this axis will be the maximum according to the collection.
        /// </summary>
        /// <value>
        /// The shares with.
        /// </value>
        public List<Axis> SharedAxes { get;}

        /// <summary>
        /// Gets or sets the x axis separator style.
        /// </summary>
        /// <value>
        /// The x axis separator style.
        /// </value>
        public ShapeStyle XSeparatorStyle
        {
            get => _xSeparatorStyle;
            set
            {
                _xSeparatorStyle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the x axis alternative separator style.
        /// </summary>
        /// <value>
        /// The x axis alternative separator style.
        /// </value>
        public ShapeStyle XAlternativeSeparatorStyle
        {
            get => _xAlternativeSeparatorStyle;
            set
            {
                _xAlternativeSeparatorStyle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the y axis separator style.
        /// </summary>
        /// <value>
        /// The y axis separator style.
        /// </value>
        public ShapeStyle YSeparatorStyle
        {
            get => _ySeparatorStyle;
            set
            {
                _ySeparatorStyle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the y axis alternative separator style.
        /// </summary>
        /// <value>
        /// The y axis alternative separator style.
        /// </value>
        public ShapeStyle YAlternativeSeparatorStyle
        {
            get => _yAlternativeSeparatorStyle;
            set
            {
                _yAlternativeSeparatorStyle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the separator provider.
        /// </summary>
        /// <value>
        /// The separator provider.
        /// </value>
        public IPlaneViewProvider ViewProvider
        {
            get => _planeViewProvider ?? (_planeViewProvider = DefaultViewProvider());
            set
            {
                _planeViewProvider = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        public TimeSpan AnimationsSpeed { get; set; }

        /// <summary>
        /// Gets or sets the animation line.
        /// </summary>
        /// <value>
        /// The animation line.
        /// </value>
        public IEnumerable<KeyFrame> AnimationLine { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string d = Dimension == 0 ? "X" : "Y";
            return $"{d} at {Position}, from {FormatValue(ActualMinValue)} to {FormatValue(ActualMaxValue)} @ {FormatValue(ActualStep)}";
        }

        internal Margin CalculateAxisMargin(ChartModel chart, Axis axis)
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
            float[] space = chart.DrawAreaSize;
            if (!(axis.Dimension == 0 || axis.Dimension == 1))
            {
                throw new LiveChartsException(
                    103, (object) axis.Dimension);
            }
            float dimension = space[axis.Dimension];
            float step = (float) (double.IsNaN(Step) ? 5d : Step);

            float l = 0f, r = 0f, t = 0f, b = 0f;

            var dummyControl = axis.ViewProvider.GetMeasurableLabel();
            chart.View.Content.AddChild(dummyControl, false);

            var labelsStyle = new LabelStyle
            {
                Font = axis.LabelsFont,
                Foreground =  axis.LabelsForeground,
                LabelsRotation = axis.LabelsRotation,
                Padding = axis.LabelsPadding
            };

            for (float i = 0f; i < dimension; i += step)
            {
                var label = axis.EvaluateAxisLabel(
                    dummyControl,
                    labelsStyle,
                    (float) chart.ScaleFromUi(i, axis, space),
                    space,
                    chart);

                float li = label.Pointer.X - label.Margin.Left;
                if (li < 0 && l > li) l = -li;

                float ri = label.Pointer.X + label.Margin.Right;
                if (ri > space[0] && r < ri - space[0]) r = ri - space[0];

                float ti = label.Pointer.Y - label.Margin.Top;
                if (ti < 0 && t > ti) t = -ti;

                float bi = label.Pointer.Y + label.Margin.Bottom;
                if (bi > space[1] && b < bi - space[1]) b = bi - space[1];
            }

            chart.View.Content.DisposeChild(dummyControl, false);

            return new Margin(t, r, b, l);
        }

        internal void DrawSeparators(ChartModel chart, LabelStyle labelStyle)
        {
            ActualStep = GetActualAxisStep(chart);
            ActualStepStart = GetActualStepStart();
            double from = Math.Ceiling(ActualStepStart / ActualStep) * ActualStep;
            double to = Math.Floor(InternalMaxValue / ActualStep) * ActualStep;

            double tolerance = ActualStep * .1f;
            float stepSize = Math.Abs(chart.ScaleToUi(ActualStep, this) - chart.ScaleToUi(0, this));
            bool alternate = false;

            var dummyControl = ViewProvider.GetMeasurableLabel();
            chart.View.Content.AddChild(dummyControl, false);

            float delta = (float) ActualStep;

            if (DependentCharts == null) DependentCharts = new Dictionary<ChartModel, Dictionary<double, PlaneSeparator>>();

            if (!DependentCharts.TryGetValue(chart, out Dictionary<double, PlaneSeparator> viewHolder))
            {
                viewHolder = new Dictionary<double, PlaneSeparator>();
                DependentCharts.Add(chart, viewHolder);
            }

            for (float i = (float) from; i <= to + tolerance; i += delta)
            {
                alternate = !alternate;
                float iui = chart.ScaleToUi(i, this);
                double key = Math.Round(i / tolerance) * tolerance;

                if (!viewHolder.TryGetValue(key, out var separator))
                {
                    separator = new PlaneSeparator
                    {
                        View = ViewProvider.GetNewVisual()
                    };
                    viewHolder.Add(key, separator);
                }

                var animation = new TimeLine
                {
                    Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                    AnimationLine = AnimationLine ?? chart.View.AnimationLine
                };

                var labelModel = EvaluateAxisLabel(dummyControl, labelStyle, i, chart.DrawAreaSize, chart);

                if (Dimension == 0)
                {
                    float w = iui + stepSize > chart.DrawAreaSize[0] ? 0 : stepSize;
                    var xModel = new RectangleF(new PointF(iui, 0), new SizeF(w, chart.DrawAreaSize[1]));

                    var args = new CartesianAxisSectionArgs
                    {
                        ZIndex = int.MinValue,
                        Plane = this,
                        // ToDo: this probably wont look good if the axis range changed, in that case it should mode from the previous range position to the new one..
                        Rectangle = new RectangleViewModel(xModel, xModel, Orientation.Auto),
                        Label = labelModel,
                        Disposing = false,
                        Style = alternate ? XAlternativeSeparatorStyle : XSeparatorStyle,
                        ChartView = chart.View
                    };

                    separator.View.DrawShape(args, animation);
                    if (ShowLabels) separator.View.DrawLabel(args, animation);
                }
                else
                {
                    float h = iui + stepSize > chart.DrawAreaSize[1] ? 0 : stepSize;
                    var yModel = new RectangleF(new PointF(0, iui), new SizeF(chart.DrawAreaSize[0], h));

                    var args = new CartesianAxisSectionArgs
                    {
                        ZIndex = int.MinValue,
                        Plane = this,
                        Rectangle = new RectangleViewModel(yModel, yModel, Orientation.Auto),
                        Label = labelModel,
                        Disposing = false,
                        Style = alternate ? YAlternativeSeparatorStyle : YSeparatorStyle,
                        ChartView = chart.View
                    };

                    separator.View.DrawShape(args, animation);
                    if (ShowLabels) separator.View.DrawLabel(args, animation);
                }

                chart.RegisterINotifyPropertyChanged(separator);
            }

            chart.View.Content.DisposeChild(dummyControl, false);

            // remove unnecessary elements from cache
            // the visual element will be removed by the chart's resource collector
            foreach (KeyValuePair<ChartModel, Dictionary<double, PlaneSeparator>> holder in DependentCharts.ToArray())
            {
                foreach (KeyValuePair<double, PlaneSeparator> separator in holder.Value.ToArray())
                {
                    if (separator.Value.UpdateId != chart.UpdateId)
                    {
                        viewHolder.Remove(separator.Key);
                    }
                }
            }
        }

        internal void DrawSections(ChartModel chart, LabelStyle labelStyle)
        {
            if (Sections == null) return;

            bool isX = Dimension == 0;

            if (isX)
            {
                labelStyle.LabelsRotation = -90;
            }

            var animation = new TimeLine
            {
                Duration = AnimationsSpeed == TimeSpan.MaxValue ? chart.View.AnimationsSpeed : AnimationsSpeed,
                AnimationLine = AnimationLine ?? chart.View.AnimationLine
            };

            foreach (var section in Sections)
            {
                if (section.View == null)
                {
                    section.View = section.ViewProvider.GetNewVisual();
                }

                float iui = chart.ScaleToUi(section.Value, this);
                float jui = chart.ScaleToUi(section.Value + section.Length, this);

                float length = Math.Abs(iui - jui);

                float w = iui + length > chart.DrawAreaSize[0] ? 0 : length;
                float h = iui + length > chart.DrawAreaSize[1] ? 0 : length;
                
                float t = isX ? (Reverse ? jui : iui) : (Reverse ? iui : jui);

                var model = isX
                    ? new RectangleF(new PointF(t, 0), new SizeF(w, chart.DrawAreaSize[1]))
                    : new RectangleF(new PointF(0, t), new SizeF(chart.DrawAreaSize[0], h));

                var dummyLabel = section.ViewProvider.GetMeasurableLabel();
                chart.View.Content.AddChild(dummyLabel, false);
                var labelSize = dummyLabel.Update(section.LabelContent, labelStyle);

                float x;
                float y;
                const float m = 3f; // default margin

                switch (section.LabelHorizontalAlignment)
                {
                    case HorizontalAlignment.Centered:
                    case HorizontalAlignment.Between:
                        x = isX
                            ? t + length * .5f - labelSize.Height * .5f
                            : chart.DrawAreaSize[0] * .5f - labelSize.Width * .5f;
                        break;
                    case HorizontalAlignment.Left:
                        x = isX ? t + m : m;
                        break;
                    case HorizontalAlignment.Right:
                        x = isX
                            ? t + length - m - labelSize.Height
                            : chart.DrawAreaSize[0] - m - labelSize.Width;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (section.LabelVerticalAlignment)
                {
                    case VerticalAlignment.Between:
                    case VerticalAlignment.Centered:
                        y = isX
                            ? chart.DrawAreaSize[1] * .5f + labelSize.Width * .5f
                            : t + length * .5f - labelSize.Height * .5f;
                        break;
                    case VerticalAlignment.Top:
                        y = isX
                            ? m + labelSize.Width
                            : t + m;
                        break;
                    case VerticalAlignment.Bottom:
                        y = isX
                            ? chart.DrawAreaSize[1]- m
                            : t + length - labelSize.Height - m;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var args = new CartesianAxisSectionArgs
                {
                    ZIndex = int.MinValue + 1,
                    Plane = this,
                    Rectangle = new RectangleViewModel(model, model, Orientation.Auto),
                    Label = new AxisSectionViewModel(
                        new PointF(x, y),
                        new PointF(0, 0),
                        new Margin(0),
                        section.LabelContent,
                        labelSize,
                        labelStyle,
                        Position),
                    Disposing = false,
                    Style = new ShapeStyle(
                        section.Stroke,
                        section.Fill,
                        (float) section.StrokeThickness,
                        section.StrokeDashArray),
                    ChartView = chart.View
                };

                section.View.DrawShape(args, animation);
                section.View.DrawLabel(args, animation);

                chart.RegisterINotifyPropertyChanged(section);
                chart.View.Content.DisposeChild(dummyLabel, false);
            }
        }

        /// <inheritdoc cref="Plane.OnDispose"/>
        protected override void OnDispose(IChartView chart, bool force)
        {
            foreach (KeyValuePair<ChartModel, Dictionary<double, PlaneSeparator>> holder in DependentCharts ?? new Dictionary<ChartModel, Dictionary<double, PlaneSeparator>>())
            {
                foreach (KeyValuePair<double, PlaneSeparator> separator in holder.Value)
                {
                    separator.Value.Dispose(chart, force);
                }
            }

            foreach (var section in Sections ?? Enumerable.Empty<Section>())
            {
                section.View.Dispose(chart, force);
            }

            DependentCharts?.Clear();
            DependentCharts = null;
        }
        
        /// <inheritdoc />
        protected override IPlaneViewProvider DefaultViewProvider()
        {
            return Charting.Settings.UiProvider.GetNewPlane();
        }

        /// <summary>
        /// Gets the actual step start.
        /// </summary>
        /// <returns></returns>
        protected virtual float GetActualStepStart()
        {
            if (!double.IsNaN(StepStart))
            {
                return (float) StepStart;
            }

            return (float) InternalMinValue;
        }

        /// <summary>
        /// Gets the actual axis step.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        protected virtual float GetActualAxisStep(ChartModel chart)
        {
            if (!double.IsNaN(Step))
            {
                return (float) Step;
            }

            double range = InternalMaxValue - InternalMinValue;
            range = range <= 0 ? 1 : range;

            const double cleanFactor = 50d;

            //ToDO: Improve this according to current labels!
            double separations = Math.Round(chart.DrawAreaSize[Dimension] / cleanFactor, 0);

            separations = separations < 2 ? 2 : separations;

            double minimum = range / separations;
            double magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));

            double residual = minimum / magnitude;
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

        private AxisSectionViewModel EvaluateAxisLabel(
            IMeasurableLabel control, 
            LabelStyle labelStyle, 
            float value, 
            float[] drawMargin,
            ChartModel chart)
        {
            if (!ShowLabels)
            {
                return new AxisSectionViewModel();
            }

            const double toRadians = Math.PI / 180;
            double angle = labelStyle.ActualLabelsRotation;
            string text = FormatValue(value);
            var labelSize = control.Update(text, labelStyle);

            double xw = Math.Abs(Math.Cos(angle * toRadians) * labelSize.Width);  // width's    horizontal    component
            double yw = Math.Abs(Math.Sin(angle * toRadians) * labelSize.Width);  // width's    vertical      component
            double xh = Math.Abs(Math.Sin(angle * toRadians) * labelSize.Height); // height's   horizontal    component
            double yh = Math.Abs(Math.Cos(angle * toRadians) * labelSize.Height); // height's   vertical      component

            bool isSecondQuadrant = angle >= 270;

            // You can find more information about the cases at 
            // docs/resources/labels.2.png

            double x, y, xo, yo, l, t;

            switch (Dimension)
            {
                case 0 when ActualPosition == AxisPosition.Bottom || ActualPosition == AxisPosition.Left:
                    // case 1
                    if (isSecondQuadrant)
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
                case 0 when ActualPosition == AxisPosition.Top || ActualPosition == AxisPosition.Right:
                    // case 3
                    if (isSecondQuadrant)
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
                case 1 when ActualPosition == AxisPosition.Left || ActualPosition == AxisPosition.Bottom:
                    // case 6
                    if (isSecondQuadrant)
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
                        yo = -yw;
                        l = xh + xw;
                        t = yw;
                    }
                    x = 0;
                    y = chart.ScaleToUi(value, this, drawMargin);
                    break;
                case 1 when ActualPosition == AxisPosition.Right || ActualPosition == AxisPosition.Top:
                    // case 8
                    if (isSecondQuadrant)
                    {
                        xo = 0;
                        yo = .5 * yh;
                        l = 0;
                        t = .5 * yh + yw;
                    }
                    // case 7
                    else
                    {
                        xo = xh;
                        yo = - .5 * yh;
                        l = 0;
                        t = yo;
                    }
                    x = drawMargin[0];
                    y = chart.ScaleToUi(value, this, drawMargin);
                    break;
                default:
                    string d = Dimension == 0 ? "X" : "Y";
                    throw new LiveChartsException(106, d, Position);
            }

            // correction by rotation
            // ReSharper disable once InvertIf
            if (Math.Abs(labelStyle.ActualLabelsRotation) < 0.001)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (Dimension == 0)
                {
                    int direction = Position != AxisPosition.Top ? 1 : -1;
                    xo = xo - xw * .5f * direction;
                }
                else if (Dimension == 1)
                {
                    yo = yo - yh * .5f;
                }
            }

            return new AxisSectionViewModel(
                new PointF(
                    (float) x - ByStackMargin.Left + ByStackMargin.Right,
                    (float) y - ByStackMargin.Top + ByStackMargin.Bottom),
                new PointF((float) xo, (float) yo),
                new Margin(
                    (float) t,
                    (float) (xw + xh - l),
                    (float) (yw + yh - t),
                    (float) l),
                text,
                labelSize,
                labelStyle,
                Position);
        }
    }
}
