using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Charts.Series;

namespace Charts.Charts
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

            Max.Y = (Math.Truncate(Max.Y / S.Y) + 1) * S.Y;
            Min.Y = (Math.Truncate(Min.Y / S.Y) - 1) * S.Y;

            Max.X = (Math.Truncate(Max.X / S.X) + 1) * S.X;
            Min.X = (Math.Truncate(Min.X / S.X) - 1) * S.X;

            DrawAxis();
        }

        public override void OnDataMouseEnter(object sender, MouseEventArgs e)
        {
            var b = new Border
            {
                BorderThickness = new Thickness(0),
                Background = new SolidColorBrush {Color = Color.FromRgb(30, 30, 30), Opacity = .8},
                CornerRadius = new CornerRadius(1)
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
                Foreground = Brushes.White
            });

            b.Child = sp;
            Canvas.Children.Add(b);

            b.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var x = senderShape.Value.X > (Min.X + Max.X)/2
                ? ToPlotArea(senderShape.Value.X, AxisTags.X) - 10 - b.DesiredSize.Width
                : ToPlotArea(senderShape.Value.X, AxisTags.X) + 10;
            var y = senderShape.Value.Y > (Min.Y+ Max.Y)/2
                ? ToPlotArea(senderShape.Value.Y, AxisTags.Y) - 10 - b.DesiredSize.Height
                : ToPlotArea(senderShape.Value.Y, AxisTags.Y) + 10;

            Canvas.SetLeft(b, x);
            Canvas.SetTop(b, y);
            Panel.SetZIndex(b, int.MaxValue-1);
            CurrentToolTip = b;
        }

        public override void OnDataMouseLeave(object sender, MouseEventArgs e)
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
            return new Point(
                (Series.Cast<ScatterSerie>().Select(x => x.SecondaryValues.Max()).DefaultIfEmpty(0).Max()),
                (Series.Cast<ScatterSerie>().Select(x => x.PrimaryValues.Max()).DefaultIfEmpty(0).Max()));
        }

        private Point GetMin()
        {
            return new Point(
                (Series.Cast<ScatterSerie>().Select(x => x.SecondaryValues.Min()).DefaultIfEmpty(0).Min()),
                (Series.Cast<ScatterSerie>().Select(x => x.PrimaryValues.Min()).DefaultIfEmpty(0).Min()));
        }

        private Point GetS()
        {
            return new Point(
                CalculateSeparator(Max.X - Min.X, AxisTags.X),
                CalculateSeparator(Max.Y - Min.Y, AxisTags.Y));
        }
    }
}