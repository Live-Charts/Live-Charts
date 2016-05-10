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
using LiveCharts.Defaults;

namespace LiveCharts.Charts
{
    public class CartesianChartCore : ChartCore
    {
        #region Contructors

        public CartesianChartCore(IChartView view, IChartUpdater updater) : base(view, updater)
        {
            updater.Chart = this;
            updater.UpdateFrequency();
        }

        #endregion

        #region Publics

        public override void PrepareAxes()
        {
            base.PrepareAxes();

            if (View.Series.Any(x => !(x.Model is ICartesianSeries)))
                throw new Exception(
                    "There is a invalid series in the series collection, " +
                    "verify that all the series are desiged to be plotted in a cartesian chart.");

            var cartesianSeries = View.Series.Select(x => x.Model).Cast<ICartesianSeries>().ToArray();
            
            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                xi.CalculateSeparator(this, AxisTags.X);
                xi.MinLimit = cartesianSeries.Where(x => x.View.ScalesXAt == index)
                    .Select(x => x.GetMinX(xi))
                    .DefaultIfEmpty(0).Min();
                xi.MaxLimit = cartesianSeries.Where(x => x.View.ScalesXAt == index)
                    .Select(x => x.GetMaxX(xi))
                    .DefaultIfEmpty(0).Max();
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                yi.CalculateSeparator(this, AxisTags.Y);
                yi.MinLimit = cartesianSeries.Where(x => x.View.ScalesXAt == index)
                    .Select(x => x.GetMinY(yi))
                    .DefaultIfEmpty(0).Min();
                yi.MaxLimit = cartesianSeries.Where(x => x.View.ScalesXAt == index)
                    .Select(x => x.GetMaxX(yi))
                    .DefaultIfEmpty(0).Max();
            }

            PrepareBubbles();
            PrepareStackedColumns();

            CalculateComponentsAndMargin();
        }

        #endregion

        #region Privates

        private void PrepareBubbles()
        {
            if (!View.Series.Any(x => x is IBubbleSeries)) return;

            var vs = View.Series.Select(x => x.Values.Value3CoreLimit).ToArray();
            Value3CoreLimit = new CoreLimit(vs.Select(x => x.Min).DefaultIfEmpty(0).Min(),
                vs.Select(x => x.Max).DefaultIfEmpty(0).Max());
        }

        private void PrepareStackedColumns()
        {
            if (!View.Series.Any(x => x is IStackedColumnSeries)) return;

            foreach (var group in View.Series.OfType<IStackedColumnSeries>().GroupBy(x => x.ScalesYAt))
            {
                StackPoints(group, AxisTags.Y, group.Key);
                AxisX[group.First().ScalesXAt].EvaluatesUnitWidth = true;
            }
        }

        private void StackPoints(IEnumerable<ISeriesView> stackables, AxisTags stackAt, int stackIndex)
        {
            var stackedColumns = stackables.Select(x => x.Values.Points.ToArray()).ToArray();

            var maxI = stackedColumns.Select(x => x.Length).DefaultIfEmpty(0).Max();

            for (var i = 0; i < maxI; i++)
            {
                var cols = stackedColumns
                    .Select(x => x.Length > i
                        ? new StackedSum(Pull(x[i], stackAt))
                        : new StackedSum()).ToArray();

                var sum = new StackedSum
                {
                    Left = cols.Select(x => x.Left).DefaultIfEmpty(0).Sum(),
                    Right = cols.Select(x => x.Right).DefaultIfEmpty(0).Sum()
                };

                if (stackAt == AxisTags.X)
                {
                    if (sum.Left < AxisX[stackIndex].MinLimit)
                        AxisX[stackIndex].MinLimit =
                            (Math.Truncate(sum.Left/AxisX[stackIndex].S) - 1)*AxisX[stackIndex].S;
                    if (sum.Right > AxisX[stackIndex].MaxLimit)
                        AxisX[stackIndex].MaxLimit =
                            (Math.Truncate(sum.Right/AxisX[stackIndex].S) + 1)*AxisX[stackIndex].S;
                }

                if (stackAt == AxisTags.Y)
                {
                    if (sum.Left < AxisY[stackIndex].MinLimit)
                        AxisY[stackIndex].MinLimit = sum.Left == 0
                            ? 0
                            : (Math.Truncate(sum.Left/AxisY[stackIndex].S) - 1)*AxisY[stackIndex].S;
                    if (sum.Right > AxisY[stackIndex].MaxLimit)
                        AxisY[stackIndex].MaxLimit = sum.Right == 0
                            ? 0
                            : (Math.Truncate(sum.Right / AxisY[stackIndex].S) + 1) * AxisY[stackIndex].S;
                }

                var lastLeft = 0d;
                var lastRight = 0d;

                foreach (var col in stackedColumns)
                {
                    if (i >= col.Length) continue;
                    var point = col[i];
                    var pulled = Pull(point, stackAt);

                    if (pulled <= 0)
                    {
                        point.From = lastLeft;
                        point.To = lastLeft + pulled;
                        point.Sum = sum.Left;
                        point.Participation = (point.To - point.From)/point.Sum;
                        
                        lastLeft = point.To;
                    }
                    else
                    {
                        point.From = lastRight;
                        point.To = lastRight + pulled;
                        point.Sum = sum.Right;
                        point.Participation = (point.To - point.From) / point.Sum;

                        lastRight = point.To;
                    }
                }
            }
        }

        private static double Pull(ChartPoint point, AxisTags source)
        {
            return source == AxisTags.Y ? point.Y : point.X;
        }

        #endregion
    }
}
