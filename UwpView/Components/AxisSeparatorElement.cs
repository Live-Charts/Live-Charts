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
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;

namespace LiveCharts.Uwp.Components
{
    public class AxisSeparatorElement : ISeparatorElementView
    {
        private readonly SeparatorElementCore _model;

        public AxisSeparatorElement(SeparatorElementCore model)
        {
            _model = model;
        }

        internal TextBlock TextBlock { get; set; }
        internal Line Line { get; set; }
        public LabelEvaluation LabelModel { get; private set; }

        public SeparatorElementCore Model
        {
            get { return _model; }
        }
        
        public LabelEvaluation UpdateLabel(string text, AxisCore axis, AxisOrientation source)
        {
            TextBlock.Text = text;
            TextBlock.UpdateLayout();

            var transform = new LabelEvaluation(axis.View.LabelsRotation,
                TextBlock.ActualWidth, TextBlock.ActualHeight, axis, source);

            TextBlock.RenderTransform = Math.Abs(transform.LabelAngle) > 1
                ? new RotateTransform(transform.LabelAngle)
                : null;

            LabelModel = transform;

            return transform;
        }

        public void Clear(IChartView chart)
        {
#if DEBUG
            Debug.WriteLine(((Canvas)chart.GetCanvas()).Children.Count);
#endif

            chart.RemoveFromView(TextBlock);
            chart.RemoveFromView(Line);

#if DEBUG
            Debug.WriteLine(((Canvas) chart.GetCanvas()).Children.Count);
#endif

            TextBlock = null;
            Line = null;
        }      

        public void Place(ChartCore chart, AxisCore axis, AxisOrientation direction, int axisIndex, 
            double toLabel, double toLine, double tab)
        {
            if (direction == AxisOrientation.Y)
            {
                Line.X1 = chart.DrawMargin.Left;
                Line.X2 = chart.DrawMargin.Left + chart.DrawMargin.Width;
                Line.Y1 = toLine;
                Line.Y2 = toLine;

                Canvas.SetLeft(TextBlock, tab);
                Canvas.SetTop(TextBlock, toLabel);
            }
            else
            {
                Line.X1 = toLine;
                Line.X2 = toLine;
                Line.Y1 = chart.DrawMargin.Top;
                Line.Y2 = chart.DrawMargin.Top + chart.DrawMargin.Height;

                Canvas.SetLeft(TextBlock, toLabel);
                Canvas.SetTop(TextBlock, tab);
            }
        }

        public void Remove(ChartCore chart)
        {
            chart.View.RemoveFromView(TextBlock);
            chart.View.RemoveFromView(Line);
            TextBlock = null;
            Line = null;
        }

        public void Move(ChartCore chart, AxisCore axis, AxisOrientation direction, int axisIndex, double toLabel, double toLine, double tab)
        {
            if (direction == AxisOrientation.Y)
            {
                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(chart.DrawMargin.Left, chart.View.AnimationsSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(chart.DrawMargin.Left + chart.DrawMargin.Width, chart.View.AnimationsSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(toLine, chart.View.AnimationsSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(toLine, chart.View.AnimationsSpeed));

                TextBlock.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(toLabel, chart.View.AnimationsSpeed));
                TextBlock.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(tab, chart.View.AnimationsSpeed));
            }
            else
            {
                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(toLine, chart.View.AnimationsSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(toLine, chart.View.AnimationsSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(chart.DrawMargin.Top, chart.View.AnimationsSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(chart.DrawMargin.Top + chart.DrawMargin.Height, chart.View.AnimationsSpeed));

                TextBlock.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(toLabel, chart.View.AnimationsSpeed));
                TextBlock.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(tab, chart.View.AnimationsSpeed));
            }

        }

        public void FadeIn(AxisCore axis, ChartCore chart)
        {
            if (TextBlock.Visibility != Visibility.Collapsed)
                TextBlock.BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 1, chart.View.AnimationsSpeed));

            if (Line.Visibility != Visibility.Collapsed)
                Line.BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 1, chart.View.AnimationsSpeed));
        }

        public void FadeOutAndRemove(ChartCore chart)
        {
            if (TextBlock.Visibility == Visibility.Collapsed &&
                Line.Visibility == Visibility.Collapsed) return;

            var anim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = chart.View.AnimationsSpeed
            };

            anim.Completed += (sender, args) =>
            {
                if (Application.Current == null)
                {
                    chart.View.RemoveFromView(TextBlock);
                    chart.View.RemoveFromView(Line);
                    return;
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    chart.View.RemoveFromView(TextBlock);
                    chart.View.RemoveFromView(Line);
                }));
            };

            TextBlock.BeginAnimation(UIElement.OpacityProperty, anim);
            Line.BeginAnimation(UIElement.OpacityProperty,
                new DoubleAnimation(1, 0, chart.View.AnimationsSpeed));
        }
    }
}
