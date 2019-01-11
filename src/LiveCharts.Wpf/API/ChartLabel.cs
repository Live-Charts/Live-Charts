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

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction.Controls;
using FontStyle = LiveCharts.Drawing.Styles.FontStyle;
using FontWeight = LiveCharts.Drawing.Styles.FontWeight;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// a default label for a point.
    /// </summary>
    /// <seealso cref="TextBlock" />
    public class ChartLabel : TextBlock, ILabel
    {
        private string _content = string.Empty;
        private string _fontFamily = string.Empty;
        private double _fontSize;
        private Drawing.Brushes.Brush _brush;
        private Padding _padding = new Padding();
        private RotateTransform _transform = null;
        private FontStyle _fontStyle;
        private FontWeight _fontWeight;

        static ChartLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ChartLabel),
                new FrameworkPropertyMetadata(typeof(ChartLabel)));
        }

        public double Left
        {
            get => Canvas.GetLeft(this);
            set => Canvas.SetLeft(this, value);
        }

        public double Top
        {
            get => Canvas.GetTop(this);
            set => Canvas.SetTop(this, value);
        }

        public double Rotation
        {
            get => _transform?.Angle ?? 0;
            set
            {
                if (Math.Abs(value) < 0.01)
                {
                    _transform = null;
                    RenderTransform = null;
                    return;
                }

                if (_transform == null)
                {
                    _transform = new RotateTransform();
                    RenderTransform = _transform;
                }

                _transform.Angle = value;
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                Text = value;
            }
        }

        string ILabel.FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                FontFamily = new System.Windows.Media.FontFamily(value);
            }
        }

        double ILabel.FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                FontSize = value;
            }
        }

        FontStyle ILabel.FontStyle
        {
            get => _fontStyle;
            set
            {
                _fontStyle = value;
                FontStyle = value.AsWpf();
            }
        }

        FontWeight ILabel.FontWeight
        {
            get => _fontWeight;
            set
            {
                _fontWeight = value;
                FontWeight = value.AsWpf();
            }
        }

        Padding ILabel.Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                Padding = new Thickness(value.Left, value.Top, value.Right, value.Left);
            }
        }

        LiveCharts.Drawing.Brushes.Brush ILabel.Foreground { get; set; }

        public IAnimationBuilder Animate(AnimatableArguments args) => new AnimationBuilder<ChartLabel>(this, args);

        void ICanvasElement.Paint()
        {
            Foreground = ((ILabel)this).Foreground.AsWpfBrush();
        }

        SizeF ILabel.Measure()
        {
            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new SizeF((float)DesiredSize.Width, (float)DesiredSize.Height);
        }

        public void FlushToCanvas(ICanvas canvas, bool clippedToDrawMargin) => canvas.AddChild(this, clippedToDrawMargin);

        public void RemoveFromCanvas(ICanvas canvas) => canvas.RemoveChild(this);

        public void Paint()
        {
            throw new NotImplementedException();
        }
    }
}
