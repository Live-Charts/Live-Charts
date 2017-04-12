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

using System.Collections.Generic;
using LiveCharts.Helpers;
using LiveCharts.Wpf.Charts.Base;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Stores a collection of axis.
    /// </summary>
    public class AxesCollection : NoisyCollection<Axis>
    {
        /// <summary>
        /// Initializes a new instance of AxisCollection class
        /// </summary>
        public AxesCollection()
        {
            NoisyCollectionChanged += OnNoisyCollectionChanged;
        }

        /// <summary>
        /// Gets the chart that owns the series.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public Chart Chart { get; internal set; }

        private void OnNoisyCollectionChanged(IEnumerable<Axis> oldItems, IEnumerable<Axis> newItems)
        {
            if(Chart != null && Chart.Model != null)
                Chart.Model.Updater.Run();

            if (oldItems == null) return;

            foreach (var oldAxis in oldItems)
            {
                oldAxis.Clean();
                if (oldAxis.Model == null) continue;
                var chart = oldAxis.Model.Chart.View;
                if (chart == null) continue;
                chart.RemoveFromView(oldAxis);
                chart.RemoveFromView(oldAxis.Separator);
            }
        }
    }
}
