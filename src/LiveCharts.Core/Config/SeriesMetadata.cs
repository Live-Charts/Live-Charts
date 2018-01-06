using System;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Config
{
    /// <summary>
    /// Series configuration class.
    /// </summary>
    public class SeriesMetadata
    {
        public SeriesMetadata()
        {
            SkipCriteria = SeriesSkipCriteria.None;
            PointViewProvider = () => throw new NotImplementedException("A view provider was not found!");
        }

        /// <summary>
        /// Gets or sets the default fill opacity.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        public double FillOpacity { get; set; }

        /// <summary>
        /// Gets or sets the point view provider.
        /// </summary>
        /// <value>
        /// The point view provider.
        /// </value>
        public Func<IPointView> PointViewProvider { get; set; }

        /// <summary>
        /// Gets or sets the skip criteria.
        /// </summary>
        /// <value>
        /// The skip criteria.
        /// </value>
        public SeriesSkipCriteria SkipCriteria { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the type of the chart point.
        /// </summary>
        /// <value>
        /// The type of the chart point.
        /// </value>
        public ChartPointTypes ChartPointType { get; set; }

        /// <summary>
        /// Gets or sets the default geometry.
        /// </summary>
        /// <value>
        /// The default geometry.
        /// </value>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Set the fill opacity.
        /// </summary>
        /// <param name="opacity">The opacity.</param>
        /// <returns></returns>
        public SeriesMetadata WithFillOpacity(double opacity)
        {
            FillOpacity = opacity;
            return this;
        }

        /// <summary>
        /// Set the point view provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public SeriesMetadata WithPointViewProvider(Func<IPointView> provider)
        {
            PointViewProvider = provider;
            return this;
        }

        /// <summary>
        /// Set the skip criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public SeriesMetadata WithSkipCriteria(SeriesSkipCriteria criteria)
        {
            SkipCriteria = criteria;
            return this;
        }

        /// <summary>
        /// Set the type of the point.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public SeriesMetadata WithPointType(ChartPointTypes type)
        {
            ChartPointType = type;
            return this;
        }

        /// <summary>
        /// Set the default geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns></returns>
        public SeriesMetadata WithDefaultGeometry(Geometry geometry)
        {
            Geometry = geometry;
            return this;
        }
    }
}