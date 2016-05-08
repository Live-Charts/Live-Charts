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

namespace LiveCharts.Charts
{
    public class CartesianChartCore : ChartCore
    {
        public CartesianChartCore(IChartView view, IChartUpdater updater) : base(view, updater)
        {
            updater.Chart = this;
            updater.UpdateFrequency();
        }

        public override void PrepareAxes()
        {
            base.PrepareAxes();

            if (View.Series.Any(x => !(x.Model is ICartesianSeries)))
                throw new Exception(
                    "There is a invalid series in the series collection, " +
                    "verify that all the series are desiged to be plotted in a cartesian chart.");

            var cartesianSeries = View.Series.Select(x => x.Model).Cast<ICartesianSeries>().ToArray();

            if (View.Series.Any(x => x is IBubbleSeries))
            {
                var vs = View.Series.Select(x => x.Values.Value3Limit).ToArray();
                Value3Limit = new Limit(vs.Select(x => x.Min).DefaultIfEmpty(0).Min(),
                    vs.Select(x => x.Max).DefaultIfEmpty(0).Max());
            }

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                xi.CalculateSeparator(this, AxisTags.X);
                SetAxisMode(cartesianSeries.Select(x => x.XAxisMode), xi, index, AxisTags.X);
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                yi.CalculateSeparator(this, AxisTags.Y);
                SetAxisMode(cartesianSeries.Select(x => x.YAxisMode), yi, index, AxisTags.Y);
            }

            CalculateComponentsAndMargin();
        }
    
        private void SetAxisMode(IEnumerable<AxisLimitsMode> modes, AxisCore axis, int index, AxisTags source)
        {
            var mode = EvaluateModes(modes);

            var rMax = axis.MaxLimit / axis.Magnitude;
            var rMin = axis.MinLimit/axis.Magnitude;

            switch (mode)
            {
                case AxisLimitsMode.Stretch:
                    axis.EvaluatesUnitWidth = false;
                    axis.MaxLimit = Math.Ceiling(rMax)*axis.Magnitude;
                    axis.MinLimit = Math.Floor(rMin)*axis.Magnitude;
                    break;
                case AxisLimitsMode.UnitWidth:
                    axis.EvaluatesUnitWidth = true;
                    axis.MaxLimit = Math.Ceiling(rMax)*axis.Magnitude + 1;
                    axis.MinLimit = Math.Floor(rMin)*axis.Magnitude;
                    break;
                case AxisLimitsMode.Separator:
                    axis.EvaluatesUnitWidth = false;
                    var sMax = axis.MaxLimit/axis.S;
                    var sMin = axis.MinLimit/axis.S;
                    axis.MaxLimit = Math.Truncate(sMax + 1)*axis.S;
                    axis.MinLimit = Math.Truncate(sMin - 1)*axis.S;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!(Math.Abs(axis.MaxLimit - axis.MinLimit) < axis.S*0.01)) return;

            axis.MinLimit = (Math.Round(rMax) - 1) * axis.S;
            axis.MaxLimit = (Math.Round(rMax) + 1) * axis.S;
        }

        private static AxisLimitsMode EvaluateModes(IEnumerable<AxisLimitsMode> modes)
        {
            var g = modes.GroupBy(x => x).Select(x => x.Key).ToList();

            if (g.Any(x => x == AxisLimitsMode.Separator))
                return AxisLimitsMode.Separator;

            if (g.Any(x => x == AxisLimitsMode.UnitWidth))
                return AxisLimitsMode.UnitWidth;

            return AxisLimitsMode.Stretch;
        }
    }
}
