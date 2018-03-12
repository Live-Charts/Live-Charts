using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Events;
using LiveCharts.Wpf.Animations;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace LiveCharts.Wpf.Views
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

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public TLabel Label { get; protected set; }

        /// <inheritdoc />
        object ICartesianAxisSeparator.VisualElement => Label;

        public void Move(CartesianAxisSeparatorArgs args)
        {
            var isNewShape = Rectangle == null;
            var isNewLabel = Label == null;
            var speed = args.ChartView.AnimationsSpeed;

            // initialize the shape
            if (isNewShape)
            {
                Rectangle = new Rectangle();
                args.ChartView.Content.AddChild(Rectangle);
                Canvas.SetLeft(Rectangle, args.From.Left);
                Canvas.SetTop(Rectangle, args.From.Top);
                Rectangle.Width = args.From.Width;
                Rectangle.Height = args.From.Height;
                Panel.SetZIndex(Rectangle, -1);
                
                Rectangle.Animate()
                    .AtSpeed(speed)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
            }

            if (isNewLabel)
            {
                Label = new TLabel();
                args.ChartView.Content.AddChild(Label);
                Canvas.SetLeft(Label, 0d);
                Canvas.SetTop(Label, 0d);

                Label.Animate()
                    .AtSpeed(speed)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
            }
            
            var st = double.IsNaN(args.Style.StrokeThickness) ? 0 : args.Style.StrokeThickness;

            Rectangle.Fill = args.Style.Fill.AsWpf();
            Rectangle.Stroke = args.Style.Stroke.AsWpf();

            Label.Measure(args.AxisLabelViewModel.Content);

            var actualLabelLocation = args.AxisLabelViewModel.Location + args.AxisLabelViewModel.Offset;

            var storyboard = Rectangle.Animate()
                .AtSpeed(speed)
                .Property(Canvas.TopProperty, args.To.Top)
                .Property(Canvas.LeftProperty, args.To.Left)
                .Property(FrameworkElement.HeightProperty, args.To.Height > st
                    ? args.To.Height
                    : st)
                .Property(FrameworkElement.WidthProperty, args.To.Width > st
                    ? args.To.Width
                    : st)
                .SetTarget(Label)
                .Property(Canvas.LeftProperty, actualLabelLocation.X)
                .Property(Canvas.TopProperty, actualLabelLocation.Y);

            if (args.Disposing)
            {
                storyboard.Property(UIElement.OpacityProperty, 0)
                    .Then((sender, e) =>
                    {
                        ((IResource) this).Dispose(args.ChartView);
                        storyboard = null;
                    });
            }

            storyboard.Begin();
        }

        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView chart)
        {
            chart.Content.RemoveChild(Rectangle);
            chart.Content.RemoveChild(Label);
            Rectangle = null;
            Disposed?.Invoke(chart, this);
        }
    }
}
