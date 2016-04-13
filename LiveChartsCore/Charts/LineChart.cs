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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.CoreComponents;
using LiveCharts.Tooltip;

namespace LiveCharts
{
    public class LineChart : Chart, ILine
    {
        public LineChart()
        {
            ShapeHoverBehavior = ShapeHoverBehavior.Dot;
            LineSmoothness = 0.8;
        }

        #region Properties
        public double LineSmoothness { get; set; }

        #endregion

        #region Overriden Methods

        protected override void PrepareAxes()
        {
            if (!HasValidSeriesAndValues) return;

            base.PrepareAxes();

            foreach (var xi in AxisX)
            {
                xi.CalculateSeparator(this, AxisTags.X);
                if (!Invert) continue;
                if (xi.MaxValue == null) xi.MaxLimit = (Math.Round(xi.MaxLimit/xi.S) + 1)*xi.S;
                if (xi.MinValue == null) xi.MinLimit = (Math.Truncate(xi.MinLimit/xi.S) - 1)*xi.S;
            }

            foreach (var yi in AxisY)
            {
                yi.CalculateSeparator(this, AxisTags.Y);
                if (Invert) continue;
                if (yi.MaxValue == null) yi.MaxLimit = (Math.Round(yi.MaxLimit/yi.S) + 1)*yi.S;
                if (yi.MinValue == null) yi.MinLimit = (Math.Truncate(yi.MinLimit/yi.S) - 1)*yi.S;
            }

            CalculateComponentsAndMargin();
        }

        protected internal override void DataMouseEnter(object sender, MouseEventArgs e)
        {
            //this is only while we fix PCL try...
            if (DataTooltip == null || !Hoverable) return;

            DataTooltip.Visibility = Visibility.Visible;
            TooltipTimer.Stop();

            var senderShape = Series.SelectMany(
                x =>
                    x.IsPrimitive
                        ? x.Tracker.Primitives.Select(v => v.Value)
                        : x.Tracker.Instances.Select(v => v.Value))
                .FirstOrDefault(x => Equals(x.HoverShape, sender));
            if (senderShape == null) return;

            var targetAxis = Invert ? senderShape.Series.ScalesYAt : senderShape.Series.ScalesXAt;

            var sibilings = Invert
                ? Series.SelectMany(
                    x =>
                        x.IsPrimitive
                            ? x.Tracker.Primitives.Select(v => v.Value)
                            : x.Tracker.Instances.Select(v => v.Value))
                    .Where(s => Math.Abs(s.ChartPoint.Y - senderShape.ChartPoint.Y) < AxisY[targetAxis].S*.01)
                    .ToList()
                : Series.SelectMany(
                    x =>
                        x.IsPrimitive
                            ? x.Tracker.Primitives.Select(v => v.Value)
                            : x.Tracker.Instances.Select(v => v.Value))
                    .Where(s => Math.Abs(s.ChartPoint.X - senderShape.ChartPoint.X) < AxisX[targetAxis].S*.01)
                    .ToList();

            var first = sibilings.Count > 0 ? sibilings[0] : null;
            var vx = first != null ? (Invert ? first.ChartPoint.Y : first.ChartPoint.X) : 0;

            foreach (var sibiling in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    sibiling.Shape.Stroke = sibiling.Series.Stroke;
                    sibiling.Shape.Fill = new SolidColorBrush { Color = PointHoverColor };
                }
                else sibiling.Shape.Opacity = .8;
                sibiling.IsHighlighted = true;
            }

            var indexedToolTip = DataTooltip as IndexedTooltip;
            if (indexedToolTip != null)
            {
                var fh = (Invert ? AxisY[targetAxis] : AxisX[targetAxis]).GetFormatter();
                var fs = (Invert ? AxisX[targetAxis] : AxisY[targetAxis]).GetFormatter();

                indexedToolTip.Header = fh(vx);
                indexedToolTip.Data = sibilings.Select(x => new IndexedTooltipData
                {
                    Index = Series.IndexOf(x.Series),
                    Series = x.Series,
                    Stroke = x.Series.Stroke,
                    Fill = x.Series.Fill,
                    Point = x.ChartPoint,
                    Value = fs(Invert ? x.ChartPoint.X : x.ChartPoint.Y)
                }).ToArray();
            }

            //var p = GetToolTipPosition(senderShape, sibilings);

            DataTooltip.UpdateLayout();
            DataTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var xt = senderShape.ChartPoint.X > (AxisX[targetAxis].MinLimit + AxisX[targetAxis].MaxLimit) / 2
                ? senderShape.ChartPoint.Location.X - 10 - DataTooltip.DesiredSize.Width
                : senderShape.ChartPoint.Location.X + 10;

            xt += System.Windows.Controls.Canvas.GetLeft(DrawMargin);

            var y = sibilings.Select(s => s.ChartPoint.Location.Y).DefaultIfEmpty(0).Sum() / sibilings.Count;
            y = y + DataTooltip.DesiredSize.Height > ActualHeight ? y - (y + DataTooltip.DesiredSize.Height - ActualHeight) - 5 : y;
            var p = new Point(xt, y);

            DataTooltip.BeginAnimation(System.Windows.Controls.Canvas.LeftProperty, new DoubleAnimation
            {
                To = p.X,
                Duration = TimeSpan.FromMilliseconds(200)
            });
            DataTooltip.BeginAnimation(System.Windows.Controls.Canvas.TopProperty, new DoubleAnimation
            {
                To = p.Y,
                Duration = TimeSpan.FromMilliseconds(200)
            });
        }

        protected internal override void DataMouseLeave(object sender, MouseEventArgs e)
        {
            if (!Hoverable) return;

            var s = sender as Shape;
            if (s == null) return;

            var shape = Series.SelectMany(
                x =>
                    x.IsPrimitive
                        ? x.Tracker.Primitives.Select(v => v.Value)
                        : x.Tracker.Instances.Select(v => v.Value))
                .FirstOrDefault(x => Equals(x.HoverShape, s));
            if (shape == null) return;

            var targetAxis = Invert ? shape.Series.ScalesYAt : shape.Series.ScalesXAt;

            var sibilings = Invert
                ? Series.SelectMany(
                    x =>
                        x.IsPrimitive
                            ? x.Tracker.Primitives.Select(v => v.Value)
                            : x.Tracker.Instances.Select(v => v.Value))
                    .Where(se => Math.Abs(se.ChartPoint.Y - shape.ChartPoint.Y) < AxisY[targetAxis].S*.01)
                    .ToList()
                : Series.SelectMany(
                    x =>
                        x.IsPrimitive
                            ? x.Tracker.Primitives.Select(v => v.Value)
                            : x.Tracker.Instances.Select(v => v.Value))
                    .Where(x => x.IsHighlighted).ToList();

            foreach (var p in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    p.Shape.Fill = p.Series.Stroke;
                    p.Shape.Stroke = new SolidColorBrush { Color = PointHoverColor };
                }
                else
                {
                    p.Shape.Opacity = 1;
                }
            }
            TooltipTimer.Stop();
            TooltipTimer.Start();
        }

        internal override void DataMouseDown(object sender, MouseEventArgs e)
        {
            var shape =
                Series.SelectMany(
                    x =>
                        x.IsPrimitive
                            ? x.Tracker.Primitives.Select(v => v.Value)
                            : x.Tracker.Instances.Select(v => v.Value))
                    .FirstOrDefault(x => Equals(x.HoverShape, sender));
            if (shape == null) return;
            OnDataClick(shape.ChartPoint);
        }

        #endregion
    }
}