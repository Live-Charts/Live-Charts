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

using System.Linq;
using Windows.UI.Xaml;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// An Logarithmic Axis of a chart
    /// </summary>
    /// <seealso cref="Axis" />
    public class LogarithmicAxis : Axis, ILogarithmicAxisView
    {
        /// <summary>
        /// Ases the core element.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public override AxisCore AsCoreElement(ChartCore chart, AxisOrientation source)
        {
            if (Model == null) Model = new LogarithmicAxisCore(this);

            Model.ShowLabels = ShowLabels;
            Model.Chart = chart;
            Model.IsMerged = IsMerged;
            Model.Labels = Labels;
            Model.LabelFormatter = LabelFormatter;
            Model.MaxValue = MaxValue;
            Model.MinValue = MinValue;
            Model.Title = Title;
            Model.Position = Position;
            Model.Separator = Separator.AsCoreElement(Model, source);
            Model.DisableAnimations = DisableAnimations;
            Model.Sections = Sections.Select(x => x.AsCoreElement(Model, source)).ToList();

            return Model;
        }

        /// <summary>
        /// The base property
        /// </summary>
        public static readonly DependencyProperty BaseProperty = DependencyProperty.Register(
            "Base", typeof(double), typeof(LogarithmicAxis), new PropertyMetadata(10d, UpdateChart()));
        /// <summary>
        /// Gets or sets the base.
        /// </summary>
        /// <value>
        /// The base.
        /// </value>
        public double Base
        {
            get { return (double)GetValue(BaseProperty); }
            set { SetValue(BaseProperty, value); }
        }
    }
}