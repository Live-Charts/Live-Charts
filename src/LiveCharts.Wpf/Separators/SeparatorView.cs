﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Wpf.Separators
{
    /// <summary>
    /// The separator class.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.ISeparator" />
    public class SeparatorView<TLabel> : ISeparator
        where TLabel : FrameworkElement, IPlaneLabelControl, new()
    {
        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public Line Line { get; protected set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public IPlaneLabelControl Label { get; protected set; }

        public void Move(
            Point point1, Point point2, AxisLabelModel labelModel, bool disposeWhenFinished, Plane plane, IChartView chart)
        {
            var isNew = Line == null;
            var isNewLabel = Label == null;
            var speed = chart.AnimationsSpeed;

            if (isNew)
            {
                var wpfChart = (CartesianChart)chart;
                Line = new Line();
                Panel.SetZIndex(Line, -1);
                if (plane.PlaneType == PlaneTypes.X)
                {
                    Line.X1 = point1.X;
                    Line.Y1 = 0;
                    Line.X2 = point2.X;
                    Line.Y2 = chart.Model.DrawAreaSize.Height;
                }
                else
                {
                    Line.X1 = 0;
                    Line.Y1 = point1.Y;
                    Line.X2 = chart.Model.DrawAreaSize.Width;
                    Line.Y2 = point2.Y;
                }
                wpfChart.DrawArea.Children.Add(Line);
                Line.Animate()
                    .AtSpeed(speed)
                    .Property(line => line.Opacity, 1, 0)
                    .Start();
            }

            if (isNewLabel)
            {
                var wpfChart = (CartesianChart) chart;
                Label = new TLabel();
            }

            Line.Stroke = plane.Style.Stroke.AsSolidColorBrush();
            Line.StrokeThickness = plane.Style.StrokeThickness;

            var animation = Line.Animate()
                .AtSpeed(speed)
                .Property(line => line.X1, point1.X)
                .Property(line => line.X2, point2.X)
                .Property(line => line.Y1, point1.Y)
                .Property(line => line.Y2, point2.Y);

            if (disposeWhenFinished)
            {
                animation.Property(p => p.Opacity, 0)
                    .Then((sender, args) =>
                    {
                        ((IResource) this).Dispose(chart);
                        animation = null;
                    });
            }

            animation.Start();
        }

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view)
        {
            var wpfChart = (CartesianChart)view;
            wpfChart.DrawArea.Children.Remove(Line);
            Line = null;
        }
    }
}