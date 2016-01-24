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
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.CoreComponents;
using LiveCharts.Tooltip;

namespace LiveCharts
{
    public class ScatterChart : Chart, ILine
    {
        public ScatterChart()
        {
            AxisY = new Axis();
            AxisX = new Axis();
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Dot;
            LineType = LineChartLineType.Bezier;
            DataToolTip = new DefaultScatterTooltip();

            var defaultConfig = new SeriesConfiguration<Point>().X(pt => pt.X).Y(pt => pt.Y);
            SetCurrentValue(SeriesProperty, new SeriesCollection(defaultConfig));
        }

        #region Properties

        /// <summary>
        /// Gets or sets Line Type
        /// </summary>
        public LineChartLineType LineType { get; set; }

        #endregion


        #region Overriden Methods

        protected override void Scale()
        {
            base.Scale();

            S = new Point(
                AxisX.Separator.Step ?? CalculateSeparator(Max.X - Min.X, AxisTags.X),
                AxisY.Separator.Step ?? CalculateSeparator(Max.Y - Min.Y, AxisTags.Y));

            if (AxisY.MaxValue == null) Max.Y = (Math.Truncate(Max.Y/S.Y) + 1)*S.Y;
            if (AxisY.MinValue == null) Min.Y = (Math.Truncate(Min.Y/S.Y) - 1)*S.Y;

            if (AxisX.MaxValue == null) Max.X = (Math.Truncate(Max.X/S.X) + 1)*S.X;
            if (AxisX.MinValue == null) Min.X = (Math.Truncate(Min.X/S.X) - 1)*S.X;

            DrawAxes();
        }

        protected override void DrawAxes()
        {
            Canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var lastLabelX = Math.Truncate((Max.X - Min.X)/S.X)*S.X;
            var longestYLabelSize = GetLongestLabelSize(AxisY);
            var firstXLabelSize = GetLabelSize(AxisX, Min.X);
            var lastXLabelSize = GetLabelSize(AxisX, lastLabelX);

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

        public override void DataMouseEnter(object sender, MouseEventArgs e)
        {
            if (DataToolTip == null) return;

            DataToolTip.Visibility = Visibility.Visible;
            TooltipTimer.Stop();

            var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
            if (senderShape == null) return;

            senderShape.Target.Stroke = senderShape.Series.Stroke;
            senderShape.Target.Fill = new SolidColorBrush {Color = PointHoverColor};

            var scatterToolTip = DataToolTip as ScatterTooltip;

            if (scatterToolTip != null)
            {
                scatterToolTip.PrimaryAxisTitle = AxisY.Title;
                scatterToolTip.PrimaryValue = AxisY.LabelFormatter == null
                    ? senderShape.Value.Y.ToString(CultureInfo.InvariantCulture)
                    : AxisY.LabelFormatter(senderShape.Value.Y);
                scatterToolTip.SecondaryAxisTitle = AxisX.Title;
                scatterToolTip.SecondaryValue = AxisX.LabelFormatter == null
                    ? senderShape.Value.X.ToString(CultureInfo.InvariantCulture)
                    : AxisX.LabelFormatter(senderShape.Value.X);
            }
            var p = GetToolTipPosition(senderShape, null);

            DataToolTip.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                To = p.X,
                Duration = TimeSpan.FromMilliseconds(200)
            });
            DataToolTip.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                To = p.Y,
                Duration = TimeSpan.FromMilliseconds(200)
            });
        }

        public override void DataMouseLeave(object sender, MouseEventArgs e)
        {
            var s = sender as Shape;
            if (s == null) return;

            var shape = HoverableShapes.FirstOrDefault(x => Equals(x.Shape, s));
            if (shape == null) return;

            shape.Target.Fill = shape.Series.Stroke;
            shape.Target.Stroke = new SolidColorBrush {Color = PointHoverColor};

            TooltipTimer.Stop();
            TooltipTimer.Start();
        }

        protected override Point GetToolTipPosition(HoverableShape sender, List<HoverableShape> sibilings)
        {
            DataToolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var x = sender.Value.X > (Min.X + Max.X)/2
                ? ToPlotArea(sender.Value.X, AxisTags.X) - 10 - DataToolTip.DesiredSize.Width
                : ToPlotArea(sender.Value.X, AxisTags.X) + 10;
            var y = sender.Value.Y > (Min.Y + Max.Y)/2
                ? ToPlotArea(sender.Value.Y, AxisTags.Y) + 10
                : ToPlotArea(sender.Value.Y, AxisTags.Y) - 10 - DataToolTip.DesiredSize.Height;
            return new Point(x, y);
        }
    }

    #endregion
}