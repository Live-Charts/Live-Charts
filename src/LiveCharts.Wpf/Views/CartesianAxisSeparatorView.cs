#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Events;
using LiveCharts.Wpf.Framework.Animations;

#endregion

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
                Canvas.SetLeft(Rectangle, args.SeparatorFrom.Left);
                Canvas.SetTop(Rectangle, args.SeparatorFrom.Top);
                Rectangle.Width = args.SeparatorFrom.Width;
                Rectangle.Height = args.SeparatorFrom.Height;
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
                Canvas.SetLeft(Label, args.LabelFrom.X);
                Canvas.SetTop(Label, args.LabelFrom.Y);

                //Label.Animate()
                //    .AtSpeed(speed)
                //    .Property(UIElement.OpacityProperty, 1, 0)
                //    .Begin();
            }
            
            Rectangle.Fill = args.Style.Fill.AsWpf();
            Rectangle.Stroke = args.Style.Stroke.AsWpf();

            Label.MeasureAndUpdate(args.ChartView.Content, args.Plane.Font, args.LabelViewModel.Content);

            var actualLabelLocation = args.LabelViewModel.ActualLocation;

            var storyboard = Rectangle.Animate()
                .AtSpeed(speed)
                .Property(Canvas.TopProperty, args.SeparatorTo.Top)
                .Property(Canvas.LeftProperty, args.SeparatorTo.Left)
                .Property(FrameworkElement.HeightProperty,
                    args.SeparatorTo.Height > args.Style.StrokeThickness
                        ? args.SeparatorTo.Height
                        : args.Style.StrokeThickness)
                .Property(FrameworkElement.WidthProperty,
                    args.SeparatorTo.Width > args.Style.StrokeThickness
                        ? args.SeparatorTo.Width
                        : args.Style.StrokeThickness)
                .ChangeTarget(Label)
                .Property(Canvas.LeftProperty, actualLabelLocation.X)
                .Property(Canvas.TopProperty, actualLabelLocation.Y);

            if (args.Disposing)
            {
                storyboard
                    .Property(UIElement.OpacityProperty, 0)
                    .ChangeTarget(Rectangle)
                    .Property(UIElement.OpacityProperty, 0)
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
