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
using System.Windows;
using lvc.Charts;

namespace lvc
{
    public class LineChart : Chart, ILine
    {
        public LineChart()
        {
            AxisX = new Axis();
            AxisY = new Axis
            {
                Separator = {IsEnabled = false, Step = 1},
                IsEnabled = false
            };
            LineType = LineChartLineType.Bezier;
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Dot;
            AreaOpacity = 0.2;
        }

        #region Properties

        public LineChartLineType LineType { get; set; }

        #endregion

        #region Overriden Methods

        protected override void DrawAxes()
        {
            if (Math.Abs(S.X) <= Min.X*.01 || Math.Abs(S.Y) <= Min.Y*.01) return;

            ConfigureSmartAxis(AxisY);

            //S = GetS();

            Canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var lastLabelX = Math.Truncate((Max.X - Min.X)/S.X)*S.X;
            var longestYLabelSize = GetLongestLabelSize(AxisX);
            var firstXLabelSize = GetLabelSize(AxisY, Min.X);
            var lastXLabelSize = GetLabelSize(AxisY, lastLabelX);

            const int padding = 5;

            PlotArea.X = padding*2 +
                         (longestYLabelSize.X > firstXLabelSize.X*.5 ? longestYLabelSize.X : firstXLabelSize.X*.5);
            PlotArea.Y = longestYLabelSize.Y*.5 + padding;
            PlotArea.Height = Math.Max(0, Canvas.DesiredSize.Height - (padding*2 + firstXLabelSize.Y) - PlotArea.Y);
            PlotArea.Width = Math.Max(0, Canvas.DesiredSize.Width - PlotArea.X - padding);
            var distanceToEnd = ToPlotArea(Max.X - lastLabelX, AxisTags.X) - PlotArea.X;
            var change = lastXLabelSize.X*.5 - distanceToEnd > 0 ? lastXLabelSize.X*.5 - distanceToEnd : 0;
            if (change <= PlotArea.Width)
                PlotArea.Width -= change;

            base.DrawAxes();
        }

        #endregion
    }
}