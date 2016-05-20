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
    public class CartesianChartCore : ChartCore
    {
        #region Contructors

        public CartesianChartCore(IChartView view, IChartUpdater updater) : base(view, updater)
        {
            updater.Chart = this;
        }

        #endregion

        #region Publics

        public override void PrepareAxes()
        {
            PivotZoomingAxis = AxisTags.X;
            base.PrepareAxes();

            if (View.Series.Any(x => !(x.Model is ICartesianSeries)))
                throw new Exception(
                    "There is a invalid series in the series collection, " +
                    "verify that all the series implement ICartesianSeries.");

            var cartesianSeries = View.Series.Select(x => x.Model).Cast<ICartesianSeries>().ToArray();

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                xi.CalculateSeparator(this, AxisTags.X);
                xi.MinLimit = xi.MinValue ?? cartesianSeries.Where(x => x.View.ScalesXAt == index)
                    .Select(x => x.GetMinX(xi))
                    .DefaultIfEmpty(0).Min();
                xi.MaxLimit = xi.MaxValue ?? cartesianSeries.Where(x => x.View.ScalesXAt == index)
                    .Select(x => x.GetMaxX(xi))
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(xi.MinLimit - xi.MaxLimit) < xi.S * .01)
                {
                    xi.MinLimit -= xi.S;
                    xi.MaxLimit += xi.S;
                }
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                yi.CalculateSeparator(this, AxisTags.Y);
                yi.MinLimit = yi.MinValue ?? cartesianSeries.Where(x => x.View.ScalesYAt == index)
                    .Select(x => x.GetMinY(yi))
                    .DefaultIfEmpty(0).Min();
                yi.MaxLimit = yi.MaxValue ?? cartesianSeries.Where(x => x.View.ScalesYAt == index)
                    .Select(x => x.GetMaxY(yi))
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(yi.MinLimit - yi.MaxLimit) < yi.S * .01)
                {
                    yi.MinLimit -= yi.S;
                    yi.MaxLimit += yi.S;
                }
            }

            PrepareUnitWidthColumns();
            PrepareBubbles();
            PrepareStackedColumns();
            PrepareStackedRows();
            PrepareStackedAreas();
            PrepareVerticalStackedAreas();

            CalculateComponentsAndMargin();

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                foreach (var section in xi.Sections)
                {
                    section.View.DrawOrMove(AxisTags.X, index);
                }
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                foreach (var section in yi.Sections)
                {
                    section.View.DrawOrMove(AxisTags.Y, index);
                }
            }
        }

        #endregion

        #region Privates

        private void PrepareBubbles()
        {
            if (!View.Series.Any(x => x is IBubbleSeriesView)) return;

            var vs = View.Series.Select(x => x.Values.Limit3).ToArray();
            Value3CoreLimit = new CoreLimit(vs.Select(x => x.Min).DefaultIfEmpty(0).Min(),
                vs.Select(x => x.Max).DefaultIfEmpty(0).Max());
        }

        private void PrepareUnitWidthColumns()
        {
            foreach (var series in View.Series)
            {
                if (series is IStackedColumnSeriesViewView || series is IColumnSerieView || series is IOhlcSeriesView)
                {
                    AxisX[series.ScalesXAt].EvaluatesUnitWidth = true;
                }
                if (series is IStackedRowSeriesViewView || series is IRowSeriesView)
                {
                    AxisY[series.ScalesYAt].EvaluatesUnitWidth = true;
                }
            }
        }

        private void PrepareStackedColumns()
        {
            if (!View.Series.Any(x => x is IStackedColumnSeriesViewView)) return;

            var isPercentage =
                View.Series.Any(x => x is IStackModelableSeriesView && x is IStackedColumnSeriesViewView &&
                                     ((IStackModelableSeriesView)x).StackMode == StackMode.Percentage);

            foreach (var group in View.Series.OfType<IStackedColumnSeriesViewView>().GroupBy(x => x.ScalesYAt))
            {
                StackPoints(group, AxisTags.Y, group.Key, isPercentage ? StackMode.Percentage : StackMode.Values);
            }
        }

        private void PrepareStackedRows()
        {
            if (!View.Series.Any(x => x is IStackedRowSeriesViewView)) return;

            var isPercentage =
                View.Series.Any(x => x is IStackModelableSeriesView && x is IStackedRowSeriesViewView &&
                                     ((IStackModelableSeriesView) x).StackMode == StackMode.Percentage);

            foreach (var group in View.Series.OfType<IStackedRowSeriesViewView>().GroupBy(x => x.ScalesXAt))
            {
                StackPoints(group, AxisTags.X, group.Key, isPercentage ? StackMode.Percentage : StackMode.Values);
            }
        }

        private void PrepareStackedAreas()
        {
            if (!View.Series.Any(x => x is IStackedAreaSeriesViewView)) return;

            var isPercentage =
                View.Series.Any(x => x is IStackModelableSeriesView && x is IStackedAreaSeriesViewView &&
                                     ((IStackModelableSeriesView) x).StackMode == StackMode.Percentage);

            foreach (var group in View.Series.OfType<IStackedAreaSeriesViewView>().GroupBy(x => x.ScalesYAt))
            {
                StackPoints(group, AxisTags.Y, group.Key, isPercentage ? StackMode.Percentage : StackMode.Values);
            }
        }

        private void PrepareVerticalStackedAreas()
        {
            if (!View.Series.Any(x => x is IVerticalStackedAreaSeriesViewView)) return;

            var isPercentage =
                View.Series.Any(x => x is IStackModelableSeriesView && x is IVerticalStackedAreaSeriesViewView &&
                                     ((IStackModelableSeriesView) x).StackMode == StackMode.Percentage);

            foreach (var group in View.Series.OfType<IVerticalStackedAreaSeriesViewView>().GroupBy(x => x.ScalesXAt))
            {
                StackPoints(group, AxisTags.X, group.Key, isPercentage ? StackMode.Percentage : StackMode.Values);
            }
        }

        #endregion
    }
}
