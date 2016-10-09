﻿//The MIT License(MIT)

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
using Windows.UI.Xaml;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Uwp.Charts.Base;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// The Cartesian chart can plot any series with x and y coordinates
    /// </summary>
    public class CartesianChart : Chart, ICartesianChart
    {
        /// <summary>
        /// Initializes a new instance of CartesianChart class
        /// </summary>
        public CartesianChart()
        {
            var freq = DisableAnimations ? TimeSpan.FromMilliseconds(10) : AnimationsSpeed;
            var updater = new Components.ChartUpdater(freq);
            ChartCoreModel = new CartesianChartCore(this, updater);
            
            /*Current*/SetValue(SeriesProperty, new SeriesCollection());

            /*Current*/SetValue(VisualElementsProperty, new VisualElementsCollection());
        }


        public static readonly DependencyProperty VisualElementsProperty = DependencyProperty.Register(
            "VisualElements", typeof (VisualElementsCollection), typeof (CartesianChart),
            new PropertyMetadata(default(VisualElementsCollection)));
        /// <summary>
        /// Gets or sets the collection of visual elements in the chart, a visual element display another UiElement in the chart.
        /// </summary>
        public VisualElementsCollection VisualElements
        {
            get { return (VisualElementsCollection) GetValue(VisualElementsProperty); }
            set { SetValue(VisualElementsProperty, value); }
        }
    }
}
