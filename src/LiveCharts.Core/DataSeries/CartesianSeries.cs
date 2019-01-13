#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

#endregion

#region

using LiveCharts.Coordinates;
using LiveCharts.Drawing.Shapes;

#endregion

namespace LiveCharts.DataSeries
{
    /// <summary>
    /// A series that requires at least a cartesian coordinate (X, Y).
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
    /// <typeparam name="TPointShape">The type of the point shape in the UI.</typeparam>
    /// <seealso cref="Series{TModel, TCoordinate, TPointShape}" />
    public abstract class CartesianStrokeSeries<TModel, TCoordinate, TPointShape>
        : StrokeSeries<TModel, TCoordinate, TPointShape>, ICartesianSeries
        where TCoordinate : ICoordinate
        where TPointShape : class, IShape
    {
        private int _zIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianStrokeSeries{TModel,TCoordinate, TPointShape}"/> class.
        /// </summary>
        protected CartesianStrokeSeries()
        {
            // A cartesian chart has 2 dimensions, X, Y
            // A cartesian chart can have as many axis as the user needs
            //      this means, There are always only 2 dimensions, X and Y
            //      but the user can define, for example multiple X axis
            //      this with the intention to allow the charts to compare
            //      trends, every axis has its own scale.
            //      see: https://lvcharts.net/App/examples/v1/wf/Multiple%20Axes
            // The ScaleAt array, for a cartesian series has 2 dimensions:
            //               {x, y}
            ScalesAt = new[] { 0, 0 };

            // This means that by default, any cartesian series is scaled at
            // the first element in the axis array for both, X and Y dimensions.
            // A user can change where the series is scaled using the properties
            // ScalesXAt and ScalesYAt, see properties below.
            // NOTE: notice we could get an OutOfRangeException if the index of this property
            // goes out of range with the CartesianChart.XAxis/YAxis array.
        }

        /// <inheritdoc />
        public int[] ScalesAt { get; protected set; }

        /// <summary>
        /// Gets or sets the index of the axis where the X coordinate is scaled at.
        /// </summary>
        /// <value>
        /// The scales x at.
        /// </value>
        public int ScalesXAt
        {
            get => ScalesAt[0];
            set
            {
                ScalesAt[0] = value;
                OnPropertyChanged(nameof(ScalesXAt));
            }
        }

        /// <summary>
        /// Gets or sets the index of the axis where the Y coordinate is scaled at.
        /// </summary>
        /// <value>
        /// The scales y at.
        /// </value>
        public int ScalesYAt
        {
            get => ScalesAt[1];
            set
            {
                ScalesAt[1] = value;
                OnPropertyChanged(nameof(ScalesYAt));
            }
        }

        /// <inheritdoc />
        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value;
                OnPropertyChanged(nameof(ZIndex));
            }
        }
    }
}