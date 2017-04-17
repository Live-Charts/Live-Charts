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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Uwp.Charts.Base;

namespace LiveCharts.Uwp.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Definitions.Charts.ISeparatorElementView" />
    public class AxisSeparatorElement : ISeparatorElementView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisSeparatorElement"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public AxisSeparatorElement(SeparatorElementCore model)
        {
            Model = model;
        }

        internal TextBlock TextBlock { get; set; }
        internal Line Line { get; set; }
        /// <summary>
        /// Gets the label model.
        /// </summary>
        /// <value>
        /// The label model.
        /// </value>
        public LabelEvaluation LabelModel { get; private set; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public SeparatorElementCore Model { get; }

        /// <summary>
        /// Updates the label.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public LabelEvaluation UpdateLabel(string text, AxisCore axis, AxisOrientation source)
        {
            TextBlock.Text = text;
            TextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var transform = new LabelEvaluation(axis.View.LabelsRotation,
                TextBlock.DesiredSize.Width, TextBlock.DesiredSize.Height, axis, source);

            TextBlock.RenderTransform = Math.Abs(transform.LabelAngle) > 1
                ? new RotateTransform {  Angle = transform.LabelAngle }
                : null;

            LabelModel = transform;

            return transform;
        }

        /// <summary>
        /// Clears the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public void Clear(IChartView chart)
        {
            chart.RemoveFromView(TextBlock);
            chart.RemoveFromView(Line);
            TextBlock = null;
            Line = null;
        }

        /// <summary>
        /// Places the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="axisIndex">Index of the axis.</param>
        /// <param name="toLabel">To label.</param>
        /// <param name="toLine">To line.</param>
        /// <param name="tab">The tab.</param>
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

        /// <summary>
        /// Removes the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public void Remove(ChartCore chart)
        {
            chart.View.RemoveFromView(TextBlock);
            chart.View.RemoveFromView(Line);
            TextBlock = null;
            Line = null;
        }

        /// <summary>
        /// Moves the specified chart.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="axisIndex">Index of the axis.</param>
        /// <param name="toLabel">To label.</param>
        /// <param name="toLine">To line.</param>
        /// <param name="tab">The tab.</param>
        public void Move(ChartCore chart, AxisCore axis, AxisOrientation direction, int axisIndex, double toLabel, double toLine, double tab)
        {
            if (direction == AxisOrientation.Y)
            {
                var x1 = AnimationsHelper.CreateDouble(chart.DrawMargin.Left, chart.View.AnimationsSpeed, nameof(Line.X1));
                var x2 = AnimationsHelper.CreateDouble(chart.DrawMargin.Left + chart.DrawMargin.Width, chart.View.AnimationsSpeed, nameof(Line.X2));
                var y1 = AnimationsHelper.CreateDouble(toLine, chart.View.AnimationsSpeed, nameof(Line.Y1));
                var y2 = AnimationsHelper.CreateDouble(toLine, chart.View.AnimationsSpeed, nameof(Line.Y2));

                AnimationsHelper.CreateStoryBoardAndBegin(Line, x1, x2, y1, y2);

                var tb1 = AnimationsHelper.CreateDouble(toLabel, chart.View.AnimationsSpeed, "(Canvas.Top)");
                var tb2 = AnimationsHelper.CreateDouble(tab, chart.View.AnimationsSpeed, "(Canvas.Left)");

                AnimationsHelper.CreateStoryBoardAndBegin(TextBlock, tb1, tb2);
            }
            else
            {
                var x1 = AnimationsHelper.CreateDouble(toLine, chart.View.AnimationsSpeed, nameof(Line.X1));
                var x2 = AnimationsHelper.CreateDouble(toLine, chart.View.AnimationsSpeed, nameof(Line.X2));
                var y1 = AnimationsHelper.CreateDouble(chart.DrawMargin.Top, chart.View.AnimationsSpeed, nameof(Line.Y1));
                var y2 = AnimationsHelper.CreateDouble(chart.DrawMargin.Top + chart.DrawMargin.Height, chart.View.AnimationsSpeed, nameof(Line.Y2));

                AnimationsHelper.CreateStoryBoardAndBegin(Line, x1, x2, y1, y2);

                var tb1 = AnimationsHelper.CreateDouble(toLabel, chart.View.AnimationsSpeed, "(Canvas.Left)");
                var tb2 = AnimationsHelper.CreateDouble(tab, chart.View.AnimationsSpeed, "(Canvas.Top)");

                AnimationsHelper.CreateStoryBoardAndBegin(TextBlock, tb1, tb2);
            }

        }

        /// <summary>
        /// Fades the in.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="chart">The chart.</param>
        public void FadeIn(AxisCore axis, ChartCore chart)
        {
            if (TextBlock.Visibility != Visibility.Collapsed)
                TextBlock.BeginDoubleAnimation("Opacity", 0, 1, chart.View.AnimationsSpeed);

            if (Line.Visibility != Visibility.Collapsed)
                Line.BeginDoubleAnimation("Opacity", 0, 1, chart.View.AnimationsSpeed);
        }

        /// <summary>
        /// Fades the out and remove.
        /// </summary>
        /// <param name="chart">The chart.</param>
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

            var dispatcher = ((Chart) chart.View).Dispatcher;
            anim.Completed += (sender, args) =>
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    chart.View.RemoveFromView(TextBlock);
                    chart.View.RemoveFromView(Line);
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            };

            TextBlock.BeginAnimation(anim, "Opacity");

            Line.BeginDoubleAnimation("Opacity", 1, 0, chart.View.AnimationsSpeed);
        }
    }
}
