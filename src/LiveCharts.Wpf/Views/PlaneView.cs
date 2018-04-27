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

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Wpf.Animations;

#endregion

namespace LiveCharts.Wpf.Views
{
    /// <summary>
    /// The separator class.
    /// </summary>
    /// <seealso cref="IPlaneSeparatorView" />
    public class PlaneView<TLabel> : IPlaneSeparatorView
        where TLabel : FrameworkElement, IMeasurableLabel, new()
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
        public virtual void DrawShape(CartesianAxisSectionArgs args, TimeLine timeLine)
        {
            var isNewShape = Rectangle == null;

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
                
                Rectangle.Animate(timeLine)
                    .Property(UIElement.OpacityProperty, 0, 1)
                    .Begin();
            }
            
            Rectangle.Fill = args.Style?.Fill.AsWpf();
            Rectangle.Stroke = args.Style?.Stroke.AsWpf();
            Rectangle.StrokeThickness = args.Style?.StrokeThickness ?? 0;
            Rectangle.StrokeDashArray = args.Style?.StrokeDashArray == null
                ? null
                : new DoubleCollection(args.Style?.StrokeDashArray.Select(x => (double) x));

            var rectangleAnimation = Rectangle.Animate(timeLine)
                .Property(Canvas.TopProperty, Canvas.GetTop(Rectangle), args.Rectangle.To.Top)
                .Property(Canvas.LeftProperty, Canvas.GetLeft(Rectangle), args.Rectangle.To.Left)
                .Property(FrameworkElement.HeightProperty,
                    Rectangle.Height,
                    args.Rectangle.To.Height > (args.Style?.StrokeThickness ?? 0)
                        ? args.Rectangle.To.Height
                        : args.Style?.StrokeThickness ?? 0)
                .Property(FrameworkElement.WidthProperty,
                    Rectangle.Width,
                    args.Rectangle.To.Width > (args.Style?.StrokeThickness ?? 0)
                        ? args.Rectangle.To.Width
                        : args.Style?.StrokeThickness ?? 0);

            if (args.Disposing)
            {
                Label.Animate(timeLine)
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Begin();
                rectangleAnimation
                    .Property(UIElement.OpacityProperty, 1, 0)
                    .Then((sender, e) =>
                    {
                        ((IPlaneSeparatorView) this).Dispose(args.ChartView);
                        rectangleAnimation = null;
                    });
            }

            rectangleAnimation.Begin();
        }

        /// <inheritdoc />
        public virtual void DrawLabel(CartesianAxisSectionArgs args, TimeLine timeLine)
        {
            var isNewLabel = Label == null;

            if (isNewLabel)
            {
                Label = new TLabel();
                args.ChartView.Content.AddChild(Label);
                Canvas.SetLeft(Label, args.Label.Position.X);
                Canvas.SetTop(Label, args.Label.Position.Y);
            }

            Label.Measure(args.Label.Content, args.Label.LabelStyle);

            Label.Animate(timeLine)
                .Property(Canvas.LeftProperty, Canvas.GetLeft(Label), args.Label.Position.X)
                .Property(Canvas.TopProperty, Canvas.GetTop(Label), args.Label.Position.Y)
                .Begin();
        }

        void IPlaneSeparatorView.Dispose(IChartView view)
        {
            view.Content.RemoveChild(Rectangle);
            view.Content.RemoveChild(Label);
            Rectangle = null;
            Label = null;
        }

        /// <inheritdoc />
        object IPlaneSeparatorView.VisualElement => Rectangle;

        /// <inheritdoc />
        IMeasurableLabel IPlaneSeparatorView.Label => Label;
    }
}
