//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

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
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Series;

namespace LiveCharts.Charts
{
    public class ScatterChart : Chart
    {
        public ScatterChart()
        {
            PrimaryAxis = new Axis();
            SecondaryAxis = new Axis();
            Hoverable = true;
            Zooming = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Dot;
            AnimatesNewPoints = true;
        }

        public static readonly DependencyProperty LineTypeProperty = DependencyProperty.Register(
            "LineType", typeof(LineChartLineType), typeof(ScatterChart));
        /// <summary>
        /// Iditacates series line type, use Bezier to get a smooth but aproximated line, or Polyline to
        /// draw a line only by the known points.
        /// </summary>
        public LineChartLineType LineType
        {
            get { return (LineChartLineType)GetValue(LineTypeProperty); }
            set { SetValue(LineTypeProperty, value); }
        }

        protected override bool ScaleChanged => GetMax() != Max ||
                                                GetMin() != Min;

        protected override void Scale()
        {
            Max = GetMax();
            Min = GetMin();
            S = GetS();

            Max.Y = PrimaryAxis.MaxValue ?? (Math.Truncate(Max.Y / S.Y) + 1) * S.Y;
            Min.Y = PrimaryAxis.MinValue ?? (Math.Truncate(Min.Y / S.Y) - 1) * S.Y;

            Max.X = SecondaryAxis.MaxValue ?? (Math.Truncate(Max.X / S.X) + 1) * S.X;
            Min.X = SecondaryAxis.MinValue ?? (Math.Truncate(Min.X / S.X) - 1) * S.X;

            DrawAxis();
        }

        public override void DataMouseEnter(object sender, MouseEventArgs e)
        {
            var b = new Border
            {
                BorderThickness = TooltipBorderThickness ?? new Thickness(0),
                Background = TooltipBackground ?? new SolidColorBrush {Color = Color.FromRgb(30, 30, 30), Opacity = .8},
                CornerRadius = TooltipCornerRadius ?? new CornerRadius(1),
                BorderBrush = TooltipBorderBrush ?? Brushes.Transparent
            };
            var sp = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            if (senderShape == null) return;

            senderShape.Target.Stroke = new SolidColorBrush {Color = senderShape.Serie.Color};
            senderShape.Target.Fill = new SolidColorBrush {Color = PointHoverColor};

            sp.Children.Add(new TextBlock
            {
                Text = "X:" +
                       (PrimaryAxis.LabelFormatter == null
                           ? senderShape.Value.X.ToString(CultureInfo.InvariantCulture)
                           : PrimaryAxis.LabelFormatter(senderShape.Value.X)) +
                       " Y: " +
                       (SecondaryAxis.LabelFormatter == null
                           ? senderShape.Value.Y.ToString(CultureInfo.InvariantCulture)
                           : SecondaryAxis.LabelFormatter(senderShape.Value.Y)),
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Calibri"),
                FontSize = 11,
                Foreground = TooltipForegroung ?? Brushes.White
            });

            b.Child = sp;
            Canvas.Children.Add(b);

            b.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var x = senderShape.Value.X > (Min.X + Max.X)/2
                ? ToPlotArea(senderShape.Value.X, AxisTags.X) - 10 - b.DesiredSize.Width
                : ToPlotArea(senderShape.Value.X, AxisTags.X) + 10;
            var y = senderShape.Value.Y > (Min.Y+ Max.Y)/2
                ? ToPlotArea(senderShape.Value.Y, AxisTags.Y) + 10
                : ToPlotArea(senderShape.Value.Y, AxisTags.Y) - 10 - b.DesiredSize.Height;

            Canvas.SetLeft(b, x);
            Canvas.SetTop(b, y);
            Panel.SetZIndex(b, int.MaxValue-1);
            CurrentToolTip = b;
        }

        public override void DataMouseLeave(object sender, MouseEventArgs e)
        {
            var s = sender as Shape;
            if (s == null) return;

            var shape = HoverableShapes.FirstOrDefault(x => Equals(x.Shape, s));
            if (shape == null) return;

            shape.Target.Fill = new SolidColorBrush {Color = shape.Serie.Color};
            shape.Target.Stroke = new SolidColorBrush {Color = PointHoverColor};

            Canvas.Children.Remove(CurrentToolTip);
        }

        private Point GetMax()
        {
            var p = new Point(
                SecondaryAxis.MaxValue ??
                Series.Cast<ScatterSerie>().Select(x => x.SecondaryValues.DefaultIfEmpty(0).Max()).DefaultIfEmpty(0).Max(),
                PrimaryAxis.MaxValue ??
				Series.Select(x => x.PrimaryValues.DefaultIfEmpty(0).Max()).DefaultIfEmpty(0).Max());
            p.X = SecondaryAxis.MaxValue ?? p.X;
            p.Y = PrimaryAxis.MaxValue ?? p.Y;
            return p;
        }

        private Point GetMin()
        {
            var p = new Point(
                Series.Cast<ScatterSerie>().Select(x => x.SecondaryValues.DefaultIfEmpty(0).Min()).DefaultIfEmpty(0).Min(),
				Series.Select(x => x.PrimaryValues.DefaultIfEmpty(0).Min()).DefaultIfEmpty(0).Min());
            p.X = SecondaryAxis.MinValue ?? p.X;
            p.Y = PrimaryAxis.MinValue ?? p.Y;
            return p;
        }

        private Point GetS()
        {
            return new Point(
                SecondaryAxis.Separator.Step ?? CalculateSeparator(Max.X - Min.X, AxisTags.X),
                PrimaryAxis.Separator.Step ?? CalculateSeparator(Max.Y - Min.Y, AxisTags.Y));
        }
    }
}