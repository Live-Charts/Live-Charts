﻿//The MIT License(MIT)

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
using LiveCharts.Charts;

namespace LiveCharts
{
    public class StackedBarChart : Chart, IStackedBar, ILine
    {
        public StackedBarChart()
        {
            PrimaryAxis = new Axis();
            SecondaryAxis = new Axis {Separator = {Step = 1}};
            Hoverable = true;
            PrimaryAxis.MinValue = 0d;
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            IgnoresLastLabel = true;
            PerformanceConfiguration = new PerformanceConfiguration { Enabled = false };
            AreaOpacity = 0.8;
            LineType = LineChartLineType.Bezier;
            IndexTotals = new Dictionary<int, StackedBarHelper>();
        }

        /// <summary>
        /// Gets or sets maximum column width, default is 60
        /// </summary>
        public double MaxColumnWidth { get; set; } = 60;
        public LineChartLineType LineType { get; set; }

        public Dictionary<int, StackedBarHelper> IndexTotals { get; set; } 
        protected override bool ScaleChanged => GetMax() != Max ||
                                                GetMin() != Min;

        private Point GetMax()
        {
            var s = Series.FirstOrDefault();
            if (s == null) return new Point(0,0);
            var p = new Point(
                Series.Select(x => x.PrimaryValues.Count).DefaultIfEmpty(0).Max(),
                s.PrimaryValues.Select(
                    (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.PrimaryValues.Any()
                        ? serie.PrimaryValues[i]
                        : double.MinValue))
                    .Concat(new[] {double.MinValue}).Max());

            //correction for lineSeries
            var maxLineSeries =
                Series.OfType<LineSeries>()
                    .Select(series => series.PrimaryValues.DefaultIfEmpty(0).Max())
                    .DefaultIfEmpty(0)
                    .Max();
            p.Y = p.Y > maxLineSeries ? p.Y : maxLineSeries;

            p.Y = PrimaryAxis.MaxValue ?? p.Y;
            return p;
        }

        private Point GetMin()
        {
            var s = Series.FirstOrDefault();
            if (s==null) return new Point(0,0);
            var p = new Point(0,
                s.PrimaryValues.Select(
                    (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.PrimaryValues.Any()
                        ? serie.PrimaryValues[i]
                        : double.MinValue))
                    .Concat(new[] {double.MaxValue}).Min());

            //correction for lineSeries
            var minLineSeries =
                Series.OfType<LineSeries>()
                    .Select(series => series.PrimaryValues.DefaultIfEmpty(0).Min())
                    .DefaultIfEmpty(0)
                    .Min();
            p.Y = p.Y < minLineSeries ? p.Y : minLineSeries;

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
            PrimaryAxis.MinValue = 0;

            var stackedSeries = Series.OfType<StackedBarSeries>().ToList();
            var fSerie = stackedSeries.FirstOrDefault();
            if (fSerie == null) return;
            for (var i = 0; i < fSerie.PrimaryValues.Count; i++)
            {
                var helper = new StackedBarHelper();
                var sum = 0d;
                for (int index = 0; index < stackedSeries.Count; index++)
                {
                    var serie = stackedSeries[index];
	                var value = serie.PrimaryValues.Any() ? serie.PrimaryValues[i] : double.MinValue;
                    helper.Stacked[index] = new StackedItem
                    {
                        Value = value,
                        Stacked = sum
                    };
                    sum += value;
                }
                helper.Total = sum;
                IndexTotals[i] = helper;
            }

            Max = GetMax();
            Min = GetMin();
            S = GetS();

            Max.Y = PrimaryAxis.MaxValue ?? (Math.Truncate(Max.Y / S.Y) + 1) * S.Y;
            Min.Y = PrimaryAxis.MinValue ?? (Math.Truncate(Min.Y / S.Y) - 1) * S.Y;

            DrawAxis();
        }

        protected override Point GetToolTipPosition(HoverableShape sender, List<HoverableShape> sibilings)
        {
            DataToolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
            var overflow = unitW - MaxColumnWidth > 0 ? unitW - MaxColumnWidth : 0;
            unitW = unitW > MaxColumnWidth ? MaxColumnWidth : unitW;
            var x = sender.Value.X + 1 > (Min.X + Max.X) / 2
                ? ToPlotArea(sender.Value.X, AxisTags.X) + overflow * .5 - DataToolTip.DesiredSize.Width
                : ToPlotArea(sender.Value.X, AxisTags.X) + unitW + overflow * .5;
            var y = ActualHeight * .5 - DataToolTip.DesiredSize.Height * .5;
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
            XOffset = unitW / 2;

            PlotArea.X = padding * 2 +
                         (fistXLabelSize.X * 0.5 - XOffset > longestYLabelSize.X
                             ? fistXLabelSize.X * 0.5 - XOffset
                             : longestYLabelSize.X);
            PlotArea.Y = longestYLabelSize.Y * .5 + padding;
            PlotArea.Height = Math.Max(0, Canvas.DesiredSize.Height - (padding * 2 + fistXLabelSize.Y) - PlotArea.Y);
            PlotArea.Width = Math.Max(0,  Canvas.DesiredSize.Width - PlotArea.X - padding);
            var distanceToEnd = PlotArea.Width - (ToPlotArea(Max.X, AxisTags.X) - ToPlotArea(1, AxisTags.X));
            distanceToEnd -= XOffset + padding;
	        var change = lastXLabelSize.X * .5 - distanceToEnd > 0 ? lastXLabelSize.X * .5 - distanceToEnd : 0;
			if (change <= PlotArea.Width)
	            PlotArea.Width -= change;

            //calculate it again to get a better result
            unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
            unitW = unitW > MaxColumnWidth * 3 ? MaxColumnWidth * 3 : unitW;
            XOffset = unitW / 2;

            base.DrawAxis();
        }
    }

    public class StackedBarHelper
    {
        public StackedBarHelper()
        {
            Stacked = new Dictionary<int, StackedItem>();
        }
        public double Total { get; set; }
        public Dictionary<int, StackedItem> Stacked { get; set; }
    }

    public class StackedItem
    {
        public double Value { get; set; }
        public double Stacked { get; set; }
    }
}
