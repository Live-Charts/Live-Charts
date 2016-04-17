using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveChartsCore;
using Size = LiveChartsCore.Size;

namespace Desktop
{
    public class AxisSeparatorCache : ISeparatorCacheView
    {
        private TimeSpan _anSpeed;

        internal TextBlock TextBlock { get; set; }
        internal Line Line { get; set; }
        

        public void Place(Chart chart, AxisTags direction, int axisIndex, Axis axis)
        {
            _anSpeed = axis.AnimationsSpeed ?? chart.AnimationsSpeed;

            switch (State)
            {
                case SeparationState.Remove:
                    MoveFromCurrentAx(chart, direction, axisIndex, axis);
                    Remove(chart, axis);
                    break;
                case SeparationState.Keep:
                    UnanimatedPlace(chart, direction, axisIndex, axis);
                    MoveFromPreviousAx(chart, direction, axisIndex, axis);
                    if (IsNew) FadeIn(chart, axis);
                    break;
                case SeparationState.InitialAdd:
                    UnanimatedPlace(chart, direction, axisIndex, axis);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UnanimatedPlace(Chart chart, AxisTags direction, int axisIndex, Axis axis)
        {
            var i = chart.ToPlotArea(Value, direction, axisIndex);

            if (direction == AxisTags.Y)
            {
                Line.X1 = Canvas.GetLeft(chart.DrawMargin);
                Line.X2 = Canvas.GetLeft(chart.DrawMargin) + chart.DrawMargin.Width;
                Line.Y1 = i;
                Line.Y2 = i;

                var topM = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > Canvas.GetTop(chart.DrawMargin) + chart.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight * .5;
                var leftM = axis.IsMerged ? TextBlock.ActualWidth + 10 : -2;
                Canvas.SetTop(TextBlock, i - topM + axis.UnitWidth * .5);
                Canvas.SetLeft(TextBlock, axis.Position == CoreComponents.AxisPosition.LeftBottom
                    ? axis.LabelsReference - TextBlock.ActualWidth + leftM
                    : axis.LabelsReference - leftM);
            }
            else
            {
                Line.X1 = i;
                Line.X2 = i;
                Line.Y1 = Canvas.GetTop(chart.DrawMargin);
                Line.Y2 = Canvas.GetTop(chart.DrawMargin) + chart.DrawMargin.Height;

                var left = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > Canvas.GetLeft(chart.DrawMargin) + chart.DrawMargin.Width
                        ? TextBlock.ActualWidth + 2
                        : -2)
                    : TextBlock.ActualWidth * .5;
                var top = axis.IsMerged ? TextBlock.ActualHeight : 0;
                Canvas.SetLeft(TextBlock, i - left + axis.UnitWidth * .5);
                Canvas.SetTop(TextBlock,
                    axis.Position == CoreComponents.AxisPosition.LeftBottom
                        ? axis.LabelsReference - top
                        : axis.LabelsReference);
            }
        }

        private void Remove(Chart chart, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations)
            {
                chart.Canvas.Children.Remove(TextBlock);
                chart.Canvas.Children.Remove(Line);
                return;
            }

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
                    chart.Canvas.Children.Remove(TextBlock);
                    chart.Canvas.Children.Remove(Line);
                }));
            };

            TextBlock.BeginAnimation(UIElement.OpacityProperty, anim);
            Line.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, 0, _anSpeed));
        }

        private void FadeIn(Chart chart, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations) return;

            TextBlock.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, _anSpeed));
            Line.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(1000)));
        }

        private void MoveFromCurrentAx(Chart chart, AxisTags direction, int axisIndex, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations) return;

            var i = chart.ToPlotArea(Value, direction, axisIndex);

            if (direction == AxisTags.Y)
            {
                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, Canvas.GetLeft(chart.DrawMargin), _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, Canvas.GetLeft(chart.DrawMargin) + chart.DrawMargin.Width, _anSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(Line.Y1, i, _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, i, _anSpeed));

                var hh = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > Canvas.GetTop(chart.DrawMargin) + chart.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight * .5;
                TextBlock.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(Line.Y1 - hh + axis.UnitWidth * .5, i - hh + axis.UnitWidth * .5, _anSpeed));
            }
            else
            {
                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, i, _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, i, _anSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(Line.Y1, Canvas.GetTop(chart.DrawMargin), _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, Canvas.GetTop(chart.DrawMargin) + chart.DrawMargin.Height, _anSpeed));

                var hw = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > Canvas.GetLeft(chart.DrawMargin) + chart.DrawMargin.Width
                        ? TextBlock.ActualWidth + 2
                        : -2)
                    : TextBlock.ActualWidth * .5;
                TextBlock.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(Line.X1 - hw + axis.UnitWidth * .5, i - hw + axis.UnitWidth * .5, _anSpeed));
            }
        }

        private void MoveFromPreviousAx(Chart chart, AxisTags direction, int axisIndex, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations) return;

            var i = chart.ToPlotArea(Value, direction, axisIndex);

            if (direction == AxisTags.Y)
            {
                var y = IsNew ? axis.FromPreviousAxisState(Value, direction, chart) : Line.Y1;

                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, Canvas.GetLeft(chart.DrawMargin), _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, Canvas.GetLeft(chart.DrawMargin) + chart.DrawMargin.Width, _anSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(y, i, _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(y, i, _anSpeed));

                var hh = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > Canvas.GetTop(chart.DrawMargin) + chart.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight * .5;
                TextBlock.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(y - hh + axis.UnitWidth * .5, i - hh + axis.UnitWidth * .5, _anSpeed));
            }
            else
            {
                var x = IsNew ? axis.FromPreviousAxisState(Value, direction, chart) : Line.X1;

                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(x, i, _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(x, i, _anSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(Line.Y1, Canvas.GetTop(chart.DrawMargin), _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, Canvas.GetTop(chart.DrawMargin) + chart.DrawMargin.Height, _anSpeed));

                var hw = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > Canvas.GetLeft(chart.DrawMargin) + chart.DrawMargin.Width
                        ? TextBlock.ActualWidth + 2
                        : -2)
                    : TextBlock.ActualWidth * .5;
                TextBlock.BeginAnimation(Canvas.LeftProperty,
                    new DoubleAnimation(x - hw + axis.UnitWidth * .5, i - hw + axis.UnitWidth * .5, _anSpeed));
            }
        }

        public bool IsNew { get; set; }
        public SeparationState State { get; set; }
        public bool IsActive { get; set; }
        public double Key { get; set; }
        public double Value { get; set; }
        public Size UpdateLabel(string text)
        {
            TextBlock.Text = text;
            TextBlock.UpdateLayout();
            return new Size(TextBlock.ActualWidth, TextBlock.ActualHeight);
        }
    }
}
