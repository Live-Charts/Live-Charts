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

using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction.Points;

#endregion

namespace LiveCharts.Core.Updating
{
    /// <summary>
    /// Defines a chart point factory.
    /// </summary>
    public interface IDataFactory
    {
        /// <summary>
        /// Fetches the data.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TSeries">The type of the series.</typeparam>
        /// <param name="context">The arguments.</param>
        /// <returns></returns>
        Point<TModel, TCoordinate, TViewModel, TSeries>[] Fetch<TModel, TCoordinate, TViewModel, TSeries>(
            DataFactoryContext<TModel, TCoordinate, TSeries> context)
            where TCoordinate : ICoordinate
            where TSeries : ISeries;
    }
}
