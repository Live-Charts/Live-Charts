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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.ComponentModel;
using LiveCharts.CoreComponents;
using LiveCharts.Shapes;
using LiveCharts.TypeConverters;

namespace LiveCharts
{
    public class PieSeries : Series
    {
        public PieSeries()
        {
            SetValue(StrokeProperty, new SolidColorBrush(Colors.White));
        }

        public static readonly DependencyProperty LabelsProperty =
            DependencyProperty.Register("Labels", typeof (IList<string>), typeof (PieSeries), new PropertyMetadata(null));

        [TypeConverter(typeof (StringCollectionConverter))]
        public IList<string> Labels
        {
            get { return (IList<string>) GetValue(LabelsProperty); }
            set { SetValue(LabelsProperty, value); }
        }

        public static readonly DependencyProperty BrushesProperty = DependencyProperty.Register(
            "Brushes", typeof (Brush[]), typeof (PieSeries), new PropertyMetadata(default(Brush[])));

        [TypeConverter(typeof (ColorCollectionConverter))]
        public Brush[] Brushes
        {
            get { return (Brush[]) GetValue(BrushesProperty); }
            set { SetValue(BrushesProperty, value); }
        }

        public override void Plot(bool animate = true)
        {
            if (Visibility != Visibility.Visible) return;
            var pChart = Chart as PieChart;
            if (pChart == null) return;
            if (pChart.PieTotalSum <= 0) return;
            var rotated = 0d;

            Chart.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var minDimension = Chart.PlotArea.Width < Chart.PlotArea.Height
                ? Chart.PlotArea.Width
                : Chart.PlotArea.Height;
            minDimension -= pChart.DrawPadding;
            minDimension = minDimension < pChart.DrawPadding ? pChart.DrawPadding : minDimension;

            var sliceId = 0;
            var isFist = true;
            foreach (var point in Values.Points)
            {
                var participation = point.Y/pChart.PieTotalSum;
                if (isFist)
                {
                    rotated = participation*-.5;
                    isFist = false;
                }

                var slice = new PieSlice
                {
                    CentreX = 0,
                    CentreY = 0,
                    RotationAngle = 360*rotated,
                    WedgeAngle = 360*participation,
                    Radius = minDimension/2,
                    InnerRadius = pChart.InnerRadius,
                    Fill = Brushes != null && Brushes.Length > sliceId
                        ? Brushes[sliceId]
                        : new SolidColorBrush(GetColorByIndex(sliceId)),
                    Stroke = Stroke,
                    StrokeThickness = pChart.SlicePadding
                };
                var wa = new DoubleAnimation
                {
                    From = 0,
                    To = slice.WedgeAngle,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                var ra = new DoubleAnimation
                {
                    From = 0,
                    To = slice.RotationAngle,
                    Duration = TimeSpan.FromMilliseconds(500)
                };

                Canvas.SetTop(slice, Chart.PlotArea.Height/2);
                Canvas.SetLeft(slice, Chart.PlotArea.Width/2);

                Chart.Canvas.Children.Add(slice);
                Shapes.Add(slice);

                var valueBlock = new TextBlock
                {
                    Text = Chart.AxisY.LabelFormatter == null
                        ? point.Y.ToString(CultureInfo.InvariantCulture)
                        : Chart.AxisY.LabelFormatter(point.Y),
                    FontFamily = Chart.AxisY.FontFamily,
                    FontSize = Chart.AxisY.FontSize,
                    FontStretch = Chart.AxisY.FontStretch,
                    FontStyle = Chart.AxisY.FontStyle,
                    FontWeight = Chart.AxisY.FontWeight,
                    Foreground = System.Windows.Media.Brushes.White
                };

                var hypo = ((minDimension/2) + (pChart.InnerRadius > 10 ? pChart.InnerRadius : 10))/2;
                var gamma = participation*360/2 + rotated*360;
                var cp = new Point(hypo*Math.Sin(gamma*(Math.PI/180)), hypo*Math.Cos(gamma*(Math.PI/180)));

                valueBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                Canvas.SetTop(valueBlock, Chart.PlotArea.Height/2 - cp.Y - valueBlock.DesiredSize.Height*.5);
                Canvas.SetLeft(valueBlock, cp.X + Chart.PlotArea.Width/2 - valueBlock.DesiredSize.Width*.5);
                Panel.SetZIndex(valueBlock, int.MaxValue - 1);
                //because math is kind of complex to detetrmine if label fits inside the slide, by now we 
                //will just add it if participation > 5% ToDo: the math!
                if (participation > .05 && Chart.AxisY.ShowLabels)
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

                slice.MouseDown += Chart.DataMouseDown;

                slice.MouseEnter += Chart.DataMouseEnter;
                slice.MouseLeave += Chart.DataMouseLeave;

                Chart.HoverableShapes.Add(new HoverableShape
                {
                    Series = this,
                    Shape = slice,
                    Target = slice,
                    Value = point,
                    Label = Labels != null && Labels.Count > point.X ? Labels[(int) point.X] : ""
                });


                sliceId++;
                rotated += participation;
            }
        }
    }
}
