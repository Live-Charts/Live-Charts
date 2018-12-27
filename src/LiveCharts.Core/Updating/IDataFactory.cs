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

using System.Collections.Generic;
using LiveCharts.Coordinates;
using LiveCharts.Drawing.Shapes;
using LiveCharts.Interaction.Points;

#endregion

namespace LiveCharts.Updating
{
    /// <summary>
    /// Defines a chart point factory.
    /// </summary>
    public interface IDataFactory
    {
        /// <summary>
        /// Fetches the data.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to plot.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
        /// <typeparam name="TPointShape">The type of the point shape in the UI.</typeparam>
        /// <param name="context">The arguments.</param>
        /// <param name="tracker">The tracker instance.</param>
        /// <param name="count">The points count.</param>
        /// <returns></returns>
        void Fetch<TModel, TCoordinate, TPointShape>(
            DataFactoryContext<TModel, TCoordinate> context,
            Dictionary<object, ChartPoint<TModel, TCoordinate, TPointShape>> tracker,
            out int count)
            where TCoordinate : ICoordinate
            where TPointShape : class, IShape;
    }
}
