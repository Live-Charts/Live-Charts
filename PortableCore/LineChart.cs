using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiveChartsCore
{
    public class LineChartModel : Chart
    {
        public LineChartModel(IChartView view) : base(view)
        {
        }

        public override void PrepareAxes()
        {
            var ax = AxisX as IList;
            var ay = AxisY as IList;

            base.PrepareAxes();

            foreach (var xi in ax.Cast<IAxisView>())
            {
                xi.Model.CalculateSeparator(this, AxisTags.X);
                if (!Invert) continue;
                if (xi.Model.MaxValue == null) xi.Model.MaxLimit = (Math.Round(xi.Model.MaxLimit / xi.Model.S) + 1) * xi.Model.S;
                if (xi.Model.MinValue == null) xi.Model.MinLimit = (Math.Truncate(xi.Model.MinLimit / xi.Model.S) - 1) * xi.Model.S;
            }

            foreach (var yi in ay.Cast<IAxisView>())
            {
                yi.Model.CalculateSeparator(this, AxisTags.Y);
                if (Invert) continue;
                if (yi.Model.MaxValue == null) yi.Model.MaxLimit = (Math.Round(yi.Model.MaxLimit / yi.Model.S) + 1) * yi.Model.S;
                if (yi.Model.MinValue == null) yi.Model.MinLimit = (Math.Truncate(yi.Model.MinLimit / yi.Model.S) - 1) * yi.Model.S;
            }

            CalculateComponentsAndMargin();
        }
    }
}
