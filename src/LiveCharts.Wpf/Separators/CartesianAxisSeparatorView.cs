using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Events;
using LiveCharts.Wpf.Animations;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace LiveCharts.Wpf.Separators
{
    /// <summary>
    /// The separator class.
    /// </summary>
    /// <seealso cref="ICartesianAxisSeparator" />
    public class CartesianAxisSeparatorView<TLabel> : ICartesianAxisSeparator
        where TLabel : FrameworkElement, IPlaneLabelControl, new()
    {
        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public Rectangle Rectangle { get; protected set; }

        /// <inheritdoc />
        public IPlaneLabelControl Label { get; protected set; }

        public void Move(CartesianAxisSeparatorArgs args)
        {
            var wpfChart = (CartesianChart)args.ChartView;

            var isNew = Rectangle == null;
            var isNewLabel = Label == null;
            var speed = args.ChartView.AnimationsSpeed;

            if (isNew)
            {
                Rectangle = new Rectangle();
                Panel.SetZIndex(Rectangle, -1);
                SetInitialLineParams(args);
                wpfChart.DrawArea.Children.Add(Rectangle);
                Rectangle.Animate()
                    .AtSpeed(speed)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
            }

            if (isNewLabel)
            {
                Label = new TLabel();
                SetInitialLabelParams();
                wpfChart.DrawArea.Children.Add((UIElement)Label);
                ((FrameworkElement) Label).Animate()
                    .AtSpeed(speed)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
            }

            var axis = (Axis)args.Plane;
            var style = args.Plane.PlaneType == PlaneTypes.X
                ? (args.IsAlternative ? axis.XAlternativeSeparatorStyle : axis.XSeparatorStyle)
                : (args.IsAlternative ? axis.YAlternativeSeparatorStyle : axis.YSeparatorStyle);
            var st = double.IsNaN(style.StrokeThickness) ? 0 : style.StrokeThickness;

            Rectangle.Fill = style.Fill.AsWpf();
            Rectangle.Stroke = style.Stroke.AsWpf();

            Label.Measure(args.AxisLabelModel.Content);

            var actualLabelLocation = args.AxisLabelModel.Location + args.AxisLabelModel.Offset;

            var storyboard = Rectangle.Animate()
                .AtSpeed(speed)
                .Property(Canvas.TopProperty, args.Model.Top)
                .Property(Canvas.LeftProperty, args.Model.Left)
                .Property(FrameworkElement.HeightProperty, args.Model.Height > st
                    ? args.Model.Height
                    : st)
                .Property(FrameworkElement.WidthProperty, args.Model.Width > st
                    ? args.Model.Width
                    : st)
                .SetTarget((UIElement)Label)
                .Property(Canvas.LeftProperty, actualLabelLocation.X)
                .Property(Canvas.TopProperty, actualLabelLocation.Y);

            if (args.Disposing)
            {
                storyboard.Property(UIElement.OpacityProperty, 0)
                    .Then((sender, e) =>
                    {
                        ((IResource)this).Dispose(args.ChartView);
                        storyboard = null;
                    });
            }

            storyboard.Begin();
        }

        private void SetInitialLineParams(CartesianAxisSeparatorArgs args)
        {
            Canvas.SetTop(Rectangle, args.Model.Top);
            Canvas.SetLeft(Rectangle, args.Model.Left);
            Rectangle.Width = args.Model.Width;
            Rectangle.Height = args.Model.Height;
        }

        private void SetInitialLabelParams()
        {
            var uiLablel = (UIElement) Label;
            Canvas.SetTop(uiLablel, 0);
            Canvas.SetLeft(uiLablel, 0);
        }

        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view)
        {
            var wpfChart = (CartesianChart) view;
            wpfChart.DrawArea.Children.Remove(Rectangle);
            wpfChart.DrawArea.Children.Remove((UIElement)Label);
            Rectangle = null;
            Disposed?.Invoke(view, this);
        }
    }
}
