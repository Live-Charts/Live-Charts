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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class BarChart : Chart, IBar, ILine
    {
        public BarChart()
        {
            AxisY = new Axis();
            AxisX = new Axis {Separator = new Separator {Step = 1}};
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            MaxColumnWidth = 60;
            DefaultFillOpacity = 0.75;
            LineSmoothness = 0.8;
        }

        #region Properties

        /// <summary>
        /// Gets or sets maximum column width, default is 60
        /// </summary>
        public double MaxColumnWidth { get; set; }

        /// <summary>
        /// Gets or sets Line Smoothness
        /// </summary>
        public double LineSmoothness { get; set; }

        internal new bool HasValidSeriesAndValues
        {
            get { return Series.Any(x => x.Values != null && x.Values.Count > 0); }
        }

        #endregion

        #region Overriden Methods

        protected override Point GetToolTipPosition(ShapeMap sender, List<ShapeMap> sibilings)
        {
            DataTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
            var overflow = unitW - MaxColumnWidth*3 > 0 ? unitW - MaxColumnWidth*3 : 0;
            unitW = unitW > MaxColumnWidth*3 ? MaxColumnWidth*3 : unitW;
            var x = sender.ChartPoint.X + 1 > (Min.X + Max.X)/2
                ? ToPlotArea(sender.ChartPoint.X, AxisTags.X) + overflow*.5 - DataTooltip.DesiredSize.Width
                : ToPlotArea(sender.ChartPoint.X, AxisTags.X) + unitW + overflow*.5;
            var y = ToPlotArea(sibilings.Select(s => s.ChartPoint.Y).DefaultIfEmpty(0).Sum()
                               /sibilings.Count, AxisTags.Y);
            y = y + DataTooltip.DesiredSize.Height > ActualHeight
                ? y - (y + DataTooltip.DesiredSize.Height - ActualHeight) - 5
                : y;
            return new Point(x, y);
        }

        protected override void Scale()
        {
            if (!HasValidSeriesAndValues) return;

            base.Scale();

            if (Invert) Min.Y -= 1;
            else Max.X += 1;

            S = new Point(
                 AxisX.Separator.Step ?? CalculateSeparator(Max.X - Min.X, AxisTags.X),
                 AxisY.Separator.Step ?? CalculateSeparator(Max.Y - Min.Y, AxisTags.Y));

            if (Invert)
            {
                if (AxisX.MaxValue == null) Max.X = (Math.Round(Max.X/S.X) + 1)*S.X;
                if (AxisX.MinValue == null) Min.X = (Math.Truncate(Min.X/S.X) - 1)*S.X;
            }
            else
            {
                if (AxisY.MaxValue == null) Max.Y = (Math.Round(Max.Y/S.Y) + 1)*S.Y;
                if (AxisY.MinValue == null) Min.Y = (Math.Truncate(Min.Y/S.Y) - 1)*S.Y;
            }

            DrawAxes();
        }

        protected override void DrawAxes()
        {
            if (Invert) ConfigureYAsIndexed();
            else ConfigureXAsIndexed();

            Canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var lastLabelX = Math.Truncate((Max.X - Min.X)/S.X)*S.X;
            var longestYLabelSize = GetLongestLabelSize(AxisY);
            var fistXLabelSize = GetLabelSize(AxisX, Min.X);
            var lastXLabelSize = GetLabelSize(AxisX, lastLabelX);

            const int padding = 5;

            var unitW = Invert
                ? ToPlotArea(Max.Y - 1, AxisTags.Y) - PlotArea.Y +5
                : ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;

            XOffset = 0;
            YOffset = 0;

            if (Invert) YOffset = unitW/2;
            else XOffset = unitW/2;

            PlotArea.X = padding*2 +
                         (fistXLabelSize.X*0.5 - XOffset > longestYLabelSize.X
                             ? fistXLabelSize.X*0.5 - XOffset
                             : longestYLabelSize.X);
            PlotArea.Y = longestYLabelSize.Y*.5 + padding;
            PlotArea.Height = Math.Max(0, Canvas.DesiredSize.Height - (padding*2 + fistXLabelSize.Y) - PlotArea.Y);
            PlotArea.Width = Math.Max(0, Canvas.DesiredSize.Width - PlotArea.X - padding);
            var distanceToEnd = PlotArea.Width - (ToPlotArea(Max.X, AxisTags.X) - ToPlotArea(1, AxisTags.X));
            distanceToEnd -= XOffset + padding;
            var change = lastXLabelSize.X*.5 - distanceToEnd > 0 ? lastXLabelSize.X*.5 - distanceToEnd : 0;
            if (change <= PlotArea.Width)
                PlotArea.Width -= change;

            //calculate it again to get a better result
            unitW = Invert
                ? ToPlotArea(Max.Y - 1, AxisTags.Y) - PlotArea.Y + 5
                : ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;

            if (Invert) YOffset = unitW / 2;
            else XOffset = unitW / 2;

            base.DrawAxes();
        }

        #endregion
    }
}
