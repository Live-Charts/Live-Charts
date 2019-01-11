using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using System.Windows;
using System.Windows.Media;

namespace LiveCharts.Wpf
{
    public class ChartSlice : ChartShape, ISlice
    {
        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(double), typeof(ChartSlice), new PropertyMetadata(0d));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(ChartSlice), new PropertyMetadata(0d));

        public static readonly DependencyProperty ForceAngleProperty =
            DependencyProperty.Register("ForceAngle", typeof(bool), typeof(ChartSlice), new PropertyMetadata(false));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(ChartSlice), new PropertyMetadata(0d));

        public static readonly DependencyProperty PushOutProperty =
           DependencyProperty.Register("PushOut", typeof(double), typeof(ChartSlice), new PropertyMetadata(0d));

        public static readonly DependencyProperty WedgeProperty =
            DependencyProperty.Register("Wedge", typeof(double), typeof(ChartSlice), new PropertyMetadata(0d));

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public bool ForceAngle
        {
            get { return (bool)GetValue(ForceAngleProperty); }
            set { SetValue(ForceAngleProperty, value); }
        }

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public double PushOut
        {
            get { return (double)GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }

        public double Wedge
        {
            get { return (double)GetValue(WedgeProperty); }
            set { SetValue(WedgeProperty, value); }
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get
            {
                var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

                using (var context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            var center = new PointD((float)Height / 2, (float)Width / 2);

            var model = Slice.Build(
                Wedge, Radius, InnerRadius, CornerRadius, center, ForceAngle, PushOut);

            context.BeginFigure(model.Points[0].AsWpf(), true, true);
            context.LineTo(model.Points[1].AsWpf(), true, true);

            var cornerSize = new Size(model.CornerRadius, model.CornerRadius);

            // corner 1
            context.ArcTo(model.Points[2].AsWpf(),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.ArcTo(model.Points[3].AsWpf(), new Size(Radius, Radius), 0,
                model.IsRadiusLargeArc, SweepDirection.Counterclockwise, true, true);

            // corner 2
            context.ArcTo(model.Points[4].AsWpf(),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.LineTo(model.Points[5].AsWpf(), true, true);

            //corner 3
            context.ArcTo(model.Points[6].AsWpf(),
                cornerSize, 0, false, SweepDirection.Counterclockwise, true, true);

            context.ArcTo(model.Points[7].AsWpf(), new Size(InnerRadius, InnerRadius), 0,
                model.IsInnerRadiusLargeArc, SweepDirection.Clockwise, true, true);

            // corner 4
            context.ArcTo(model.Points[0].AsWpf(), cornerSize, 0,
                false, SweepDirection.Counterclockwise, true, true);
        }
    }
}
