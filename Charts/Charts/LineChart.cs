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
using System.Linq;
using System.Windows;

namespace LiveCharts.Charts
{
	public class LineChart : Chart
	{
		private Point _rawMax = new Point(0, 0);
		private Point _rawMin = new Point(0, 0);
		private Point _rawS = new Point(0, 0);

		public LineChart()
		{
			PrimaryAxis = new Axis();
			SecondaryAxis = new Axis
				{
					Separator = {Enabled = false, Step = 1},
					Enabled = false
				};
			LineType = LineChartLineType.Bezier;
			IncludeArea = true;
			Hoverable = true;
			ShapeHoverBehavior = ShapeHoverBehavior.Dot;
		}

		protected override bool ScaleChanged => GetMax() != _rawMax ||
		                                        GetMin() != _rawMin ||
		                                        GetS() != _rawS;

		public static readonly DependencyProperty IncludeAreaProperty = DependencyProperty.Register("IncludeArea", typeof (bool), typeof (LineChart));
		/// <summary>
		/// indicates wheather Series should  draw its area.
		/// </summary>
		public bool IncludeArea
		{
			get { return (bool) GetValue(IncludeAreaProperty); }
			set { SetValue(IncludeAreaProperty, value); }
		}

		public static readonly DependencyProperty LineTypeProperty = DependencyProperty.Register("LineType", typeof (LineChartLineType), typeof (LineChart));
		/// <summary>
		/// Iditacates series line type, use Bezier to get a smooth but aproximated line, or Polyline to
		/// draw a line only by the known points.
		/// </summary>
		public LineChartLineType LineType
		{
			get { return (LineChartLineType) GetValue(LineTypeProperty); }
			set { SetValue(LineTypeProperty, value); }
		}

		private Point GetMax()
		{
			var p = new Point(TypedSeries.Select(x => x.PrimaryValues.Count).DefaultIfEmpty(0).Max() - 1,
							  TypedSeries.Select(x => x.PrimaryValues.DefaultIfEmpty(0).Max()).DefaultIfEmpty(0).Max());
			p.Y = PrimaryAxis.MaxValue ?? p.Y;
			return p;

		}

		private Point GetMin()
		{
			var p = new Point(0,
							  TypedSeries.Select(x => x.PrimaryValues.DefaultIfEmpty(0).Min()).DefaultIfEmpty(0).Min());
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
			_rawMax = GetMax();
			_rawMin = GetMin();

			Max = new Point(_rawMax.X, _rawMax.Y);
			Min = new Point(_rawMin.X, _rawMin.Y);

			_rawS = GetS();
			S = new Point(_rawS.X, _rawS.Y);

			Max.Y = PrimaryAxis.MaxValue ?? (Math.Truncate(Max.Y / S.Y) + 1) * S.Y;
			Min.Y = PrimaryAxis.MinValue ?? (Math.Truncate(Min.Y / S.Y) - 1) * S.Y;

			DrawAxis();
		}

		protected override void DrawAxis()
		{
			ConfigureSmartAxis(SecondaryAxis);

			S = GetS();

			Canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

			var lastLabelX = Math.Truncate((Max.X - Min.X) / S.X) * S.X;
			var longestYLabelSize = GetLongestLabelSize(PrimaryAxis);
			var firstXLabelSize = GetLabelSize(SecondaryAxis, Min.X);
			var lastXLabelSize = GetLabelSize(SecondaryAxis, lastLabelX);

			const int padding = 5;

			PlotArea.X = padding * 2 + (longestYLabelSize.X > firstXLabelSize.X * .5 ? longestYLabelSize.X : firstXLabelSize.X * .5);
			PlotArea.Y = longestYLabelSize.Y * .5 + padding;
			PlotArea.Height = Math.Max(0, Canvas.DesiredSize.Height - (padding * 2 + firstXLabelSize.Y) - PlotArea.Y);
			PlotArea.Width = Math.Max(0, Canvas.DesiredSize.Width - PlotArea.X - padding);
			var distanceToEnd = ToPlotArea(Max.X - lastLabelX, AxisTags.X) - PlotArea.X;
			var change = lastXLabelSize.X * .5 - distanceToEnd > 0 ? lastXLabelSize.X * .5 - distanceToEnd : 0;
			if (change <= PlotArea.Width)
				PlotArea.Width -= change;

			base.DrawAxis();
		}
	}
}