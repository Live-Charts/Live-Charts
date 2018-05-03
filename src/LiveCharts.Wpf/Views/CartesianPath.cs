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
using System.Windows.Shapes;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Wpf.Animations;
using Brush = LiveCharts.Core.Drawing.Brush;

#endregion

namespace LiveCharts.Wpf.Views
{
    public class CartesianPath : ICartesianPath
    {
        private readonly PathFigure _figure;
        protected IEnumerable<double> StrokeDashArray;
        protected double PreviousLength;
        private TimeLine _timeLine;
        private bool _isNew;

        public CartesianPath()
        {
            StrokePath = new Path();
            _figure = new PathFigure
            {
                Segments = new PathSegmentCollection()
            };
            StrokePath.Data = new PathGeometry
            {
                Figures = new PathFigureCollection(1)
                {
                    _figure
                }
            };
        }

        public Path StrokePath { get; }

        public Path FillPath => null;

        /// <inheritdoc />
        public void Initialize(IChartView view, TimeLine timeLine)
        {
            _timeLine = timeLine;
            view.Content.AddChild(StrokePath, true);
            _isNew = true;
        }

        /// <inheritdoc />
        public void SetStyle(
            PointF startPoint, Brush stroke, Brush fill,
            double strokeThickness, IEnumerable<double> strokeDashArray)
        {
            if (_isNew)
            {
                // To self drawn animation.
                _figure.StartPoint = startPoint.AsWpf();
                _isNew = false;
            }
            else
            {
                _figure.Animate(_timeLine)
                    .Property(PathFigure.StartPointProperty, _figure.StartPoint, startPoint.AsWpf())
                    .Begin();
            }

            StrokePath.Stroke = stroke.AsWpf();
            StrokePath.Fill = null;
            StrokePath.StrokeThickness = strokeThickness;
            StrokeDashArray = strokeDashArray;
            StrokePath.StrokeDashOffset = 0;
        }

        /// <inheritdoc />
        public object InsertSegment(object segment, int index, PointF p1, PointF p2, PointF p3)
        {
            var s = (BezierSegment)segment ?? new BezierSegment(p1.AsWpf(), p2.AsWpf(), p3.AsWpf(), true);

            _figure.Segments.Remove(s);
            _figure.Segments.Insert(index, s);

            return s;
        }

        public void RemoveSegment(object segment)
        {
            _figure.Segments.Remove((PathSegment)segment);
        }

        public virtual void Close(IChartView view, float length, float i, float j)
        {
            
        }

        public void Dispose(IChartView view)
        {
            view.Content.RemoveChild(StrokePath, true);
        }
    }
}
