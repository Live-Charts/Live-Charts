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
using System.Collections;
using System.Linq;

namespace LiveChartsCore
{
    public class CartesianChartCore : ChartCore
    {
        public CartesianChartCore(IChartView view) : base(view)
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
                //if (!Invert) continue;
                if (xi.Model.MaxValue == null) xi.Model.MaxLimit = (Math.Round(xi.Model.MaxLimit / xi.Model.S) + 1) * xi.Model.S;
                if (xi.Model.MinValue == null) xi.Model.MinLimit = (Math.Truncate(xi.Model.MinLimit / xi.Model.S) - 1) * xi.Model.S;
            }

            foreach (var yi in ay.Cast<IAxisView>())
            {
                yi.Model.CalculateSeparator(this, AxisTags.Y);
                //if (Invert) continue;
                if (yi.Model.MaxValue == null) yi.Model.MaxLimit = (Math.Round(yi.Model.MaxLimit / yi.Model.S) + 1) * yi.Model.S;
                if (yi.Model.MinValue == null) yi.Model.MinLimit = (Math.Truncate(yi.Model.MinLimit / yi.Model.S) - 1) * yi.Model.S;
            }

            CalculateComponentsAndMargin();
        }
    }
}
