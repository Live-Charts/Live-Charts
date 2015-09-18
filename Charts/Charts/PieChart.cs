using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Charts.Series;
using Charts.Shapes;

namespace Charts.Charts
{
    public class PieChart : Chart
    {
        private double _pieSum;
        private int _pointsCount;

        public PieChart()
        {
            PrimaryAxis = new Axis();
            SecondaryAxis = new Axis();
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            InnerRadius = 0;
            SlicePadding = 5;
            Background = Brushes.White;
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof(double), typeof(PieChart));
        /// <summary>
        /// Gets or sets chart inner radius.
        /// </summary>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty SlicePaddingProperty = DependencyProperty.Register(
            "SlicePadding", typeof(double), typeof(Chart));
        /// <summary>
        /// Gets or sets padding between slices.
        /// </summary>
        public double SlicePadding
        {
            get { return (double)GetValue(SlicePaddingProperty); }
            set { SetValue(SlicePaddingProperty, value); }
        }

        public double PieSum => _pieSum;

        protected override bool ScaleChanged
        {
            get
            {
                var serie = Series.FirstOrDefault();
                var pieSerie = serie as PieSerie;
                var min = pieSerie?.PrimaryValues.DefaultIfEmpty(0).Min() ?? 0.01;
                var psc = pieSerie?.PrimaryValues?.Count ?? 0;
                return Math.Abs(GetPieSum() - PieSum) > .001*min || _pointsCount != psc;
            }
        }

        protected override void Scale()
        {
            var serie = Series.FirstOrDefault();
            var pieSerie = serie as PieSerie;
            if (pieSerie == null) return;
            _pointsCount = pieSerie.PrimaryValues.Count;
            _pieSum = GetPieSum();
        }

        protected override void DrawAxis()
        {
            //we dont need axes for a pie chart.
            //we override it with an empty void.
        }
        
        public override void OnDataMouseEnter(object sender, MouseEventArgs e)
        {
            var b = new Border
            {
                BorderThickness = new Thickness(0),
                Background = new SolidColorBrush { Color = Color.FromRgb(30, 30, 30), Opacity = .8 },
                CornerRadius = new CornerRadius(1)
            };
            var sp = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            var pieSlice = senderShape?.Shape as PieSlice;
            if (pieSlice == null) return;

            pieSlice.Opacity = .8;

            sp.Children.Add(new TextBlock
            {
                Text = senderShape.Label + ", " + (SecondaryAxis.LabelFormatter == null
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

            var minDimension = DesiredSize.Width < DesiredSize.Height
               ? DesiredSize.Width : DesiredSize.Height;

            Canvas.SetLeft(b, (DesiredSize.Width-minDimension)/2 +10);
            Canvas.SetTop(b, (DesiredSize.Height - minDimension) / 2 +10);

            var anim = new DoubleAnimation
            {
                To = 5,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            pieSlice.BeginAnimation(PieSlice.PushOutProperty, anim);

            CurrentToolTip = b;
        }

        public override void OnDataMouseLeave(object sender, MouseEventArgs e)
        {
            base.OnDataMouseLeave(sender, e);
            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            var pieSlice = senderShape?.Shape as PieSlice;
            if (pieSlice == null) return;
            pieSlice.Opacity = 1;

            var anim = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            pieSlice.BeginAnimation(PieSlice.PushOutProperty, anim);
        }

        private double GetPieSum()
        {
            var serie = Series.FirstOrDefault();
            var pieSerie = serie as PieSerie;
            return pieSerie?.PrimaryValues.DefaultIfEmpty(0).Sum() ?? 0;
        }
    }
}