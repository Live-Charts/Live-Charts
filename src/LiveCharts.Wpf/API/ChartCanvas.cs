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
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Charts;
using LiveCharts.Interaction.Controls;
using LiveCharts.Interaction.Events;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

#endregion

namespace LiveCharts.Wpf
{
    /// <inheritdoc cref="ICanvas" />
    public class ChartCanvas : Canvas, ICanvas
    {
        #region fields

        private readonly Canvas _drawMargin = new Canvas();
        private RectangleF _drawArea;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCanvas"/> class.
        /// </summary>
        public ChartCanvas(IChartView view)
        {
            View = view;
            Background = Brushes.Transparent; // otherwise mouse move is not fired...
            Children.Add(_drawMargin);

            MouseMove += NotifyCorePointerMoved;
            MouseLeftButtonDown += NotifyCorePointerDown;
            MouseLeftButtonUp += NotifyCorePointerUp;
            Loaded += NotifyCoreCanvasLoaded;

            if (view is ICartesianChartView)
            {
                MouseWheel += ZoomOnMouseWheelMove;
            }
        }

        #region Properties

        /// <inheritdoc />
        public IChartView View { get; }

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

        #endregion

        #region Events

        public event PointerHandler PointerMoved;

        public event PointerHandler PointerDown;

        public event PointerHandler PointerUp;

        public event ChartEventHandler CanvasLoaded;

        #endregion

        ///// <inheritdoc />
        //public void AddChild(object child, bool isClipped)
        //{
        //    if (isClipped)
        //    {
        //        if (child is IShape iShape)
        //        {
        //            _drawMargin.Children.Add((UIElement) iShape);
        //            return;
        //        }

        //        _drawMargin.Children.Add((UIElement) child);
        //        return;
        //    }

        //    Children.Add((UIElement) child);
        //}

        ///// <inheritdoc />
        //public void DisposeChild(object child, bool isClipped)
        //{
        //    if (isClipped)
        //    {
        //        _drawMargin.Children.Remove((UIElement) child);
        //        return;
        //    }

        //    Children.Remove((UIElement) child);
        //}

        public void Dispose()
        {
            PointerMoved = null;
            PointerDown = null;
            PointerUp = null;
            MouseMove -= NotifyCorePointerMoved;
            MouseLeftButtonDown -= NotifyCorePointerDown;
            MouseLeftButtonUp -= NotifyCorePointerUp;
            Loaded -= NotifyCoreCanvasLoaded;

            if (View is ICartesianChartView)
            {
                MouseWheel -= ZoomOnMouseWheelMove;
            }
        }

        /// <summary>
        /// Called when [mouse move].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="args">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void NotifyCorePointerMoved(object o, MouseEventArgs args)
        {
            PointerMoved?.Invoke(GetDrawAreaLocation(args), args);
        }

        /// <summary>
        /// Called when [left button down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void NotifyCorePointerDown(object sender, MouseButtonEventArgs args)
        {
            PointerDown?.Invoke(GetDrawAreaLocation(args), args);
        }

        /// <summary>
        /// Called when the pointer goes up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void NotifyCorePointerUp(object sender, MouseButtonEventArgs args)
        {
            PointerUp?.Invoke(GetDrawAreaLocation(args), args);
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void NotifyCoreCanvasLoaded(object sender, EventArgs eventArgs)
        {
            CanvasLoaded?.Invoke(View);
        }

        /// <summary>
        /// Called when [cartesian chart mouse wheel moved].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        protected virtual void ZoomOnMouseWheelMove(object sender, MouseWheelEventArgs e)
        {
            var pivot = e.GetPosition(_drawMargin);
            e.Handled = true;
            var cartesianModel = (CartesianChartModel)View.Model;
            cartesianModel.Zoom(new PointF((float)pivot.X, (float)pivot.Y), e.Delta);
        }

        private PointF GetDrawAreaLocation(MouseEventArgs args)
        {
            var p = args.GetPosition(this);
            var c = new Point(
                p.X - DrawArea.X,
                p.Y - DrawArea.Y);
            return new PointF((float)c.X, (float)c.Y);
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
