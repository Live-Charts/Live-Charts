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
        public CartesianPath()
        {
            StrokePath = new Path();
            StrokePathFigure = new PathFigure
            {
                Segments = new PathSegmentCollection()
            };
            StrokePath.Data = new PathGeometry
            {
                Figures = new PathFigureCollection(1)
                {
                    StrokePathFigure
                }
            };

            FillPath = new Path();
            FillPathFigure = new PathFigure
            {
                Segments = new PathSegmentCollection()
            };
            FillPath.Data = new PathGeometry
            {
                Figures = new PathFigureCollection(1)
                {
                    FillPathFigure
                }
            };
        }

        /// <summary>
        /// Gets the stroke path.
        /// </summary>
        /// <value>
        /// The stroke path.
        /// </value>
        public Path StrokePath { get; }

        /// <summary>
        /// Gets the fill path.
        /// </summary>
        /// <value>
        /// The fill path.
        /// </value>
        public Path FillPath { get; }

        protected TimeLine TimeLine { get; set; }
        protected bool IsNew { get; set; }
        protected PathFigure StrokePathFigure { get; set; }
        protected PathFigure FillPathFigure { get; set; }
        protected IEnumerable<double> StrokeDashArray { get; set; }
        protected double PreviousLength { get; set; }

        /// <inheritdoc />
        public void Initialize(IChartView view, TimeLine timeLine)
        {
            TimeLine = timeLine;
            view.Content.AddChild(StrokePath, true);
            view.Content.AddChild(FillPath, true);
            IsNew = true;
        }

        /// <inheritdoc />
        public virtual void SetStyle(
            PointF startPoint,
            PointF pivot,
            Brush stroke,
            Brush fill,
            double strokeThickness,
            int zIndex,
            IEnumerable<double> strokeDashArray)
        {
        }

        /// <inheritdoc />
        public object InsertSegment(object segment, int index, PointF p1, PointF p2, PointF p3)
        {
            var s = (BezierSegment) segment ?? new BezierSegment(p1.AsWpf(), p2.AsWpf(), p3.AsWpf(), true);

            StrokePathFigure.Segments.Remove(s);
            StrokePathFigure.Segments.Insert(index, s);

            FillPathFigure.Segments.Remove(s);
            FillPathFigure.Segments.Insert(index + 1, s);

            return s;
        }

        public void RemoveSegment(object segment)
        {
            var pathSegment = (PathSegment) segment;
            StrokePathFigure.Segments.Remove(pathSegment);
            FillPathFigure.Segments.Remove(pathSegment);
        }

        public virtual void Close(
            IChartView view,
            PointF pivot,
            float length,
            float i,
            float j)
        {
        }

        public virtual void Dispose(IChartView view)
        {
            view.Content.DisposeChild(StrokePath, true);
            view.Content.DisposeChild(FillPath, true);
        }
    }
}
