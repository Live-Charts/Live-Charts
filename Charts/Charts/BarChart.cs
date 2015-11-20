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
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LiveCharts.Charts
{
    public class BarChart : Chart
    {
        public BarChart()
        {
            PrimaryAxis = new Axis();
            SecondaryAxis = new Axis {Separator = new Separator {Step = 1}};
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            IgnoresLastLabel = true;
        }

        protected override bool ScaleChanged => GetMax() != Max ||
                                                GetMin() != Min;

        /// <summary>
        /// Gets or sets maximum column width, default is 60
        /// </summary>
        public double MaxColumnWidth { get; set; } = 60;

        private Point GetMax()
        {
            var p = new Point(
                Series.Select(x => x.PrimaryValues.Count).DefaultIfEmpty(0).Max(),
                Series.Select(x => x.PrimaryValues.Max()).DefaultIfEmpty(0).Max());
            p.Y = PrimaryAxis.MaxValue ?? p.Y;
            return p;
        }

        private Point GetMin()
        {
            var p = new Point(0, Series.Select(x => x.PrimaryValues.Min()).DefaultIfEmpty(0).Min());
            p.Y = PrimaryAxis.MinValue ?? p.Y;
            return p;
        }

        private Point GetS()
        {
            return new Point(
                SecondaryAxis.Separator.Step ?? CalculateSeparator(Max.X - Min.X, AxisTags.X),
                PrimaryAxis.Separator.Step ?? CalculateSeparator(Max.Y - Min.Y, AxisTags.Y));
        }

        protected override void Scale()
        {
            Max = GetMax();
            Min = GetMin();
            S = GetS();

            Max.Y = PrimaryAxis.MaxValue ?? (Math.Truncate(Max.Y / S.Y) + 1) * S.Y;
            Min.Y = PrimaryAxis.MinValue ?? (Math.Truncate(Min.Y / S.Y) - 1) * S.Y;

            DrawAxis();
        }

        protected override Point GetToolTipPosition(HoverableShape sender, List<HoverableShape> sibilings, Border b)
        {
            var unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
            var overflow = unitW - MaxColumnWidth * 3 > 0 ? unitW - MaxColumnWidth * 3 : 0;
            unitW = unitW > MaxColumnWidth * 3 ? MaxColumnWidth * 3 : unitW;
            var x = sender.Value.X + 1 > (Min.X + Max.X)/2
                ? ToPlotArea(sender.Value.X, AxisTags.X) + overflow*.5 - b.DesiredSize.Width
                : ToPlotArea(sender.Value.X, AxisTags.X) + unitW + overflow*.5;
            var y = ActualHeight*.5 - b.DesiredSize.Height*.5;
            return new Point(x, y);
        }

        protected override void DrawAxis()
        {
            ConfigureSmartAxis(SecondaryAxis);

            S = GetS();

            Canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var lastLabelX = Math.Truncate((Max.X - Min.X) / S.X) * S.X;
            var longestYLabelSize = GetLongestLabelSize(PrimaryAxis);
            var fistXLabelSize = GetLabelSize(SecondaryAxis, Min.X);
            var lastXLabelSize = GetLabelSize(SecondaryAxis, lastLabelX);

            const int padding = 5;

            var unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
            unitW = unitW > MaxColumnWidth * 3 ? MaxColumnWidth * 3 : unitW;
            LabelOffset = unitW / 2;

            PlotArea.X = padding*2 +
                         (fistXLabelSize.X*0.5 - LabelOffset > longestYLabelSize.X
                             ? fistXLabelSize.X*0.5 - LabelOffset
                             : longestYLabelSize.X);
            PlotArea.Y = longestYLabelSize.Y * .5 + padding;
            PlotArea.Height = Canvas.DesiredSize.Height - (padding * 2 + fistXLabelSize.Y) - PlotArea.Y;
            PlotArea.Width = Canvas.DesiredSize.Width - PlotArea.X - padding;
            var distanceToEnd = PlotArea.Width - (ToPlotArea(Max.X, AxisTags.X) - ToPlotArea(1, AxisTags.X));
            distanceToEnd -= LabelOffset + padding;
            PlotArea.Width -= lastXLabelSize.X * .5 - distanceToEnd > 0 ? lastXLabelSize.X * .5 - distanceToEnd : 0;

            //calculate it again to get a better result
            unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5; 
            unitW = unitW > MaxColumnWidth * 3 ? MaxColumnWidth * 3 : unitW;
            LabelOffset = unitW / 2;

            base.DrawAxis();
        }
    }
}
