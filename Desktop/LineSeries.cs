//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez

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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveChartsCore;

namespace LiveChartsDesktop
{
    public class LineSeries : Series
    {
        public LineSeries()
        {
            Model = new LiveChartsCore.LineSeries(this);
        }

        public LineSeries(SeriesConfiguration configuration) : base(configuration)
        {
            Model = new LiveChartsCore.LineSeries(this);
        }

        public static readonly DependencyProperty PointRadiusProperty = DependencyProperty.Register(
            "PointRadius", typeof (double), typeof (LineSeries), new PropertyMetadata(default(double)));

        public double PointRadius
        {
            get { return (double) GetValue(PointRadiusProperty); }
            set { SetValue(PointRadiusProperty, value); }
        }

        public override IChartPointView InitializePointView()
        {
            var mhr = PointRadius < 5 ? 5 : PointRadius;

            return new PointBezierView
            {
                HoverShape = Model.Chart.View.IsHoverable
                    ? new Rectangle
                    {
                        Fill = Brushes.Transparent,
                        StrokeThickness = 0,
                        Width = mhr,
                        Height = mhr
                    }
                    : null,
                Ellipse = Math.Abs(PointRadius) < 0.1 ? null : new Ellipse(),
                Segment = Math.Abs(StrokeThickness) < 0.1 ? null : new BezierSegment(),
                IsNew = true,
                DataLabel = DataLabels ? new TextBlock() : null,
            };
        }
    }

    public class LineVisualPoint : VisualPoint
    {
        public PathFigure Owner { get; set; }
        public BezierSegment Segment { get; set; }

        public BezierData Data { get; set; }
        public LineVisualPoint Previous { get; set; }
    }

    public class VisualPoint
    {
        public ChartPoint ChartPoint { get; set; }
        public Series Series { get; set; }
        public Shape Shape { get; set; }
        public Shape HoverShape { get; set; }
        public TextBlock TextBlock { get; set; }
        public bool IsNew { get; set; }
        public bool IsHighlighted { get; set; }
    }
}
