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
       
        private void UnanimatedPlace(IChartView chart, AxisTags direction, int axisIndex, Axis axis)
        {
            var i = ChartFunctions.ToPlotArea(Value, direction, chart.Model, axisIndex);

            if (direction == AxisTags.Y)
            {
                Line.X1 = chart.Model.DrawMargin.X;
                Line.X2 = chart.Model.DrawMargin.X + chart.Model.DrawMargin.Width;
                Line.Y1 = i;
                Line.Y2 = i;

                var topM = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > chart.Model.DrawMargin.Y + chart.Model.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight * .5;
                var leftM = axis.IsMerged ? TextBlock.ActualWidth + 10 : -2;
                Canvas.SetTop(TextBlock, i - topM + axis.UnitWidth * .5);
                Canvas.SetLeft(TextBlock, axis.Position == AxisPosition.LeftBottom
                    ? axis.LabelsReference - TextBlock.ActualWidth + leftM
                    : axis.LabelsReference - leftM);
            }
            else
            {
                Line.X1 = i;
                Line.X2 = i;
                Line.Y1 = chart.Model.DrawMargin.Y;
                Line.Y2 = chart.Model.DrawMargin.Y + chart.Model.DrawMargin.Height;

                var left = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > chart.Model.DrawMargin.X + chart.Model.DrawMargin.Width
                        ? TextBlock.ActualWidth + 2
                        : -2)
                    : TextBlock.ActualWidth * .5;
                var top = axis.IsMerged ? TextBlock.ActualHeight : 0;
                Canvas.SetLeft(TextBlock, i - left + axis.UnitWidth * .5);
                Canvas.SetTop(TextBlock,
                    axis.Position == AxisPosition.LeftBottom
                        ? axis.LabelsReference - top
                        : axis.LabelsReference);
            }
        }

        private void Remove(Chart chart, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations)
            {
                chart.RemoveFromView(TextBlock);
                chart.RemoveFromView(Line);
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
                //ToDo: test this in windForms! this might throw in winforms!
                Application.Current.Dispatcher.Invoke(() =>
                {
                    chart.RemoveFromView(TextBlock);
                    chart.RemoveFromView(Line);
                });
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

        private void MoveFromCurrentAx(Chart chart, AxisTags direction, int axisPosition, Axis axis)
        {
            if (axis.DisableAnimations || chart.DisableAnimations) return;

            var i = ChartFunctions.ToPlotArea(Value, direction, chart.Model, axisPosition);

            if (direction == AxisTags.Y)
            {
                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, chart.Model.DrawMargin.X, _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, chart.Model.DrawMargin.X + chart.Model.DrawMargin.Width, _anSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(Line.Y1, i, _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, i, _anSpeed));

                var hh = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > chart.Model.DrawMargin.Y + chart.Model.DrawMargin.Height
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
                    new DoubleAnimation(Line.Y1, chart.Model.DrawMargin.Y, _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, chart.Model.DrawMargin.Y + chart.Model.DrawMargin.Height, _anSpeed));

                var hw = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > chart.Model.DrawMargin.X + chart.Model.DrawMargin.Width
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

            var i = ChartFunctions.ToPlotArea(Value, direction, chart.Model, axisIndex);

            if (direction == AxisTags.Y)
            {
                var y = IsNew
                    ? axis.Model.FromPreviousAxisState(Value, direction, chart.Model)
                    : Line.Y1;

                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(Line.X1, chart.Model.DrawMargin.X, _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(Line.X2, chart.Model.DrawMargin.X + chart.Model.DrawMargin.Width, _anSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(y, i, _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(y, i, _anSpeed));

                var hh = axis.IsMerged
                    ? (i + TextBlock.ActualHeight > chart.Model.DrawMargin.Y + chart.Model.DrawMargin.Height
                        ? +TextBlock.ActualHeight
                        : 0)
                    : TextBlock.ActualHeight * .5;
                TextBlock.BeginAnimation(Canvas.TopProperty,
                    new DoubleAnimation(y - hh + axis.UnitWidth * .5, i - hh + axis.UnitWidth * .5, _anSpeed));
            }
            else
            {
                var x = IsNew ? axis.Model.FromPreviousAxisState(Value, direction, chart.Model) : Line.X1;

                Line.BeginAnimation(Line.X1Property,
                    new DoubleAnimation(x, i, _anSpeed));
                Line.BeginAnimation(Line.X2Property,
                    new DoubleAnimation(x, i, _anSpeed));
                Line.BeginAnimation(Line.Y1Property,
                    new DoubleAnimation(Line.Y1, chart.Model.DrawMargin.Y, _anSpeed));
                Line.BeginAnimation(Line.Y2Property,
                    new DoubleAnimation(Line.Y2, chart.Model.DrawMargin.Y + chart.Model.DrawMargin.Height, _anSpeed));

                var hw = axis.IsMerged
                    ? (i + TextBlock.ActualWidth > chart.Model.DrawMargin.X + chart.Model.DrawMargin.Width
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

        public void UpdateLine(AxisTags source, IChartModel chart, int axisIndex, IAxisModel axis)
        {
            var wpfChart = chart.View as Chart;
            var wpfAxis = axis.View as Axis;
            if (wpfChart == null || wpfAxis == null) return;

            _anSpeed = wpfAxis.AnimationsSpeed ?? wpfChart.AnimationsSpeed;

            switch (State)
            {
                case SeparationState.Remove:
                    MoveFromCurrentAx(wpfChart, source, axisIndex, wpfAxis);
                    Remove(wpfChart, wpfAxis);
                    break;
                case SeparationState.Keep:
                    UnanimatedPlace(wpfChart, source, axisIndex, wpfAxis);
                    MoveFromPreviousAx(wpfChart, source, axisIndex, wpfAxis);
                    if (IsNew) FadeIn(wpfChart, wpfAxis);
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
