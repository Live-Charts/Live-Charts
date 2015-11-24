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
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Charts;
using LiveCharts.Shapes;

namespace LiveCharts
{
    public class PieChart : Chart
    {
        private int _pointsCount;

        public PieChart()
        {
            PrimaryAxis = new Axis { FontWeight = FontWeights.Bold, FontSize = 11, FontFamily = new FontFamily("Calibri")};
            SecondaryAxis = new Axis();
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            InnerRadius = 0;
            SlicePadding = 5;
            DrawPadding = 20;
            Background = Brushes.White;
            AnimatesNewPoints = true;
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

        public double PieTotalSum { get; private set; }

        public double DrawPadding { get; set; }

        protected override bool ScaleChanged
        {
            get
            {
                var serie = Series.FirstOrDefault();
                var pieSerie = serie as PieSerie;
                var min = pieSerie?.PrimaryValues.DefaultIfEmpty(0).Min() ?? 0.01;
                var psc = pieSerie?.PrimaryValues?.Count ?? 0;
                return Math.Abs(GetPieSum() - PieTotalSum) > .001*min || _pointsCount != psc;
            }
        }

        protected override void Scale()
        {
            DrawAxis();
            var serie = Series.FirstOrDefault();
            var pieSerie = serie as PieSerie;
            if (pieSerie == null) return;
            _pointsCount = pieSerie.PrimaryValues.Count;
            PieTotalSum = GetPieSum();
        }

        protected override void DrawAxis()
        {
            foreach (var l in AxisLabels) Canvas.Children.Remove(l);
            foreach (var s in AxisShapes) Canvas.Children.Remove(s);
            AxisLabels.Clear();
            AxisShapes.Clear();
        }
        
        public override void DataMouseEnter(object sender, MouseEventArgs e)
        {
            //This code is maybe going to be removed. I think Pie charts do not need hover, they work better 
            //if we print just values on charts

            //var b = new Border
            //{
            //    BorderThickness = new Thickness(0),
            //    Background = new SolidColorBrush { Color = Color.FromRgb(30, 30, 30), Opacity = .8 },
            //    CornerRadius = new CornerRadius(1)
            //};
            //var sp = new StackPanel
            //{
            //    Orientation = Orientation.Vertical
            //};

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            var pieSlice = senderShape?.Shape as PieSlice;
            if (pieSlice == null) return;

            pieSlice.Opacity = .8;

            //sp.Children.Add(new TextBlock
            //{
            //    Text = senderShape.Label + ", " + (SecondaryAxis.LabelFormatter == null
            //        ? senderShape.Value.Y.ToString(CultureInfo.InvariantCulture)
            //        : SecondaryAxis.LabelFormatter(senderShape.Value.Y)),
            //    Margin = new Thickness(5),
            //    VerticalAlignment = VerticalAlignment.Center,
            //    FontFamily = new FontFamily("Calibri"),
            //    FontSize = 11,
            //    Foreground = Brushes.White
            //});

            //b.Child = sp;
            //Canvas.Children.Add(b);

            //var minDimension = DesiredSize.Width < DesiredSize.Height
            //   ? DesiredSize.Width : DesiredSize.Height;

            //Canvas.SetLeft(b, (DesiredSize.Width-minDimension)/2 +10);
            //Canvas.SetTop(b, (DesiredSize.Height - minDimension) / 2 +10);

            var anim = new DoubleAnimation
            {
                To = 5,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            pieSlice.BeginAnimation(PieSlice.PushOutProperty, anim);

            //CurrentToolTip = b;
        }

        public override void DataMouseLeave(object sender, MouseEventArgs e)
        {
            base.DataMouseLeave(sender, e);
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