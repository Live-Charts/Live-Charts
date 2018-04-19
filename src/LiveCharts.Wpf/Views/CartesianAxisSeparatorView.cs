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
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Events;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Wpf.Animations;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The separator class.
    /// </summary>
    /// <seealso cref="ICartesianAxisSectionView" />
    public class CartesianAxisSectionView<TLabel> : ICartesianAxisSectionView
        where TLabel : UIElement, IPlaneLabelControl, new()
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
        public IPlaneLabelControl Label { get; protected set; }

        /// <inheritdoc />
        object ICartesianAxisSectionView.VisualElement => Rectangle;

        /// <inheritdoc />
        public virtual void DrawShapes(CartesianAxisSectionArgs args)
        {
            var isNewShape = Rectangle == null;
            var speed = args.ChartView.AnimationsSpeed;

            // initialize the shape
            if (isNewShape)
            {
                Rectangle = new Rectangle();
                args.ChartView.Content.AddChild(Rectangle);
                Canvas.SetLeft(Rectangle, args.Rectangle.From.Left);
                Canvas.SetTop(Rectangle, args.Rectangle.From.Top);
                Rectangle.Width = args.Rectangle.From.Width;
                Rectangle.Height = args.Rectangle.From.Height;
                Panel.SetZIndex(Rectangle, args.ZIndex);
                
                Rectangle.Animate()
                    .AtSpeed(speed)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
            }
            
            Rectangle.Fill = args.Style?.Fill.AsWpf();
            Rectangle.Stroke = args.Style?.Stroke.AsWpf();

            var storyboard = Rectangle.Animate()
                .AtSpeed(speed)
                .Property(Canvas.TopProperty, args.Rectangle.To.Top)
                .Property(Canvas.LeftProperty, args.Rectangle.To.Left)
                .Property(FrameworkElement.HeightProperty,
                    args.Rectangle.To.Height > (args.Style?.StrokeThickness ?? 0)
                        ? args.Rectangle.To.Height
                        : args.Style?.StrokeThickness ?? 0)
                .Property(FrameworkElement.WidthProperty,
                    args.Rectangle.To.Width > (args.Style?.StrokeThickness ?? 0)
                        ? args.Rectangle.To.Width
                        : args.Style?.StrokeThickness ?? 0);

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

        /// <inheritdoc />
        public virtual void DrawLabel(CartesianAxisSectionArgs args)
        {
            var isNewLabel = Label == null;

            if (isNewLabel)
            {
                Label = new TLabel();
                args.ChartView.Content.AddChild(Label);
                Canvas.SetLeft((UIElement)Label, args.Label.Position.X);
                Canvas.SetTop((UIElement)Label, args.Label.Position.Y);
            }

            Label.MeasureAndUpdate(args.ChartView.Content, args.Plane.Font, args.Label.Content);

            
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
