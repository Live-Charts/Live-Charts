using System.Windows;
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

        /// <inheritdoc />
        public IPlaneLabelControl Label { get; protected set; }

        public void Move(
            Point point1, Point point2, AxisLabelModel labelModel, 
            bool disposeWhenFinished, Core.Styles.Style style, Plane plane,
            IChartView chart)
        {
            var isNew = Line == null;
            var isNewLabel = Label == null;
            var speed = chart.AnimationsSpeed;

            if (isNew)
            {
                var wpfChart = (CartesianChart) chart;
                Line = new Line();
                Panel.SetZIndex(Line, -1);
                SetInitialLineParams(plane, point1, point2, chart);
                wpfChart.DrawArea.Children.Add(Line);
                Line.AsStoryboardTarget()
                    .AtSpeed(speed)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
            }

            if (isNewLabel)
            {
                var wpfChart = (CartesianChart) chart;
                Label = new TLabel();
                SetInitialLabelParams();
                wpfChart.DrawArea.Children.Add((UIElement) Label);
                ((UIElement) Label).AsStoryboardTarget()
                    .AtSpeed(speed)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
            }

            Line.Stroke = style.Stroke.AsSolidColorBrush();
            Line.StrokeThickness = style.StrokeThickness;
            Label.Measure(labelModel.Content);

            var actualLabelLocation = labelModel.Location + labelModel.Offset;

            var storyboard = Line.AsStoryboardTarget()
                .AtSpeed(speed)
                .Property(Line.X1Property, point1.X)
                .Property(Line.X2Property, point2.X)
                .Property(Line.Y1Property, point1.Y)
                .Property(Line.Y2Property, point2.Y)
                .SetTarget((UIElement) Label)
                .Property(Canvas.LeftProperty, actualLabelLocation.X)
                .Property(Canvas.TopProperty, actualLabelLocation.Y);

            if (disposeWhenFinished)
            {
                storyboard.Property(UIElement.OpacityProperty, 0)
                    .Then((sender, args) =>
                    {
                        ((IResource) this).Dispose(chart);
                        storyboard = null;
                    });
            }

            storyboard.Begin();
        }

        private void SetInitialLineParams(Plane plane, Point point1, Point point2, IChartView chart)
        {
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
        }

        private void SetInitialLabelParams()
        {
            var uiLablel = (UIElement) Label;
            Canvas.SetTop(uiLablel, 0);
            Canvas.SetLeft(uiLablel, 0);
        }

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view)
        {
            var wpfChart = (CartesianChart) view;
            wpfChart.DrawArea.Children.Remove(Line);
            wpfChart.DrawArea.Children.Remove((UIElement) Label);
            Line = null;
        }
    }
}
