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
using System.Linq;

namespace LiveCharts.Charts
{
    public class PieChartCore : ChartCore
    {
        #region Contructors

        public PieChartCore(IChartView view, IChartUpdater updater) : base(view, updater)
        {
            updater.Chart = this;
        }

        #endregion

        #region Publics

        public override void PrepareAxes()
        {
            PivotZoomingAxis = AxisTags.None;
            View.Zoom = ZoomingOptions.None;

            if (View.Series.Any(x => !(x.Model is IPieSeries)))
                throw new Exception(
                    "There is a invalid series in the series collection, " +
                    "verify that all the series implement IPieSeries.");

            foreach (var xi in AxisX)
            {
                xi.S = 1;
                xi.MinLimit = View.Series.Select(x => x.Values.Limit1.Min)
                    .DefaultIfEmpty(0).Min();
                xi.MaxLimit = View.Series.Select(x => x.Values.Limit1.Max)
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(xi.MinLimit - xi.MaxLimit) < xi.S * .01)
                {
                    xi.MinLimit -= xi.S;
                    xi.MaxLimit += xi.S;
                }
            }

            foreach (var yi in AxisY)
            {
                //yi.CalculateSeparator(this, AxisTags.X);
                yi.MinLimit = View.Series.Select(x => x.Values.Limit2.Min)
                    .DefaultIfEmpty(0).Min();
                yi.MaxLimit = View.Series.Select(x => x.Values.Limit2.Max)
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(yi.MinLimit - yi.MaxLimit) < yi.S * .01)
                {
                    yi.MinLimit -= yi.S;
                    yi.MaxLimit += yi.S;
                }
            }

            StackPoints(View.Series, AxisTags.Y, 0);

            var curSize = new CoreRectangle(0, 0, ChartControlSize.Width, ChartControlSize.Height);

            curSize = PlaceLegend(curSize);

            DrawMargin.Top = curSize.Top;
            DrawMargin.Left = curSize.Left;
            DrawMargin.Width = curSize.Width;
            DrawMargin.Height = curSize.Height;
        }

        #endregion

    }
}
