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
using LiveCharts.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// Stores a collection of series to plot, this collection notifies the changes every time you add/remove any series.
    /// </summary>
    public class SeriesCollection : NoisyCollection<ISeriesView>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SeriesCollection class
        /// </summary>
        public SeriesCollection()
        {
            NoisyCollectionChanged += OnNoisyCollectionChanged;
            CollectionReset += OnCollectionReset;
        }

        /// <summary>
        /// Initializes a new instance of the SeriesCollection class, with a given mapper
        /// </summary>
        public SeriesCollection(object configuration)
        {
            Configuration = configuration;

            NoisyCollectionChanged += OnNoisyCollectionChanged;
            CollectionReset += OnCollectionReset;
        }

        #endregion

        /// <summary>
        /// Gets or sets the current series index, this index is used to pull out the automatic color of any series
        ///  </summary>
        public int CurrentSeriesIndex { get; set; }

        /// <summary>
        /// Gets the chart that owns the collection
        /// </summary>
        public ChartCore Chart { get; internal set; }
        /// <summary>
        /// Gets or sets then mapper in the collection, this mapper will be used in any series inside the collection, if null then LiveCharts will try to get the value from the global configuration.
        /// </summary>
        public object Configuration { get; set; }

        private void OnNoisyCollectionChanged(IEnumerable<ISeriesView> oldItems, IEnumerable<ISeriesView> newItems)
        {
            if (newItems != null)
            {
                foreach (var view in newItems)
                {
                    view.Model.SeriesCollection = this;
                    view.Model.Chart = Chart;
                }
            }

            if (oldItems != null)
            {
                foreach (var view in oldItems)
                {
                    view.Erase(true);
                }
            }
           
            if (Chart != null) Chart.Updater.Run();
        }

        private void OnCollectionReset()
        {
            CurrentSeriesIndex = 0;
        }
    }
}