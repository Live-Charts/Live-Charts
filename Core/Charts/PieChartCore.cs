//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;

namespace LiveCharts.Charts
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Charts.ChartCore" />
    public class PieChartCore : ChartCore
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="updater">The updater.</param>
        public PieChartCore(IChartView view, ChartUpdater updater) : base(view, updater)
        {
            updater.Chart = this;
        }

        #endregion

        #region Publics

        /// <summary>
        /// Prepares the axes.
        /// </summary>
        /// <exception cref="LiveChartsException">There is a invalid series in the series collection, " +
        ///                     "verify that all the series implement IPieSeries.</exception>
        public override void PrepareAxes()
        {
            View.Zoom = ZoomingOptions.None;

            if (View.ActualSeries.Any(x => !(x.Model is IPieSeries)))
                throw new LiveChartsException(
                    "There is a invalid series in the series collection, " +
                    "verify that all the series implement IPieSeries.");

            foreach (var xi in AxisX)
            {
                xi.S = 1;
                xi.BotLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).XLimit.Min)
                    .DefaultIfEmpty(0).Min();
                xi.TopLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).XLimit.Max)
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(xi.BotLimit - xi.TopLimit) < xi.S * .01)
                {
                    xi.BotLimit -= xi.S;
                    xi.TopLimit += xi.S;
                }
            }

            foreach (var yi in AxisY)
            {
                //yi.CalculateSeparator(this, AxisTags.X);
                yi.BotLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).YLimit.Min)
                    .DefaultIfEmpty(0).Min();
                yi.TopLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).YLimit.Max)
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(yi.BotLimit - yi.TopLimit) < yi.S * .01)
                {
                    yi.BotLimit -= yi.S;
                    yi.TopLimit += yi.S;
                }
            }

            StackPoints(View.ActualSeries, AxisOrientation.Y, 0);

            var curSize = new CoreRectangle(0, 0, ControlSize.Width, ControlSize.Height);

            curSize = PlaceLegend(curSize);

            DrawMargin.Top = curSize.Top;
            DrawMargin.Left = curSize.Left;
            DrawMargin.Width = curSize.Width;
            DrawMargin.Height = curSize.Height;
        }

        #endregion

    }
}
