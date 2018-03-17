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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using LiveCharts.Wpf.Animations;

#endregion

namespace LiveCharts.Wpf.Views
{
    public class CartesianPath : ICartesianPath
    {
        private readonly Path _strokePath;
        private readonly PathFigure _figure;
        private IEnumerable<double> _strokeDashArray;
        private double _previousLength;
        private TimeSpan _animationsSpeed;
        private bool _isNew;

        public CartesianPath()
        {
            _strokePath = new Path();
            _figure = new PathFigure
            {
                Segments = new PathSegmentCollection()
            };
            _strokePath.Data = new PathGeometry
            {
                Figures = new PathFigureCollection(1)
                {
                    _figure
                }
            };
        }

        /// <inheritdoc />
        public void Initialize(IChartView view)
        {
            _animationsSpeed = view.AnimationsSpeed;
            view.Content.AddChild(_strokePath);
            _isNew = true;
        }

        /// <inheritdoc />
        public void SetStyle(
            PointF startPoint, System.Drawing.Color stroke, System.Drawing.Color fill, 
            double strokeThickness, IEnumerable<double> strokeDashArray)
        {
            if (_isNew)
            {
                // To self drawn animation.
                _figure.StartPoint = startPoint.AsWpf();
            }
            else
            {
                _figure.Animate()
                    .AtSpeed(_animationsSpeed)
                    .Property(PathFigure.StartPointProperty, startPoint.AsWpf())
                    .Begin();
            }

            _strokePath.Stroke = stroke.AsWpf();
            _strokePath.Fill = null;
            _strokePath.StrokeThickness = strokeThickness;
            _strokeDashArray = strokeDashArray;
            _strokePath.StrokeDashOffset = 0;
        }

        /// <inheritdoc />
        public object InsertSegment(object segment, int index, PointF p1, PointF p2, PointF p3)
        {
            var s = (BezierSegment) segment ?? new BezierSegment(p1.AsWpf(), p2.AsWpf(), p3.AsWpf(), true);

            _figure.Segments.Remove(s);
            _figure.Segments.Insert(index, s);

            return s;
        }

        public void RemoveSegment(object segment)
        {
            _figure.Segments.Remove((PathSegment) segment);
        }

        public void Close(IChartView view, double length, double i, double j)
        {
            var chart = (CartesianChart) view;

            var l = length / _strokePath.StrokeThickness;
            var tl = l - _previousLength;
            var remaining = 0d;
            if (tl < 0)
            {
                remaining = -tl;
            }

            _strokePath.StrokeDashArray = new DoubleCollection(GetAnimatedStrokeDashArray(l + remaining));
            _strokePath.BeginAnimation(
                Shape.StrokeDashOffsetProperty,
                new DoubleAnimation(tl + remaining, 0, chart.AnimationsSpeed, FillBehavior.Stop));

            _strokePath.StrokeDashOffset = 0;
            _previousLength = l;
        }

        public void Dispose(IChartView view)
        {
            view.Content.RemoveChild(_strokePath);
        }

        private IEnumerable<double> GetAnimatedStrokeDashArray(double length)
        {
            var stack = 0d;

            if (_strokeDashArray == null)
            {
                yield return length;
                yield return length;
                yield break;
            }

            var e = _strokeDashArray.GetEnumerator();
            var isStroked = true;

            while (stack < length)
            {
                if (!e.MoveNext())
                {
                    e.Reset();
                    e.MoveNext();
                }
                isStroked = !isStroked;
                yield return e.Current;
                stack += e.Current;
            }

            if (isStroked)
            {
                yield return 0;
            }
            yield return length;
            e.Dispose();
        }
    }
}
