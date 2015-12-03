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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Charts;

namespace LiveCharts
{
    public class RadarChart : Chart
    {
        public double Radius;

        public RadarChart()
        {
            PrimaryAxis = new Axis();
            SecondaryAxis = new Axis();
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Dot;
            AlphaLabel = true;
            AreaOpacity = .2;
            InnerRadius = 10;
        }

        public double InnerRadius { get; set; }

        protected override bool ScaleChanged
        {
            get
            {
                if (!(GetMax() != Max))
                    return GetMin() != Min;
                return true;
            }
        }
        
        private void Measure()
        {
            Size desiredSize = DesiredSize;
            double height = desiredSize.Height;
            desiredSize = DesiredSize;
            double width = desiredSize.Width;
            Radius = height < width ? DesiredSize.Height : DesiredSize.Width;
            Radius = Radius/2.0;
            Radius = Radius - 20.0;
        }

        private Point GetMax()
        {
            var fSerie = Series.FirstOrDefault();
            if (fSerie == null) return new Point(0,0);
            var point =
                new Point(fSerie.PrimaryValues.Count,
					Series.Select(x => x.PrimaryValues.Max()).DefaultIfEmpty(0.0).Max());
            point.Y = PrimaryAxis.MaxValue ?? point.Y;
            return point;
        }

        private Point GetMin()
        {
            var point = new Point(0,
				Series.Select((x => x.PrimaryValues.Min())).DefaultIfEmpty(0.0).Min());
            point.Y = PrimaryAxis.MinValue ?? point.Y;
            return point;
        }

        private Point GetS()
        {
            double? step = this.SecondaryAxis.Separator.Step;
            double x = step ?? this.CalculateSeparator(this.Max.X - this.Min.X, AxisTags.X);
            step = this.PrimaryAxis.Separator.Step;
            double y = step ?? this.CalculateSeparator(this.Max.Y - this.Min.Y, AxisTags.Y);
            return new Point(x, y);
        }

        protected override void DrawAxis()
        {
            if (Series == null || Series.Count == 0) return;
            this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            this.Measure();
            foreach (UIElement element in this.Shapes)
                this.Canvas.Children.Remove(element);
            foreach (UIElement element in this.Shapes)
                this.Canvas.Children.Remove(element);
            this.Shapes.Clear();
            this.Shapes.Clear();
            double num1 = 0.0;
            while (num1 < 360.0)
            {
                if (this.PrimaryAxis.Separator.Enabled)
                {
                    Line line1 = new Line();
                    line1.Stroke = (Brush) new SolidColorBrush()
                    {
                        Color = this.PrimaryAxis.Separator.Color
                    };
                    line1.StrokeThickness = (double) this.PrimaryAxis.Separator.Thickness;
                    line1.X1 = this.ActualWidth/2.0;
                    line1.Y1 = this.ActualHeight/2.0;
                    line1.X2 = this.ActualWidth/2.0 + Math.Sin(num1*(Math.PI/180.0))*this.Radius;
                    line1.Y2 = this.ActualHeight/2.0 - Math.Cos(num1*(Math.PI/180.0))*this.Radius;
                    Line line2 = line1;
                    this.Canvas.Children.Add((UIElement) line2);
                    this.Shapes.Add((Shape) line2);
                }
                double num2 = num1 + 360.0/this.Max.X;
                double y = this.Max.Y;
                while (y >= this.Min.Y)
                {
                    Line line1 = new Line();
                    line1.Stroke = (Brush) new SolidColorBrush()
                    {
                        Color = this.PrimaryAxis.Separator.Color
                    };
                    line1.StrokeThickness = (double) this.PrimaryAxis.Separator.Thickness;
                    line1.X1 = this.ActualWidth/2.0 + Math.Sin(num1*(Math.PI/180.0))*this.ToChartRadius(y);
                    line1.Y1 = this.ActualHeight/2.0 - Math.Cos(num1*(Math.PI/180.0))*this.ToChartRadius(y);
                    line1.X2 = this.ActualWidth/2.0 + Math.Sin(num2*(Math.PI/180.0))*this.ToChartRadius(y);
                    line1.Y2 = this.ActualHeight/2.0 - Math.Cos(num2*(Math.PI/180.0))*this.ToChartRadius(y);
                    Line line2 = line1;
                    this.Canvas.Children.Add((UIElement) line2);
                    this.Shapes.Add((Shape) line2);
                    y -= this.S.Y;
                }
                num1 += 360.0/this.Max.X;
            }
        }

        protected override void Scale()
        {
            this.Max = this.GetMax();
            this.Min = this.GetMin();
            this.S = this.GetS();
            this.Measure();
            this.DrawAxis();
        }

        protected override Point GetToolTipPosition(HoverableShape sender, List<HoverableShape> sibilings)
        {
            Size desiredSize1 = this.DesiredSize;
            double width = desiredSize1.Width;
            desiredSize1 = this.DesiredSize;
            double height = desiredSize1.Height;
            Size desiredSize2;
            double num1;
            if (width >= height)
            {
                desiredSize2 = this.DesiredSize;
                num1 = desiredSize2.Height;
            }
            else
            {
                desiredSize2 = this.DesiredSize;
                num1 = desiredSize2.Width;
            }
            double num2 = num1;
            desiredSize2 = this.DesiredSize;
            double x = (desiredSize2.Width - num2)/2.0 + 10.0;
            desiredSize2 = this.DesiredSize;
            double y = (desiredSize2.Height - num2)/2.0 + 10.0;
            return new Point(x, y);
        }

        public double ToChartRadius(double value)
        {
            return (this.InnerRadius - this.Radius)/(this.Min.Y - this.Max.Y)*(value - this.Min.Y) + this.InnerRadius;
        }
    }
}
