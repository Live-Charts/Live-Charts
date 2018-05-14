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

using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Charts;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

#endregion

namespace LiveCharts.Wpf.Controls
{
    /// <inheritdoc cref="IChartContent" />
    public class ChartContent : Canvas, IChartContent
    {
        private readonly Canvas _drawMargin = new Canvas();
        private RectangleF _drawArea;
        private SizeF _controlSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartContent"/> class.
        /// </summary>
        public ChartContent()
        {
            Background = Brushes.Transparent; // otherwise mouse move is not fired...
            Children.Add(_drawMargin);
        }

        /// <inheritdoc />
        public RectangleF DrawArea
        {
            get => _drawArea;
            set
            {
                _drawArea = value;
                OnDrawAreaChanged();
            }
        }

        /// <inheritdoc />
        public SizeF ControlSize
        {
            get => _controlSize;
            set
            {
                _controlSize = value;
                OnControlSizeChanged();
            }
        }

        /// <inheritdoc />
        public void AddChild(object child, bool isClipped)
        {
            if (isClipped)
            {
                _drawMargin.Children.Add((UIElement) child);
                return;
            }

            Children.Add((UIElement) child);
        }

        /// <inheritdoc />
        public void DisposeChild(object child, bool isClipped)
        {
            if (isClipped)
            {
                _drawMargin.Children.Remove((UIElement) child);
                return;
            }

            Children.Remove((UIElement) child);
        }

        private void OnControlSizeChanged()
        {
            Width = _controlSize.Width;
            Height = _controlSize.Height;
        }

        private void OnDrawAreaChanged()
        {
            SetTop(_drawMargin, _drawArea.Top);
            SetLeft(_drawMargin, _drawArea.Left);
            _drawMargin.Width = _drawArea.Width;
            _drawMargin.Height = _drawArea.Height;
            _drawMargin.Clip = new RectangleGeometry(
                new Rect(new Point(0d, 0d),
                new Size(_drawArea.Width, _drawArea.Height)));
        }
    }
}