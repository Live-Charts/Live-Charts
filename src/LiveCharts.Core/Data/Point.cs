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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Events;
using LiveCharts.Core.Interaction;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// Represents a point int he chart.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref="T:System.IDisposable" />
    public class Point<TModel, TCoordinate, TViewModel> : IResource
        where TCoordinate : ICoordinate
    {
        /// <summary>
        /// Gets the key of the point, a key is used internally as a unique identifier in 
        /// in a <see cref="Series"/> 
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public int Key { get; internal set; }

        /// <summary>
        /// Gets the instance represented by this point.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public TModel Model { get; internal set; }

        /// <summary>
        /// Gets the view model,the model to drawn in the user interface.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public TViewModel ViewModel { get; internal set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public IPointView<TModel, Point<TModel, TCoordinate, TViewModel>, TCoordinate, TViewModel>
            View { get; internal set; }

        /// <summary>
        /// Gets the point coordinate.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public TCoordinate Coordinate { get; internal set; }

        /// <summary>
        /// Gets the hover area.
        /// </summary>
        /// <value>
        /// The hover area.
        /// </value>
        public HoverArea HoverArea { get; internal set; }

        /// <summary>
        /// Gets the series that owns the point.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public DataSeries.BaseSeries Series { get; internal set; }

        /// <summary>
        /// Gets the chart that owns the point.
        /// </summary>
        /// <value>
        /// The chart.
        /// </value>
        public ChartModel Chart { get; internal set; }

        /// <summary>
        /// returns a point with an unknown.
        /// </summary>
        /// <returns></returns>
        public PackedPoint<TModel, TCoordinate> Pack()
        {
            return new PackedPoint<TModel, TCoordinate>
            {
                Chart = Chart,
                Coordinate = Coordinate,
                Key = Key,
                Model = Model,
                Series = Series,
                View = null,
                ViewModel = ViewModel
            };
        }

        /// <summary>
        /// Packs all.
        /// </summary>
        /// <returns></returns>
        public PackedPoint PackAll()
        {
            return new PackedPoint
            {
                Chart = Chart,
                Coordinate = Coordinate,
                Key = Key,
                Model = Model,
                Series = Series,
                View = null,
                ViewModel = ViewModel
            };
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Coordinate.ToString();
        }

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view)
        {
            View.Dispose(view);
            Disposed?.Invoke(view, this);
        }
    }
}
