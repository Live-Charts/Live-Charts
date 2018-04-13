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
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction;

#endregion

namespace LiveCharts.Core.Updater
{
    /// <summary>
    /// Point factory options class.
    /// </summary>
    public class DataFactoryContext<TModel, TCoordinate, TViewModel, TPoint> : IDataFactoryContext
        where TPoint : Point<TModel, TCoordinate, TViewModel>, new()
        where TCoordinate : ICoordinate
    {
        private bool _isGiKnown;
        private int _gi;
        private bool _isScalesAtKnown;
        private int[] _scalesAt;
        private static readonly int[] ScalesPie = {0, 0};

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public IList<TModel> Collection { get; internal set; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public Series<TModel, TCoordinate, TViewModel, TPoint> Series { get; internal set; }
        
        /// <inheritdoc />
        public float[][][] Ranges { get; set; }

        /// <inheritdoc />
        public int[] SeriesScalesAt
        {
            get
            {
                if (_isScalesAtKnown) return _scalesAt;

                if (Series is IPieSeries) return ScalesPie;

                if (!(Series is ICartesianSeries cartesianSeries))
                {
                    throw new LiveChartsException(
                        "It was not possible to determine the plane of the series.",
                        155);
                }

                _scalesAt = cartesianSeries.ScalesAt;
                _isScalesAtKnown = true;

                return _scalesAt;
            }
        }

        /// <inheritdoc />
        public int SeriesGroupingIndex
        {
            get
            {
                if (_isGiKnown) return _gi;

                _gi = ((ISeries) Series).GroupingIndex;
                _isGiKnown = true;

                return _gi;
            }
        }

        /// <inheritdoc />
        public ChartModel Chart { get; internal set; }
        
        /// <inheritdoc />
        public UpdateContext UpdateContext { get; internal set; }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Ranges = null;
            Collection = null;
            Series = null;
            UpdateContext = null;
            Chart = null;
        }
    }
}