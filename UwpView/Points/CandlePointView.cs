//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Uwp.Components;

namespace LiveCharts.Uwp.Points
{
    internal class CandlePointView : PointView, IOhlcPointView
    {
        public Line HighToLowLine { get; set; }
        public Rectangle OpenToCloseRectangle { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Close { get; set; }
        public double Low { get; set; }
        public double Width { get; set; }
        public double Left { get; set; }
        public double StartReference { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            var center = Left + Width / 2;

            if (IsNew)
            {
                HighToLowLine.X1 = center;
                HighToLowLine.X2 = center;
                HighToLowLine.Y1 = StartReference;
                HighToLowLine.Y2 = StartReference;

                Canvas.SetTop(OpenToCloseRectangle, (Open+Close)/2);
                Canvas.SetLeft(OpenToCloseRectangle, Left);

                OpenToCloseRectangle.Width = Width;
                OpenToCloseRectangle.Height = 0;
            }

            if (DataLabel != null && double.IsNaN(Canvas.GetLeft(DataLabel)))
            {
                Canvas.SetTop(DataLabel, current.ChartLocation.Y);
                Canvas.SetLeft(DataLabel, current.ChartLocation.X);
            }

            if (HoverShape != null)
            {
                var h = Math.Abs(High - Low);
                HoverShape.Width = Width;
                HoverShape.Height = h > 10 ? h : 10;
                Canvas.SetLeft(HoverShape, Left);
                Canvas.SetTop(HoverShape, High);
            }

            if (chart.View.DisableAnimations)
            {
                HighToLowLine.Y1 = High;
                HighToLowLine.Y2 = Low;
                HighToLowLine.X1 = center;
                HighToLowLine.X2 = center;

                OpenToCloseRectangle.Width = Width;
                OpenToCloseRectangle.Height = Math.Abs(Open - Close);

                Canvas.SetTop(OpenToCloseRectangle, Math.Min(Open, Close));
                Canvas.SetLeft(OpenToCloseRectangle, Left);

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();

                    var cx = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualHeight*.5, chart);
                    var cy = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualWidth*.5, chart);
                    
                    Canvas.SetTop(DataLabel, cy);
                    Canvas.SetLeft(DataLabel, cx);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();

                var cx = CorrectXLabel(current.ChartLocation.X - DataLabel.ActualWidth*.5, chart);
                var cy = CorrectYLabel(current.ChartLocation.Y - DataLabel.ActualHeight*.5, chart);

                DataLabel.CreateCanvasStoryBoardAndBegin(cx, cy, animSpeed);
            }

            var x1Animation = AnimationsHelper.CreateDouble(center, animSpeed, "Line.X1");
            var x2Animation = AnimationsHelper.CreateDouble(center, animSpeed, "Line.X2");
            AnimationsHelper.CreateStoryBoard(HighToLowLine, x1Animation, x2Animation);

            var y1Animation = AnimationsHelper.CreateDouble(High, animSpeed, "Line.Y1");
            var y2Animation = AnimationsHelper.CreateDouble(Low, animSpeed, "Line.Y2");
            AnimationsHelper.CreateStoryBoardAndBegin(HighToLowLine, y1Animation, y2Animation);
            
            OpenToCloseRectangle.BeginDoubleAnimation("(Canvas.Left)", Left, animSpeed);
            OpenToCloseRectangle.BeginDoubleAnimation("(Canvas.Top)", Math.Min(Open, Close), animSpeed);

            OpenToCloseRectangle.BeginDoubleAnimation("Width", Width, animSpeed);
            OpenToCloseRectangle.BeginDoubleAnimation("Height", Math.Max(Math.Abs(Open - Close), OpenToCloseRectangle.StrokeThickness), animSpeed);
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(OpenToCloseRectangle);
            chart.View.RemoveFromDrawMargin(HighToLowLine);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        protected double CorrectXLabel(double desiredPosition, ChartCore chart)
        {
            if (desiredPosition + DataLabel.ActualWidth > chart.DrawMargin.Width)
                desiredPosition -= desiredPosition + DataLabel.ActualWidth - chart.DrawMargin.Width + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        protected double CorrectYLabel(double desiredPosition, ChartCore chart)
        {
            //desiredPosition -= Ellipse.ActualHeight * .5 + DataLabel.ActualHeight * .5 + 2;

            if (desiredPosition + DataLabel.ActualHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + DataLabel.ActualHeight - chart.DrawMargin.Height + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }
        
    }
}
