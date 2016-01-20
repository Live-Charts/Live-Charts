//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace lvc
{
	public class LineSeries : Series
	{
	    public LineSeries()
	    {
	        StrokeThickness = 2.5;
	        PointRadius = 4;
	    }

		private ILine LineChart
		{
		    get { return Chart as ILine; }
		}

	    public double StrokeThickness { get; set; }
        public double PointRadius { get; set; }

        public override void Plot(bool animate = true)
        {
            foreach (var segment in Values.Points.AsSegments())
            {
                var s = new List<Shape>();
                if (LineChart.LineType == LineChartLineType.Bezier)
                    s.AddRange(_addSerieAsBezier(segment.Select(x => new Point(
                        ToPlotArea(x.X, AxisTags.X) + Chart.XOffset, ToPlotArea(x.Y, AxisTags.Y))).ToArray(), animate));

                if (LineChart.LineType == LineChartLineType.Polyline)
                    s.AddRange(_addSeriesAsPolyline(segment.Select(x => new Point(
                        ToPlotArea(x.X, AxisTags.X) + Chart.XOffset, ToPlotArea(x.Y, AxisTags.Y))).ToArray(), animate));

                var hoverableAreas = new List<HoverableShape>();

                if (Chart.Hoverable)
                {
                    foreach (var point in segment)
                    {
                        var plotPoint = ToPlotArea(point);
                        plotPoint.X += Chart.XOffset;
                        var e = new Ellipse
                        {
                            Width = PointRadius * 2,
                            Height = PointRadius * 2,
                            Fill = Stroke,
                            Stroke = new SolidColorBrush { Color = Chart.PointHoverColor },
                            StrokeThickness = 1,
                            ClipToBounds = true
                        };
                        var r = new Rectangle
                        {
                            Fill = Brushes.Transparent,
                            Width = 40,
                            Height = 40,
                            StrokeThickness = 0
                        };

                        r.MouseEnter += Chart.DataMouseEnter;
                        r.MouseLeave += Chart.DataMouseLeave;

                        Canvas.SetLeft(r, ToPlotArea(point.X, AxisTags.X) - r.Width / 2);
                        Canvas.SetTop(r, ToPlotArea(point.Y, AxisTags.Y) - r.Height / 2);
                        Panel.SetZIndex(r, int.MaxValue);
                        Canvas.SetLeft(e, plotPoint.X - e.Width * .5);
                        Canvas.SetTop(e, plotPoint.Y - e.Height * .5);

                        Chart.Canvas.Children.Add(r);
                        Chart.Canvas.Children.Add(e);

                        s.Add(e);
                        hoverableAreas.Add(new HoverableShape
                        {
                            Series = this,
                            Shape = r,
                            Value = point,
                            Target = e
                        });
                    }
                }
                Shapes.AddRange(s);
                Chart.HoverableShapes.AddRange(hoverableAreas);
            }
		}

		private IEnumerable<Shape> _addSerieAsBezier(Point[] points, bool animate = true)
		{
            if (points.Length < 2) return Enumerable.Empty<Shape>();
			var addedFigures = new List<Shape>();

			Point[] cp1, cp2;
			BezierSpline.GetCurveControlPoints(points, out cp1, out cp2);

			var lines = new PathSegmentCollection();
			var areaLines = new PathSegmentCollection {new LineSegment(points[0], true)};
			var l = 0d;
			for (var i = 0; i < cp1.Length; ++i)
			{
				lines.Add(new BezierSegment(cp1[i], cp2[i], points[i + 1], true));
				areaLines.Add(new BezierSegment(cp1[i], cp2[i], points[i + 1], true));
				//it would be awesome to use a better formula to calculate bezier lenght
				l += Math.Sqrt(
				               Math.Pow(Math.Abs(cp1[i].X - cp2[i].X), 2) +
				               Math.Pow(Math.Abs(cp1[i].Y - cp2[i].Y), 2));
				l += Math.Sqrt(
				               Math.Pow(Math.Abs(cp2[i].X - points[i + 1].X), 2) +
				               Math.Pow(Math.Abs(cp2[i].Y - points[i + 1].Y), 2));
			}
			//aprox factor, it was calculated by aproximation.
			//the more line is curved, the more it fails.
			l = l * .65;
			areaLines.Add(new LineSegment(new Point(points.Max(x => x.X), ToPlotArea(Chart.Min.Y, AxisTags.Y)), true));
			var f = new PathFigure(points[0], lines, false);
			var fa = new PathFigure(new Point(points.Min(x => x.X), ToPlotArea(Chart.Min.Y, AxisTags.Y)), areaLines, false);
			var g = new PathGeometry(new[] {f});
			var ga = new PathGeometry(new[] {fa});

		    var path = new Path
		    {
		        Stroke = Stroke,
		        StrokeThickness = StrokeThickness,
		        Data = g,
		        StrokeEndLineCap = PenLineCap.Round,
		        StrokeStartLineCap = PenLineCap.Round,
		        StrokeDashOffset = l,
		        StrokeDashArray = new DoubleCollection {l, l},
		        ClipToBounds = true
		    };
		    var patha = new Path
		    {
		        StrokeThickness = 0,
		        Data = ga,
		        Fill = Fill,
		        ClipToBounds = true
		    };

			Chart.Canvas.Children.Add(path);
			addedFigures.Add(path);
            
		    Chart.Canvas.Children.Add(patha);
		    addedFigures.Add(patha);

		    var draw = new DoubleAnimationUsingKeyFrames
		    {
		        BeginTime = TimeSpan.FromSeconds(0),
		        KeyFrames = new DoubleKeyFrameCollection
		        {
		            new SplineDoubleKeyFrame
		            {
		                KeyTime = TimeSpan.FromMilliseconds(1),
		                Value = l
		            },
		            new SplineDoubleKeyFrame
		            {
		                KeyTime = TimeSpan.FromMilliseconds(750),
		                Value = 0
		            }
		        }
		    };

			Storyboard.SetTarget(draw, path);
			Storyboard.SetTargetProperty(draw, new PropertyPath(Shape.StrokeDashOffsetProperty));
			var sbDraw = new Storyboard();
			sbDraw.Children.Add(draw);
			var animated = false;
			if (!Chart.DisableAnimation)
			{
				if (animate)
				{
					sbDraw.Begin();
					animated = true;
				}
			}
			if (!animated) path.StrokeDashOffset = 0;
			return addedFigures;
		}

		private IEnumerable<Shape> _addSeriesAsPolyline(IList<Point> points, bool animate = true)
		{
            if (points.Count < 2) return Enumerable.Empty<Shape>();
            var addedFigures = new List<Shape>();

            var l = 0d;
			for (var i = 1; i < points.Count; i++)
			{
				var p1 = points[i - 1];
				var p2 = points[i];
				l += Math.Sqrt(
				               Math.Pow(Math.Abs(p1.X - p2.X), 2) +
				               Math.Pow(Math.Abs(p1.Y - p2.Y), 2)
					);
			}

			var f = points.First();
			var p = points.Where(x => x != f);
			var g = new PathGeometry
				{
					Figures = new PathFigureCollection(new List<PathFigure>
						{
							new PathFigure
								{
									StartPoint = f,
									Segments = new PathSegmentCollection(
						                                   p.Select(x => new LineSegment {Point = new Point(x.X, x.Y)}))
								}
						})
				};

		    var path = new Path
		    {
		        Stroke = Stroke,
		        StrokeThickness = StrokeThickness,
		        Data = g,
		        StrokeEndLineCap = PenLineCap.Round,
		        StrokeStartLineCap = PenLineCap.Round,
		        StrokeDashOffset = l,
		        StrokeDashArray = new DoubleCollection {l, l},
		        ClipToBounds = true
		    };

			var sp = points.ToList();
			sp.Add(new Point(points.Max(x => x.X), ToPlotArea(Chart.Min.Y, AxisTags.Y)));
		    var sg = new PathGeometry
		    {
		        Figures = new PathFigureCollection(new List<PathFigure>
		        {
		            new PathFigure
		            {
		                StartPoint = ToPlotArea(new Point(Chart.Min.X, Chart.Min.Y)),
		                Segments = new PathSegmentCollection(
		                    sp.Select(x => new LineSegment {Point = new Point(x.X, x.Y)}))
		            }
		        })
		    };

		    var spath = new Path
		    {
		        StrokeThickness = 0,
		        Data = sg,
		        Fill = Fill,
		        ClipToBounds = true
		    };

            Chart.Canvas.Children.Add(path);
            addedFigures.Add(path);

		    Chart.Canvas.Children.Add(spath);
		    addedFigures.Add(spath);

		    var draw = new DoubleAnimationUsingKeyFrames
		    {
		        BeginTime = TimeSpan.FromSeconds(0),
		        KeyFrames = new DoubleKeyFrameCollection
		        {
		            new SplineDoubleKeyFrame
		            {
		                KeyTime = TimeSpan.FromMilliseconds(1),
		                Value = l
		            },
		            new SplineDoubleKeyFrame
		            {
		                KeyTime = TimeSpan.FromMilliseconds(750),
		                Value = 0
		            }
		        }
		    };
			Storyboard.SetTarget(draw, path);
			Storyboard.SetTargetProperty(draw, new PropertyPath(Shape.StrokeDashOffsetProperty));
			var sbDraw = new Storyboard();
			sbDraw.Children.Add(draw);
			var animated = false;
			if (!Chart.DisableAnimation)
			{
				if (animate)
				{
					sbDraw.Begin();
					animated = true;
				}
			}
			if (!animated) path.StrokeDashOffset = 0;
			return addedFigures;
		}
	}
}