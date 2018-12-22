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

using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Shapes;
using LiveCharts.Core.Interaction.ChartAreas;
using LiveCharts.Core.Interaction.Events;
#endregion

namespace LiveCharts.Core.Interaction.Points
{
    /// <summary>
    /// Represents a point int he chart.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
    /// <typeparam name="TPointShape">The type of the point shape in hte UI.</typeparam>
    /// <seealso cref="T:System.IDisposable" />
    public class ChartPoint<TModel, TCoordinate, TPointShape> 
        : IResource, IChartPoint<TModel, TCoordinate>
        where TCoordinate : ICoordinate
        where TPointShape : class, IShape
    {
        /// <inheritdoc />
        public int Key { get; internal set; }

        /// <inheritdoc />
        public TModel Model { get; internal set; }

        object IChartPoint.Model => Model;

        /// <summary>
        /// Gets the shape.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        public TPointShape Shape { get; internal set; }

        IShape IChartPoint.Shape => Shape;

        /// <inheritdoc />
        public ILabel Label { get; internal set; }

        /// <inheritdoc />
        public TCoordinate Coordinate { get; internal set; }

        ICoordinate IChartPoint.Coordinate => Coordinate;

        /// <inheritdoc />
        public InteractionArea InteractionArea { get; internal set; }

        public ISeries Series { get; internal set; }

        /// <inheritdoc />
        public IChartView Chart { get; internal set; }

        /// <inheritdoc />
        public string ToolTipText => Series.GetTooltipLabel(Coordinate);

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Coordinate.ToString();
        }

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view, bool force)
        {
            Disposed?.Invoke(view, this, force);
        }
    }
}
