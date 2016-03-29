using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xaml;
using LiveCharts.CoreComponents;

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

namespace LiveCharts
{
    internal class Separation
    {
        private readonly TimeSpan _anSpeed = TimeSpan.FromMilliseconds(200);

        internal TextBlock Label { get; set; }
        internal Line Line { get; set; }
        internal double Value { get; set; }
        internal SeparationAnimation State { get; set; }
        internal int AxisPosition { get; set; }

        public void Draw(Chart chart, AxisTags direction, int axisIndex)
        {
            switch (State)
            {
                case SeparationAnimation.FadeOut:
                    FadeOut(chart, direction, axisIndex);
                    break;
                case SeparationAnimation.FadeIn:
                    FadeIn(chart, direction, axisIndex);
                    break;
                case SeparationAnimation.FromLeft:
                    FromLeft(chart, direction, axisIndex);
                    break;
                case SeparationAnimation.FromRight:
                    FromRight(chart, direction, axisIndex);
                    break;
                case SeparationAnimation.None:
                    None(chart, direction, axisIndex);
                    break;
                case SeparationAnimation.Move:
                    Move(chart, direction, axisIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void None(Chart chart, AxisTags direction, int axisIndex)
        {
            var i = chart.ToPlotArea(Value, direction, axisIndex);

            Line.X1 = direction == AxisTags.Y
                ? chart.PlotArea.X
                : i;
            Line.X2 = direction == AxisTags.Y
                ? chart.PlotArea.X + chart.PlotArea.Width
                : i;

            Line.Y1 = direction == AxisTags.Y
                ? i
                : chart.PlotArea.Y;
            Line.Y2 = direction == AxisTags.Y
                ? i
                : chart.PlotArea.Y + chart.PlotArea.Height;
        }

        private void FadeOut(Chart chart, AxisTags direction, int axisIndex)
        {
            var anim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = _anSpeed
            };

            anim.Completed += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    chart.Canvas.Children.Remove(Label);
                    chart.Canvas.Children.Remove(Line);
                }));
            };

            Label.BeginAnimation(UIElement.OpacityProperty, anim);
            Line.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, 0, _anSpeed));
        }

        private void FadeIn(Chart chart, AxisTags direction, int axisIndex)
        {
            None(chart, direction, axisIndex);

            Label.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, _anSpeed));
            Line.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, _anSpeed));
        }

        private void FromLeft(Chart chart, AxisTags direction, int axisIndex)
        {
            var i = chart.ToPlotArea(Value, direction, axisIndex);

            None(chart, direction, axisIndex);

            if (direction == AxisTags.Y)
            {
                Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(0, i, _anSpeed));
                Line.BeginAnimation(Line.Y2Property, new DoubleAnimation(0, i, _anSpeed));
            }
            else
            {
                Line.BeginAnimation(Line.X1Property, new DoubleAnimation(0, i, _anSpeed));
                Line.BeginAnimation(Line.X2Property, new DoubleAnimation(0, i, _anSpeed));
            }
        }

        private void FromRight(Chart chart, AxisTags direction, int axisIndex)
        {
            var i = chart.ToPlotArea(Value, direction, axisIndex);

            None(chart, direction, axisIndex);

            if (direction == AxisTags.Y)
            {
                Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(chart.ActualHeight, i, _anSpeed));
                Line.BeginAnimation(Line.Y2Property, new DoubleAnimation(chart.ActualHeight, i, _anSpeed));
            }
            else
            {
                Line.BeginAnimation(Line.X1Property, new DoubleAnimation(chart.ActualWidth, i, _anSpeed));
                Line.BeginAnimation(Line.X2Property, new DoubleAnimation(chart.ActualWidth, i, _anSpeed));
            }
        }

        private void Move(Chart chart, AxisTags direction, int axisIndex)
        {
            var i = chart.ToPlotArea(Value, direction, axisIndex);

            if (direction == AxisTags.Y)
            {
                Line.BeginAnimation(Line.X1Property, new DoubleAnimation(Line.X1, chart.PlotArea.X, _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, chart.PlotArea.X + chart.PlotArea.Width, _anSpeed));
                Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(Line.Y1, i, _anSpeed));
                Line.BeginAnimation(Line.Y2Property, new DoubleAnimation(Line.Y2, i, _anSpeed));
            }
            else
            {
                Line.BeginAnimation(Line.X1Property, new DoubleAnimation(Line.X1, i, _anSpeed));
                Line.BeginAnimation(Line.X2Property, new DoubleAnimation(Line.X2, i, _anSpeed));
                Line.BeginAnimation(Line.Y1Property, new DoubleAnimation(Line.Y1, chart.PlotArea.Y, _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, chart.PlotArea.Y + chart.PlotArea.Height, _anSpeed));
            }
        }
    }

    internal enum SeparationAnimation
    {
        FadeOut,
        FadeIn,
        Move,
        FromLeft,
        FromRight,
        None
    }
}