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

using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Animations;
using LiveCharts.Charts;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Drawing.Styles;
using LiveCharts.Interaction.Events;
#if NET45 || NET46
using Font = LiveCharts.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.Dimensions
{
    /// <summary>
    /// Defines a section in an axis.
    /// </summary>
    public class Section : IResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section()
        {
            Font = Font.Empty;
            LabelHorizontalAlignment = HorizontalAlignment.Centered;
            LabelVerticalAlignment = VerticalAlignment.Centered;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public IBrush? Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        public IEnumerable<double>? StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public IBrush? Fill { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the content of the label.
        /// </summary>
        /// <value>
        /// The content of the label.
        /// </value>
        public string LabelContent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the label vertical alignment.
        /// </summary>
        /// <value>
        /// The label vertical alignment.
        /// </value>
        public VerticalAlignment LabelVerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the label horizontal alignment.
        /// </summary>
        /// <value>
        /// The label horizontal alignment.
        /// </value>
        public HorizontalAlignment LabelHorizontalAlignment { get; set; }

        /// <summary>
        /// Gets the shape in the UI.
        /// </summary>
        public IRectangle? Shape { get; internal set; }

        /// <summary>
        /// Gets the label in the UI.
        /// </summary>
        public ILabel? Label { get; internal set; }

        internal void DrawLabel(IChartView chart, AnimatableArguments animationArgs, PointF pos)
        {
            if (Label == null)
            {
                Label = UIFactory.GetNewLabel(chart.Model);
                Label.FlushToCanvas(chart.Canvas, false);
                Label.Left = pos.X;
                Label.Top = pos.Y;
            }

            Label.Paint(Stroke, Fill);

            Label.Animate(animationArgs)
                .Property(nameof(ILabel.Left), Label.Left, pos.X)
                .Property(nameof(ILabel.Top), Label.Top, pos.Y)
                .Begin();
        }

        internal void DrawShape(
            IChartView chart, AnimatableArguments animationArgs, RectangleViewModel vm)
        {
            if (Shape == null)
            {
                Shape = UIFactory.GetNewRectangle(chart.Model);
                Shape.FlushToCanvas(chart.Canvas, true);
                Shape.Left = vm.From.Left;
                Shape.Top = vm.From.Top;
                Shape.Width = vm.From.Width;
                Shape.Height = vm.From.Height;
                Shape.ZIndex = int.MinValue + 1;

                Shape.Animate(animationArgs)
                    .Property(nameof(IShape.Opacity), 0, 1)
                    .Begin();
            }

            Shape.StrokeDashArray = StrokeDashArray;
            Shape.StrokeThickness = StrokeThickness;
            Shape.Paint(Stroke, Fill);

            Shape.Animate(animationArgs)
                .Property(nameof(IShape.Top), Shape.Top, vm.To.Top)
                .Property(nameof(IShape.Left), Shape.Left, vm.To.Left)
                .Property(nameof(IShape.Height),
                    Shape.Height,
                    vm.To.Height > StrokeThickness
                        ? vm.To.Height
                        : StrokeThickness)
                .Property(nameof(IShape.Width),
                    Shape.Width,
                    vm.To.Width > StrokeThickness
                        ? vm.To.Width
                        : StrokeThickness)
               .Begin();
        }

        #region IResource implementation

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; } = new object();

        void IResource.Dispose(IChartView view, bool force)
        {
            Disposed?.Invoke(view, this, force);
        }

        #endregion
    }
}