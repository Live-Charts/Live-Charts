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

namespace LiveChartsCore
{
    public class Series : ISeriesModel
    {
        public Series(ISeriesView view)
        {
            View = view;
        }

        public Series(ISeriesView view, SeriesConfiguration configuration)
        {
            View = view;
            Configuration = configuration;
        }

        public ISeriesView View { get; set; }
        public IChartModel Chart { get; set; }
        public IChartValues Values { get; set; }
        public SeriesConfiguration Configuration { get; set; }
        public int ScalesXAt { get; set; }
        public int ScalesYAt { get; set; }
        //public Axis CurrentXAxis
        //{
        //    get { return Chart.AxisX[ScalesXAt]; }
        //}

        //public Axis CurrentYAxis
        //{
        //    get { return Chart.AxisY[ScalesYAt]; }
        //}
        public SeriesCollection SeriesCollection { get; set; }
        public string Title { get; set; }
        public SeriesCollection Collection { get; private set; }

        /// <summary>
        /// Setup a configuration for this collection, notice this method returns the current instance to support fleunt syntax. if Series.Cofiguration is not null then SeriesCollection.Configuration will be ignored.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public Series Setup<T>(SeriesConfiguration<T> config)
        {
            Configuration = config;
            return this;
        }
    }
}
