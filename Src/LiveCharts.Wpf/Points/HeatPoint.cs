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

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf.Points
{
    internal class HeatPoint : PointView, IHeatPointView
    {
        public Rectangle Rectangle { get; set; }
        public CoreColor ColorComponents { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            Canvas.SetTop(Rectangle, current.ChartLocation.Y);
            Canvas.SetLeft(Rectangle, current.ChartLocation.X);

            Rectangle.Width = Width;
            Rectangle.Height = Height;

            if (IsNew)
            {
                Rectangle.Fill = new SolidColorBrush(Colors.Transparent);
            }

            if (HoverShape != null)
            {
                HoverShape.Width = Width;
                HoverShape.Height = Height;
                Canvas.SetLeft(HoverShape, current.ChartLocation.X);
                Canvas.SetTop(HoverShape, current.ChartLocation.Y);
            }

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();
                Canvas.SetTop(DataLabel, current.ChartLocation.Y + (Height/2) - DataLabel.ActualHeight*.5);
                Canvas.SetLeft(DataLabel, current.ChartLocation.X + (Width/2) - DataLabel.ActualWidth*.5);
            }

            var targetColor = new Color
            {
                A = ColorComponents.A,
                R = ColorComponents.R,
                G = ColorComponents.G,
                B = ColorComponents.B
            };

            if (chart.View.DisableAnimations)
            {
                Rectangle.Fill = new SolidColorBrush(targetColor);
                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            Rectangle.Fill.BeginAnimation(SolidColorBrush.ColorProperty,
                new ColorAnimation(targetColor, animSpeed));
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Rectangle);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        public override void OnHover(ChartPoint point)
        {
            Rectangle.StrokeThickness++;
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            Rectangle.StrokeThickness = ((Series) point.SeriesView).StrokeThickness;
        }
    }
}
