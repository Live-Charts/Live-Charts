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

using LiveCharts.Charts;

namespace LiveCharts
{
    public abstract class SeriesAlgorithm
    {
        protected SeriesAlgorithm(ISeriesView view)
        {
            View = view;
        }

        public ISeriesView View { get; set; }
        public ChartCore Chart { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesOrientation SeriesOrientation { get; set; }
        public string Title { get; set; }
        public AxisCore CurrentXAxis
        {
            get { return Chart.AxisX[View.ScalesXAt]; }
        }
        public AxisCore CurrentYAxis
        {
            get { return Chart.AxisY[View.ScalesYAt]; }
        }

        public abstract void Update();
    }
}
