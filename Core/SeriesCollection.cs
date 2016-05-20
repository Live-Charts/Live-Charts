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
using LiveCharts.Charts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;

namespace LiveCharts
{
    public class SeriesCollection : NoisyCollection<ISeriesView>
    {
        #region Contructors

        public SeriesCollection()
        {
            CollectionChanged += OnCollectionChanged;
        }

        public SeriesCollection(object configuration)
        {
            Configuration = configuration;

            CollectionChanged += OnCollectionChanged;
        }

        #endregion
        
        public ChartCore Chart { get; internal set; }
        public object Configuration { get; set; }

        public SeriesCollection WithConfig<T>(IPointEvaluator<T> configuration)
        {
            Configuration = configuration;
            return this;
        }

        #region Obsoletes
        /// <summary>
        /// Setup a configuration for this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        [Obsolete]
        public SeriesCollection Setup<T>(SeriesConfiguration<T> config)
        {
            Configuration = config;
            return this;
        }
        #endregion

        private void OnCollectionChanged(IEnumerable<ISeriesView> oldItems, IEnumerable<ISeriesView> newItems)
        {
            if (oldItems != null)
            {
                foreach (var view in oldItems)
                {
                    view.Erase();
                }
            }

            if (Chart != null) Chart.Updater.Run();
        }

    }
}