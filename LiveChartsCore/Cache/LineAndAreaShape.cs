//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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

using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LiveCharts.Cache
{
    internal class LineAndAreaShape
    {
        public LineAndAreaShape(PathFigure figure, Path path)
        {
            Figure = figure;
            Path = path;
            Right = new LineSegment(new Point(), false);
            Bottom = new LineSegment(new Point(), false);
            Left = new LineSegment(new Point(), false);

            Figure.Segments.Add(Right);
            Figure.Segments.Add(Bottom);
            Figure.Segments.Add(Left);
        }

        public PathFigure Figure { get; set; }
        public Path Path { get; set; }
        public LineSegment Right { get; set; }
        public LineSegment Bottom { get; set; }
        public LineSegment Left { get; set; }

        public void DrawLimits(Point first, Point last, Point minPoint, bool invert)
        {
            Figure.Segments.Remove(Right);
            Figure.Segments.Remove(Bottom);
            Figure.Segments.Remove(Left);

            Figure.Segments.Add(Right);
            Figure.Segments.Add(Bottom);
            Figure.Segments.Add(Left);

            Right.Point = invert
                ? new Point(minPoint.X, last.Y)
                : new Point(last.X, minPoint.Y);
            Bottom.Point = invert
                ? new Point(minPoint.X, first.Y)
                : new Point(first.X, minPoint.Y);
            Left.Point = new Point(first.X, first.Y);
        }
    }
}

