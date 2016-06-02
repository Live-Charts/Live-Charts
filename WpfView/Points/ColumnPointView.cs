//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;

namespace LiveCharts.Wpf.Points
{
    internal class ColumnPointView : PointView, IRectanglePointView
    {
        public Rectangle Rectangle { get; set; }
        public CoreRectangle Data { get; set; }
        public double ZeroReference  { get; set; }
        public bool LabelInside { get; set; }
        public RotateTransform RotateTransform { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            if (IsNew)
            {
                Canvas.SetTop(Rectangle, ZeroReference);
                Canvas.SetLeft(Rectangle, Data.Left);

                Rectangle.Width = Data.Width;
                Rectangle.Height = 0;

                if (DataLabel != null)
                {
                    Canvas.SetTop(DataLabel, ZeroReference);
                    Canvas.SetLeft(DataLabel, current.ChartLocation.X);
                }
            }
          
            Func<double> getY = () =>
            {
                double y;

                if (LabelInside)
                {
                    if (RotateTransform == null)
                        RotateTransform = new RotateTransform(90);

                    DataLabel.RenderTransform = RotateTransform;

                    y = Data.Top + Data.Height/2 - DataLabel.ActualWidth*.5;
                }
                else
                {
                    if (ZeroReference > Data.Top)
                    {
                        y = Data.Top - DataLabel.ActualHeight;
                        if (y < 0) y = Data.Top;
                    }
                    else
                    {
                        y = Data.Top + Data.Height;
                        if (y + DataLabel.ActualHeight > chart.DrawMargin.Height) y -= DataLabel.ActualHeight;
                    }
                }

                return y;
            };

            Func<double> getX = () =>
            {
                double x;

                if (LabelInside)
                {
                    x = Data.Left + DataLabel.ActualHeight/2 + Data.Width/2;
                }
                else
                {
                    x = Data.Left + Data.Width / 2 - DataLabel.ActualWidth / 2;
                    if (x < 0)
                        x = 2;
                    if (x + DataLabel.ActualWidth > chart.DrawMargin.Width)
                        x -= x + DataLabel.ActualWidth - chart.DrawMargin.Width + 2;
                }

                return x;
            };

            if (chart.View.DisableAnimations)
            {
                Rectangle.Width = Data.Width;
                Rectangle.Height = Data.Height;

                Canvas.SetTop(Rectangle, Data.Top);
                Canvas.SetLeft(Rectangle, Data.Left);

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();

                    Canvas.SetTop(DataLabel, getY());
                    Canvas.SetLeft(DataLabel, getX());
                }

                if (HoverShape != null)
                {
                    Canvas.SetTop(HoverShape, Data.Top);
                    Canvas.SetLeft(HoverShape, Data.Left);
                    HoverShape.Height = Data.Height;
                    HoverShape.Width = Data.Width;
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();

                DataLabel.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(getX(), animSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(getY(), animSpeed));
            }

            Canvas.SetLeft(Rectangle, Data.Left);
            Rectangle.BeginAnimation(Canvas.TopProperty,
                new DoubleAnimation(Data.Top, animSpeed));

            Rectangle.Width = Data.Width;
            Rectangle.BeginAnimation(FrameworkElement.HeightProperty,
                new DoubleAnimation(Data.Height, animSpeed));

            if (HoverShape != null)
            {
                Canvas.SetTop(HoverShape, Data.Top);
                Canvas.SetLeft(HoverShape, Data.Left);
                HoverShape.Height = Data.Height;
                HoverShape.Width = Data.Width;
            }
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Rectangle);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }

        public override void OnHover(ChartPoint point)
        {
            var copy = Rectangle.Fill.Clone();
            copy.Opacity -= .15;
            Rectangle.Fill = copy;
        }

        public override void OnHoverLeave(ChartPoint point)
        {
            BindingOperations.SetBinding(Rectangle, Shape.FillProperty,
                new Binding
                {
                    Path = new PropertyPath(Series.Series.FillProperty),
                    Source = ((Series.Series) point.SeriesView)
                });
        }
    }
}
