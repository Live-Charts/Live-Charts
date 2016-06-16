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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;

namespace LiveCharts.Wpf.Components
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
        public RotatedSize LabelModel { get; private set; }

        public SeparatorElementCore Model
        {
            get { return _model; }
        }

        private void UnanimatedPlace(IChartView chart, AxisTags direction, int axisIndex, Axis axis)
        {
            var i = ChartFunctions.ToPlotArea(Model.Value, direction, chart.Model, axisIndex);

            var uw = new CorePoint(
                axis.Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.X, chart.Model, axis.Model)/2
                    : 0,
                axis.Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.Y, chart.Model, axis.Model)/2
                    : 0);

            if (direction == AxisTags.Y)
            {
                Line.X1 = chart.Model.DrawMargin.Left;
                Line.X2 = chart.Model.DrawMargin.Left + chart.Model.DrawMargin.Width;
                Line.Y1 = i;
                Line.Y2 = i;

                var topM = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > chart.Model.DrawMargin.Top + chart.Model.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight * .5;
                var leftM = axis.IsMerged ? TextBlock.ActualWidth + 10 : -2;
                Canvas.SetTop(TextBlock, i - uw.Y - topM);
                Canvas.SetLeft(TextBlock, axis.Position == AxisPosition.LeftBottom
                    ? axis.LabelsTab - TextBlock.ActualWidth + leftM
                    : axis.LabelsTab - leftM);
            }
            else
            {
                Line.X1 = i;
                Line.X2 = i;
                Line.Y1 = chart.Model.DrawMargin.Top;
                Line.Y2 = chart.Model.DrawMargin.Top + chart.Model.DrawMargin.Height;

                var left = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > chart.Model.DrawMargin.Left + chart.Model.DrawMargin.Width
                        ? TextBlock.ActualWidth + 2
                        : -2)
                    : TextBlock.ActualWidth * .5;
                var top = axis.IsMerged ? TextBlock.ActualHeight : 0;
                Canvas.SetLeft(TextBlock, i + uw.X - left);
                Canvas.SetTop(TextBlock,
                    axis.Position == AxisPosition.LeftBottom
                        ? axis.LabelsTab - top
                        : axis.LabelsTab);
            }
        }

        private void MoveFromPreviousAx(IChartView chart, AxisTags direction, int axisIndex, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations) return;

            var i = ChartFunctions.ToPlotArea(Model.Value, direction, chart.Model, axisIndex);

            var uw = new CorePoint(
                axis.Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.X, chart.Model, axis.Model) / 2
                    : 0,
                axis.Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.Y, chart.Model, axis.Model) / 2
                    : 0);

            if (direction == AxisTags.Y)
            {
                var y = Model.IsNew
                    ? axis.Model.FromPreviousState(Model.Value, direction, chart.Model)
                    : Line.Y1;

                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, chart.Model.DrawMargin.Left, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, chart.Model.DrawMargin.Left + chart.Model.DrawMargin.Width,
                        chart.AnimationsSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(y, i, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(y, i, chart.AnimationsSpeed));

                var hh = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > chart.Model.DrawMargin.Top + chart.Model.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight*.5;

                TextBlock.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(y - hh - uw.Y, i - hh - uw.Y,
                        chart.AnimationsSpeed));
            }
            else
            {
                var x = Model.IsNew ? axis.Model.FromPreviousState(Model.Value, direction, chart.Model) : Line.X1;

                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(x, i, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(x, i, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(Line.Y1, chart.Model.DrawMargin.Top, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, chart.Model.DrawMargin.Top + chart.Model.DrawMargin.Height,
                        chart.AnimationsSpeed));

                var hw = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > chart.Model.DrawMargin.Left + chart.Model.DrawMargin.Width
                        ? TextBlock.ActualWidth + 2
                        : -2)
                    : TextBlock.ActualWidth*.5;

                TextBlock.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Line.X1 - hw + uw.X, i - hw + uw.X,
                        chart.AnimationsSpeed));
            }
        }

        public RotatedSize UpdateLabel(string text, AxisCore axis, AxisTags source)
        {
            TextBlock.Text = text;
            TextBlock.UpdateLayout();

            var transform = new RotatedSize(axis.View.LabelsRotation,
                TextBlock.ActualWidth, TextBlock.ActualHeight, axis, source);

            TextBlock.RenderTransform = Math.Abs(transform.LabelAngle) > 1
                ? new RotateTransform(transform.LabelAngle)
                : null;

            LabelModel = transform;

            return transform;
        }

        public void Clear(IChartView chart)
        {
            chart.RemoveFromView(TextBlock);
            chart.RemoveFromView(Line);

            TextBlock = null;
            Line = null;
        }      

        public void Place(ChartCore chart, AxisCore axis, AxisTags direction, int axisIndex, 
            double toLabel, double toLine)
        {
            if (direction == AxisTags.Y)
            {
                Line.X1 = chart.DrawMargin.Left;
                Line.X2 = chart.DrawMargin.Left + chart.DrawMargin.Width;
                Line.Y1 = toLine;
                Line.Y2 = toLine;

                Canvas.SetTop(TextBlock, toLabel);
                Canvas.SetLeft(TextBlock, toLabel);
            }
            else
            {
                Line.X1 = toLine;
                Line.X2 = toLine;
                Line.Y1 = chart.DrawMargin.Top;
                Line.Y2 = chart.DrawMargin.Top + chart.DrawMargin.Height;

                Canvas.SetLeft(TextBlock, toLabel);
                Canvas.SetTop(TextBlock, toLabel);
            }
        }

        public void Remove(ChartCore chart)
        {
            chart.View.RemoveFromView(TextBlock);
            chart.View.RemoveFromView(Line);
            TextBlock = null;
            Line = null;
        }

        public void Move(ChartCore chart, AxisCore axis, AxisTags direction, int axisIndex, 
            double fromLabel, double toLabel, double fromLine, double toLine)
        {
            throw new NotImplementedException();
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
