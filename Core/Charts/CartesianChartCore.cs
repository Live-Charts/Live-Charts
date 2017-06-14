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
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Helpers;

namespace LiveCharts.Charts
{
    /// <summary>
    /// Chart Model
    /// </summary>
    public class CartesianChartCore : ChartCore
    {
        #region Constructors

        /// <summary>
        /// Initializes Chart model
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="updater">The updater.</param>
        public CartesianChartCore(IChartView view, ChartUpdater updater) : base(view, updater)
        {
            updater.Chart = this;
        }

        #endregion

        #region Publics

        /// <summary>
        /// Prepares Chart Axes
        /// </summary>
        public override void PrepareAxes()
        {
            base.PrepareAxes();

            if (View.ActualSeries.Any(x => !(x.Model is ICartesianSeries)))
                throw new LiveChartsException(
                    "There is a invalid series in the series collection, " +
                    "verify that all the series implement ICartesianSeries.");

            var cartesianSeries = View.ActualSeries.Select(x => x.Model).Cast<ICartesianSeries>().ToArray();

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];

                xi.CalculateSeparator(this, AxisOrientation.X);

                // ReSharper disable once AccessToModifiedClosure
                SetAxisLimits(xi, cartesianSeries.Where(x => x.View.ScalesXAt == index).ToArray(), AxisOrientation.X);

                if (Math.Abs(xi.BotLimit - xi.TopLimit) < xi.S * .01)
                {
                    if (Math.Abs(xi.PreviousBot - xi.PreviousTop) < xi.S * .01)
                    {
                        if (double.IsNaN(xi.MinValue)) xi.BotLimit -= xi.S;
                        else xi.BotLimit = xi.MinValue;

                        if (double.IsNaN(xi.MaxValue)) xi.TopLimit += xi.S;
                        else xi.TopLimit = xi.MaxValue;

                        if (Math.Abs(xi.BotLimit - xi.TopLimit) < xi.S * .01 && !View.IsInDesignMode)
                            throw new LiveChartsException("One axis has an invalid range, it is or it " +
                                                          "tends to zero, please ensure your axis has a valid " +
                                                          "range");
                    }
                    else
                    {
                        xi.BotLimit = xi.PreviousBot;
                        xi.TopLimit = xi.PreviousTop;
                    }
                }
                xi.PreviousBot = xi.BotLimit;
                xi.PreviousTop = xi.TopLimit;
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];

                yi.CalculateSeparator(this, AxisOrientation.Y);

                // ReSharper disable once AccessToModifiedClosure
                SetAxisLimits(yi, cartesianSeries.Where(x => x.View.ScalesYAt == index).ToArray(), AxisOrientation.Y);

                if (Math.Abs(yi.BotLimit - yi.TopLimit) < yi.S * .01)
                {
                    if (Math.Abs(yi.PreviousBot - yi.PreviousTop) < yi.S * .01)
                    {
                        if (double.IsNaN(yi.MinValue)) yi.BotLimit -= yi.S;
                        else yi.BotLimit = yi.MinValue;

                        if (double.IsNaN(yi.MaxValue)) yi.TopLimit += yi.S;
                        else yi.TopLimit = yi.MaxValue;

                        if (Math.Abs(yi.BotLimit - yi.TopLimit) < yi.S * .01)
                            throw new LiveChartsException("One axis has an invalid range, it is or it " +
                                                          "tends to zero, please ensure your axis has a valid " +
                                                          "range");
                    }
                    else
                    {
                        yi.BotLimit = yi.PreviousBot;
                        yi.TopLimit = yi.PreviousTop;
                    }
                }
                yi.PreviousBot = yi.BotLimit;
                yi.PreviousTop = yi.TopLimit;
            }

            PrepareSeries();
            CalculateComponentsAndMargin();
            DrawOrUpdateSections();

            AreComponentsLoaded = true;
        }

        /// <summary>
        /// Runs the specialized chart components.
        /// </summary>
        public override void RunSpecializedChartComponents()
        {
            foreach (var visualElement in ((ICartesianChart) View).VisualElements)
            {
                visualElement.AddOrMove(this);
            }
        }

        /// <summary>
        /// Draws the or update sections.
        /// </summary>
        public void DrawOrUpdateSections()
        {
            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                foreach (var section in xi.Sections)
                {
                    section.AxisIndex = index;
                    section.Source = AxisOrientation.X;
                    section.View.DrawOrMove(AxisOrientation.X, index);
                }
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                foreach (var section in yi.Sections)
                {
                    section.AxisIndex = index;
                    section.Source = AxisOrientation.Y;
                    section.View.DrawOrMove(AxisOrientation.Y, index);
                }
            }
            
        }

        #endregion

        #region Privates

        private static void SetAxisLimits(AxisCore ax, IList<ICartesianSeries> series, AxisOrientation orientation)
        {
            var first = new CoreLimit();
            var firstR = 0d;

            if (series.Count > 0)
            {
                first = orientation == AxisOrientation.X
                    ? new CoreLimit(series[0].GetMinX(ax), series[0].GetMaxX(ax))
                    : new CoreLimit(series[0].GetMinY(ax), series[0].GetMaxY(ax));
                var view = series[0].View as IAreaPoint;
                firstR = view != null ? view.GetPointDiameter() : 0;
            }

            //                     [ max, min, pointRadius ]
            var boundries = new[] { first.Max, first.Min, firstR };

            for (var index = 1; index < series.Count; index++)
            {
                var cartesianSeries = series[index];
                var limit = orientation == AxisOrientation.X
                    ? new CoreLimit(cartesianSeries.GetMinX(ax), cartesianSeries.GetMaxX(ax))
                    : new CoreLimit(cartesianSeries.GetMinY(ax), cartesianSeries.GetMaxY(ax));
                var view = cartesianSeries.View as IAreaPoint;
                var radius = view != null ? view.GetPointDiameter() : 0;

                if (limit.Max > boundries[0]) boundries[0] = limit.Max;
                if (limit.Min < boundries[1]) boundries[1] = limit.Min;
                if (radius > boundries[2]) boundries[2] = radius;
            }

            ax.TopSeriesLimit = boundries[0];
            ax.BotSeriesLimit = boundries[1];

            ax.TopLimit = double.IsNaN(ax.MaxValue) ? boundries[0] : ax.MaxValue;
            ax.BotLimit = double.IsNaN(ax.MinValue) ? boundries[1] : ax.MinValue;

            ax.MaxPointRadius = boundries[2];
        }

        private void PrepareSeries()
        {
            PrepareUnitWidth();
            PrepareWeight();
            PrepareStackedColumns();
            PrepareStackedRows();
            PrepareStackedAreas();
            PrepareVerticalStackedAreas();
        }

        private void PrepareWeight()
        {
            if (!View.ActualSeries.Any(x => x is IScatterSeriesView || x is IHeatSeriesView)) return;

            var vs = View.ActualSeries
                .Select(x => x.ActualValues.GetTracker(x).WLimit)
                .DefaultIfEmpty(new CoreLimit()).ToArray();
            WLimit = new CoreLimit(vs.Select(x => x.Min).DefaultIfEmpty(0).Min(),
                vs.Select(x => x.Max).DefaultIfEmpty(0).Max());
        }

        private void PrepareUnitWidth()
        {
            foreach (var series in View.ActualSeries)
            {
                if (series is IStackedColumnSeriesView || series is IColumnSeriesView || 
                    series is IFinancialSeriesView || series is IHeatSeriesView)
                {
                    AxisX[series.ScalesXAt].EvaluatesUnitWidth = true;
                }
                if (series is IStackedRowSeriesView || series is IRowSeriesView || series is IHeatSeriesView)
                {
                    AxisY[series.ScalesYAt].EvaluatesUnitWidth = true;
                }
            }
        }

        private void PrepareStackedColumns()
        {
            if (!View.ActualSeries.Any(x => x is IStackedColumnSeriesView)) return;

            var isPercentage =
                View.ActualSeries.Any(x => x is IStackModelableSeriesView && x is IStackedColumnSeriesView &&
                                           ((IStackModelableSeriesView) x).StackMode == StackMode.Percentage);

            foreach (var group in View.ActualSeries.OfType<IStackedColumnSeriesView>().GroupBy(x => x.ScalesYAt))
            {
                StackPoints(group, AxisOrientation.Y, group.Key, isPercentage
                    ? StackMode.Percentage : StackMode.Values);
            }
        }

        private void PrepareStackedRows()
        {
            if (!View.ActualSeries.Any(x => x is IStackedRowSeriesView)) return;

            var isPercentage =
                View.ActualSeries.Any(x => x is IStackModelableSeriesView && x is IStackedRowSeriesView &&
                                     ((IStackModelableSeriesView) x).StackMode == StackMode.Percentage);

            foreach (var group in View.ActualSeries.OfType<IStackedRowSeriesView>().GroupBy(x => x.ScalesXAt))
            {
                StackPoints(group, AxisOrientation.X, group.Key, isPercentage ? StackMode.Percentage : StackMode.Values);
            }
        }

        private void PrepareStackedAreas()
        {
            if (!View.ActualSeries.Any(x => x is IStackedAreaSeriesView)) return;

            var isPercentage =
                View.ActualSeries.Any(x => x is IStackModelableSeriesView && x is IStackedAreaSeriesView &&
                                     ((IStackModelableSeriesView) x).StackMode == StackMode.Percentage);

            foreach (var group in View.ActualSeries.OfType<IStackedAreaSeriesView>().GroupBy(x => x.ScalesYAt))
            {
                StackPoints(group, AxisOrientation.Y, group.Key, isPercentage ? StackMode.Percentage : StackMode.Values);
            }
        }

        private void PrepareVerticalStackedAreas()
        {
            if (!View.ActualSeries.Any(x => x is IVerticalStackedAreaSeriesView)) return;

            var isPercentage =
                View.ActualSeries.Any(x => x is IStackModelableSeriesView && x is IVerticalStackedAreaSeriesView &&
                                     ((IStackModelableSeriesView) x).StackMode == StackMode.Percentage);

            foreach (var group in View.ActualSeries.OfType<IVerticalStackedAreaSeriesView>().GroupBy(x => x.ScalesXAt))
            {
                StackPoints(group, AxisOrientation.X, group.Key, isPercentage ? StackMode.Percentage : StackMode.Values);
            }
        }

        #endregion
    }
}
