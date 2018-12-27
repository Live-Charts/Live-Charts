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
    public class DataFactoryContext<TModel, TCoordinate> : IDataFactoryContext
        where TCoordinate : ICoordinate
    {
        private bool _isGiKnown;
        private int _gi;
        private bool _isScalesAtKnown;
        private int[] _scalesAt = new int[0];

        /// <summary>
        /// Inializes a new intance of the <see cref="DataFactoryContext{TModel, TCoordinate}"/> class.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="series"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="values"></param>
        public DataFactoryContext(
            ChartModel chart,
            ISeries series,
            UpdateContext context,
            ModelToCoordinateMapper<TModel, TCoordinate> mapper,
            IList<TModel> values)
        {
            Series = series;
            Mapper = mapper;
            Chart = chart;
            UpdateContext = context;
            Collection = values;
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public IList<TModel> Collection { get; private set; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        public ISeries Series { get; private set; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        public ModelToCoordinateMapper<TModel, TCoordinate> Mapper { get; private set; }

        /// <summary>
        /// Gets or sets the ranges.
        /// </summary>
        /// <value>
        /// The ranges.
        /// </value>
        public float[][][] Ranges { get; internal set; } = new float[0][][];

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
        public ChartModel Chart { get; private set; }
        
        /// <inheritdoc />
        public UpdateContext UpdateContext { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Ranges = new float[0][][];
            Collection.Clear();
        }
    }
}