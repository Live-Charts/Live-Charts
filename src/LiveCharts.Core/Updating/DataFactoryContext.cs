#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodr�guez Orozco & LiveCharts contributors
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
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction.Series;

#endregion

namespace LiveCharts.Core.Updating
{
    /// <summary>
    /// Point factory options class.
    /// </summary>
    public class DataFactoryContext<TModel, TCoordinate, TSeries> : IDataFactoryContext
        where TCoordinate : ICoordinate
        where TSeries : ISeries
    {
        private bool _isGiKnown;
        private int _gi;
        private bool _isScalesAtKnown;
        private int[] _scalesAt;

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
        public TSeries Series { get; internal set; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public ModelToCoordinateMapper<TModel, TCoordinate> Mapper { get; internal set; }

        /// <summary>
        /// Gets or sets the ranges.
        /// </summary>
        /// <value>
        /// The ranges.
        /// </value>
        public float[][][] Ranges { get; set; }

        /// <inheritdoc />
        public int[] SeriesScalesAt
        {
            get
            {
                if (_isScalesAtKnown) return _scalesAt;

                if (Series is IPieSeries) return Config.ScalesPieConst;

                if (!(Series is ICartesianSeries cartesianSeries))
                {
                    throw new LiveChartsException(101, null);
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

                _gi = Series.GroupingIndex;
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
            Series = default(TSeries);
            UpdateContext = null;
            Chart = null;
        }
    }
}