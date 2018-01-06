using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Config
{
    /// <summary>
    /// Series configuration class.
    /// </summary>
    public class SeriesMetadata
    {
        internal double DefaultFillOpacity { get; set; }

        internal Func<IPointView> PointViewProvider { get; set; }

        internal SeriesSkipCriteria SkipCriteria { get; set; }

        internal string Type { get; set; }

        internal ChartPointTypes ChartPointType { get; set; }

        /// <summary>
        /// Withes the fill opacity.
        /// </summary>
        /// <param name="opacity">The opacity.</param>
        /// <returns></returns>
        public SeriesMetadata WithFillOpacity(double opacity)
        {
            DefaultFillOpacity = opacity;
            return this;
        }

        /// <summary>
        /// Withes the point view provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public SeriesMetadata WithPointViewProvider(Func<IPointView> provider)
        {
            PointViewProvider = provider;
            return this;
        }

        /// <summary>
        /// Withes the skip criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public SeriesMetadata WithSkipCriteria(SeriesSkipCriteria criteria)
        {
            SkipCriteria = criteria;
            return this;
        }

        /// <summary>
        /// Withes the type of the point.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public SeriesMetadata WithPointType(ChartPointTypes type)
        {
            ChartPointType = type;
            return this;
        }
    }
}