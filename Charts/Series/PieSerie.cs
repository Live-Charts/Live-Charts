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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Charts;
using LiveCharts.Shapes;

namespace LiveCharts.Series
{
    public class PieSerie : Serie
    {
        public override ObservableCollection<double> PrimaryValues { get; set; }
        public string[] Labels { get; set; } 

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
            for (var index = 0; index < PrimaryValues.Count; index++)
            {
                var value = PrimaryValues[index];
                var participation = value/pChart.PieTotalSum;
                if (index == 0) rotated = participation *-.5;

                var slice = new PieSlice
                {
                    CentreX = 0,
                    CentreY = 0,
                    RotationAngle = 360 * rotated,
                    WedgeAngle = 360 * participation,
                    Radius = minDimension / 2,
                    InnerRadius = pChart.InnerRadius,
                    Fill = new SolidColorBrush {Color = GetColorByIndex(sliceId), Opacity = .8},
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

                Canvas.SetTop(slice, Chart.ActualHeight/2);
                Canvas.SetLeft(slice, Chart.ActualWidth/2);
                
                Chart.Canvas.Children.Add(slice);
                Shapes.Add(slice);

                var valueBlock = new TextBlock
                {
                    Text = Chart.PrimaryAxis.LabelFormatter == null
                        ? value.ToString(CultureInfo.InvariantCulture)
                        : Chart.PrimaryAxis.LabelFormatter(value),
                    FontFamily = Chart.PrimaryAxis.FontFamily,
                    FontSize = Chart.PrimaryAxis.FontSize,
                    FontStretch = Chart.PrimaryAxis.FontStretch,
                    FontStyle = Chart.PrimaryAxis.FontStyle,
                    FontWeight = Chart.PrimaryAxis.FontWeight,
                    Foreground = Brushes.White
                };

                var hypo = ((minDimension/2) + (pChart.InnerRadius > 10 ? pChart.InnerRadius : 10))/2;
                var gamma = participation*360/2 + rotated*360;
                var cp = new Point(hypo*Math.Sin(gamma*(Math.PI/180)), hypo*Math.Cos(gamma*(Math.PI/180)));

                valueBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                Canvas.SetTop(valueBlock, Chart.ActualHeight/2 -cp.Y - valueBlock.DesiredSize.Height*.5);
                Canvas.SetLeft(valueBlock, cp.X + Chart.ActualWidth/2 - valueBlock.DesiredSize.Width*.5);
                Panel.SetZIndex(valueBlock, int.MaxValue);
                //because math is kind of complex to detetrmine if label fits inside the slide, by now we 
                //will just add it if participation > 5% ToDo: the math!
                if (participation > .05 && Chart.PrimaryAxis.PrintLabels)
                {
                    Chart.Canvas.Children.Add(valueBlock);
                    Chart.AxisLabels.Add(valueBlock);
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
                    slice.MouseEnter += Chart.OnDataMouseEnter;
                    slice.MouseLeave += Chart.OnDataMouseLeave;
                    Chart.HoverableShapes.Add(new HoverableShape
                    {
                        Serie = this,
                        Shape = slice,
                        Target = slice,
                        Value = new Point(0, value),
                        Label = Labels.Length > index ? Labels[index] : ""
                    });
                }

                sliceId++;
                rotated += participation;
            }
        }
    }
}
