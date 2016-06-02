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
using LiveCharts.Wpf.Charts.Chart;

//ToDo 
//Practically everthing ins this calss should not be here, this should be in the core of the library!
//here only the renderingm and animations, never a math operation!

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
                    ? axis.LabelsReference - TextBlock.ActualWidth + leftM
                    : axis.LabelsReference - leftM);
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
                        ? axis.LabelsReference - top
                        : axis.LabelsReference);
            }
        }

        private void Remove(IChartView chart, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations)
            {
                chart.RemoveFromView(TextBlock);
                chart.RemoveFromView(Line);
                return;
            }

            if (TextBlock.Visibility == Visibility.Collapsed &&
                Line.Visibility == Visibility.Collapsed) return;

            var anim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = chart.AnimationsSpeed
            };

            anim.Completed += (sender, args) =>
            {
                if (Application.Current == null)
                {
                    chart.RemoveFromView(TextBlock);
                    chart.RemoveFromView(Line);
                    return;
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    chart.RemoveFromView(TextBlock);
                    chart.RemoveFromView(Line);
                }));
            };

            TextBlock.BeginAnimation(UIElement.OpacityProperty, anim);
            Line.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, 0, chart.AnimationsSpeed));
        }

        private void FadeIn(IChartView chart, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations) return;

            if (TextBlock.Visibility != Visibility.Collapsed)
                TextBlock.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, chart.AnimationsSpeed));

            if (Line.Visibility != Visibility.Collapsed)
                Line.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(1000)));
        }

        private void MoveFromCurrentAx(IChartView chart, AxisTags direction, int axisPosition, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations) return;

            var i = ChartFunctions.ToPlotArea(Model.Value, direction, chart.Model, axisPosition);

            var uw = new CorePoint(
                axis.Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.X, chart.Model, axis.Model) / 2
                    : 0,
                axis.Model.EvaluatesUnitWidth
                    ? ChartFunctions.GetUnitWidth(AxisTags.Y, chart.Model, axis.Model) / 2
                    : 0);

            if (direction == AxisTags.Y)
            {
                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, chart.Model.DrawMargin.Left, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, chart.Model.DrawMargin.Left + chart.Model.DrawMargin.Width,
                        chart.AnimationsSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(Line.Y1, i, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, i, chart.AnimationsSpeed));

                var hh = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > chart.Model.DrawMargin.Top + chart.Model.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight*.5;

                TextBlock.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(Line.Y1 - hh - uw.Y, i - hh - uw.Y,
                        chart.AnimationsSpeed));
            }
            else
            {
                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, i, chart.AnimationsSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, i, chart.AnimationsSpeed));
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
                    ? axis.Model.FromPreviousAxisState(Model.Value, direction, chart.Model)
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
                var x = Model.IsNew ? axis.Model.FromPreviousAxisState(Model.Value, direction, chart.Model) : Line.X1;

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

        public CoreSize UpdateLabel(string text, AxisCore axis)
        {
            TextBlock.Text = text;
            TextBlock.UpdateLayout();

            var alpha = axis.View.LabelsRotation;
            alpha *= Math.PI / 180;

            return new CoreSize(Math.Cos(alpha)*TextBlock.ActualWidth,
                Math.Sin(alpha)*TextBlock.ActualWidth +
                Math.Sin((90*(Math.PI/180)) - alpha)*TextBlock.ActualHeight);
        }

        public void UpdateLine(AxisTags source, ChartCore chart, int axisIndex, AxisCore axis)
        {
            var wpfChart = chart.View as Chart;
            var wpfAxis = axis.View as Axis;
            if (wpfChart == null || wpfAxis == null) return;

            switch (Model.State)
            {
                case SeparationState.Remove:
                    if (!chart.View.DisableAnimations && !axis.DisableAnimations)
                        MoveFromCurrentAx(wpfChart, source, axisIndex, wpfAxis);
                    Remove(wpfChart, wpfAxis);
                    break;
                case SeparationState.Keep:
                    UnanimatedPlace(wpfChart, source, axisIndex, wpfAxis);
                    if (!chart.View.DisableAnimations && !axis.DisableAnimations)
                        MoveFromPreviousAx(wpfChart, source, axisIndex, wpfAxis);
                    if (Model.IsNew) FadeIn(wpfChart, wpfAxis);
                    break;
                case SeparationState.InitialAdd:
                    UnanimatedPlace(wpfChart, source, axisIndex, wpfAxis);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
