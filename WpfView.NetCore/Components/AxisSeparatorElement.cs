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
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;
using LiveCharts.Wpf.Charts.Base;

namespace LiveCharts.Wpf.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Definitions.Charts.ISeparatorElementView" />
    public class AxisSeparatorElement : ISeparatorElementView
    {
        private readonly SeparatorElementCore _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisSeparatorElement"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public AxisSeparatorElement(SeparatorElementCore model)
        {
            _model = model;
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
        public SeparatorElementCore Model
        {
            get { return _model; }
        }

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

            var formattedText = new FormattedText(
                  TextBlock.Text,
                  CultureInfo.CurrentUICulture,
                  FlowDirection.LeftToRight,
                  new Typeface(TextBlock.FontFamily, TextBlock.FontStyle, TextBlock.FontWeight, TextBlock.FontStretch),
                  TextBlock.FontSize, Brushes.Black);

            var transform = new LabelEvaluation(axis.View.LabelsRotation,
                formattedText.Width, formattedText.Height, axis, source);

            TextBlock.RenderTransform = Math.Abs(transform.LabelAngle) > 1
                ? new RotateTransform(transform.LabelAngle)
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

        /// <summary>
        /// Fades the in.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="chart">The chart.</param>
        public void FadeIn(AxisCore axis, ChartCore chart)
        {
            if (TextBlock.Visibility != Visibility.Collapsed)
                TextBlock.BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 1, chart.View.AnimationsSpeed));

            if (Line.Visibility != Visibility.Collapsed)
                Line.BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 1, chart.View.AnimationsSpeed));
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
                dispatcher.Invoke(new Action(() =>
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
