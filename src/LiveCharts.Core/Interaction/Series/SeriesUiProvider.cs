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
using LiveCharts.Core.Interaction.Points;

#endregion

namespace LiveCharts.Core.Interaction.Series
{
    /// <summary>
    /// The series UI provider class.
    /// </summary>
    public interface ISeriesViewProvider<TModel, TCoordinate, TViewModel, TSeries>
        where TCoordinate : ICoordinate
        where TSeries : ISeries
    {
        /// <summary>
        /// Called when the series update starts.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="series">The series.</param>
        void OnUpdateStarted(IChartView chart, TSeries series);

        /// <summary>
        /// Called when LiveCharts requires a new visual point in the UI.
        /// </summary>
        /// <returns></returns>
        IPointView<TModel, TCoordinate, TViewModel, TSeries> GetNewPoint();

        /// <summary>the series update finishes.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="series">The series.</param>
        void OnUpdateFinished(IChartView chart, TSeries series);
    }
}