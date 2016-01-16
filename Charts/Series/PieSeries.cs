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
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.ComponentModel;
using lvc.Shapes;
using lvc.TypeConverters;

namespace lvc
{
    public class PieSeries : Series
    {
		public static readonly DependencyProperty LabelsProperty =
			DependencyProperty.Register("Labels", typeof(IList<string>), typeof(PieSeries), new PropertyMetadata(null));
        [TypeConverter(typeof(StringCollectionConverter))]
		public IList<string> Labels
		{
			get { return (IList<string>)GetValue(LabelsProperty); }
			set { SetValue(LabelsProperty, value); }
		}

        public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register(
            "Colors", typeof (Color[]), typeof (PieSeries), new PropertyMetadata(default(Color[])));
        [TypeConverter(typeof(ColorCollectionConverter))]
        public Color[] Colors
        {
            get { return (Color[]) GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        public override void Plot(bool animate = true)
        {
            var pChart = Chart as PieChart;
            if (pChart == null) return;
            if (pChart.PieTotalSum <= 0) return;
            var rotated = 0d;

            Chart.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var minDimension = Chart.DesiredSize.Width < Chart.DesiredSize.Height 
                ? Chart.DesiredSize.Width : Chart.DesiredSize.Height;
            minDimension -= pChart.DrawPadding;
            minDimension = minDimension < pChart.DrawPadding ? pChart.DrawPadding : minDimension;

            var sliceId = 0;
            var isFist = true;
            foreach (var point in Values.Points)
            {
                var participation = point.Y / pChart.PieTotalSum;
                if (isFist)
                {
                    rotated = participation * -.5;
                    isFist = false;
                }
                var slice = new PieSlice
                {
                    CentreX = 0,
                    CentreY = 0,
                    RotationAngle = 360 * rotated,
                    WedgeAngle = 360 * participation,
                    Radius = minDimension / 2,
                    InnerRadius = pChart.InnerRadius,
                    Fill = new SolidColorBrush
                    {
                        Color = Colors != null && Colors.Length > sliceId
                            ? Colors[sliceId]
                            : GetColorByIndex(sliceId),
                        Opacity = 1
                    },
                    Stroke = Chart.Background,
                    StrokeThickness = pChart.SlicePadding
                };
                var wa = new DoubleAnimation
                {
                    From = 0,
                    To = slice.WedgeAngle,
                    Duration = TimeSpan.FromMilliseconds(300)
                };
                var ra = new DoubleAnimation
                {
                    From = 0,
                    To = slice.RotationAngle,
                    Duration = TimeSpan.FromMilliseconds(300)
                };

                Canvas.SetTop(slice, Chart.ActualHeight / 2);
                Canvas.SetLeft(slice, Chart.ActualWidth / 2);

                Chart.Canvas.Children.Add(slice);
                Shapes.Add(slice);

                var valueBlock = new TextBlock
                {
                    Text = Chart.AxisX.LabelFormatter == null
                        ? point.Y.ToString(CultureInfo.InvariantCulture)
                        : Chart.AxisX.LabelFormatter(point.Y),
                    FontFamily = Chart.AxisX.FontFamily,
                    FontSize = Chart.AxisX.FontSize,
                    FontStretch = Chart.AxisX.FontStretch,
                    FontStyle = Chart.AxisX.FontStyle,
                    FontWeight = Chart.AxisX.FontWeight,
                    Foreground = Brushes.White
                };

                var hypo = ((minDimension / 2) + (pChart.InnerRadius > 10 ? pChart.InnerRadius : 10)) / 2;
                var gamma = participation * 360 / 2 + rotated * 360;
                var cp = new Point(hypo * Math.Sin(gamma * (Math.PI / 180)), hypo * Math.Cos(gamma * (Math.PI / 180)));

                valueBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                Canvas.SetTop(valueBlock, Chart.ActualHeight / 2 - cp.Y - valueBlock.DesiredSize.Height * .5);
                Canvas.SetLeft(valueBlock, cp.X + Chart.ActualWidth / 2 - valueBlock.DesiredSize.Width * .5);
                Panel.SetZIndex(valueBlock, int.MaxValue);
                //because math is kind of complex to detetrmine if label fits inside the slide, by now we 
                //will just add it if participation > 5% ToDo: the math!
                if (participation > .05 && Chart.AxisX.PrintLabels)
                {
                    Chart.Canvas.Children.Add(valueBlock);
                    Chart.Shapes.Add(valueBlock);
                }

                if (!Chart.DisableAnimation)
                {
                    if (animate)
                    {
                        slice.BeginAnimation(PieSlice.WedgeAngleProperty, wa);
                        slice.BeginAnimation(PieSlice.RotationAngleProperty, ra);
                    }
                }

                if (Chart.Hoverable)
                {
                    slice.MouseEnter += Chart.DataMouseEnter;
                    slice.MouseLeave += Chart.DataMouseLeave;
                    Chart.HoverableShapes.Add(new HoverableShape
                    {
                        Series = this,
                        Shape = slice,
                        Target = slice,
                        Value = new Point(0, point.Y),
                        Label = Labels != null && Labels.Count > point.X ? Labels[(int) point.X] : ""
                    });
                }

                sliceId++;
                rotated += participation;
            }
        }
    }
}
