using System;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing.Svg;

namespace LiveCharts.Core.Config
{
    /// <summary>
    /// Series configuration class.
    /// </summary>
    public class SeriesDefault
    {
        /// <summary>
        /// Strings this instance.
        /// </summary>
        /// <returns></returns>
        public SeriesDefault(string name)
        {
            Name = name;
            SkipCriteria = SeriesSkipCriteria.None;
            Geometry = Geometry.Circle;
            FillOpacity = .8;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the default fill opacity.
        /// </summary>
        /// <value>
        /// The default fill opacity.
        /// </value>
        public double FillOpacity { get; set; }

        /// <summary>
        /// Gets or sets the skip criteria.
        /// </summary>
        /// <value>
        /// The skip criteria.
        /// </value>
        public SeriesSkipCriteria SkipCriteria { get; set; }

        /// <summary>
        /// Gets or sets the default geometry.
        /// </summary>
        /// <value>
        /// The default geometry.
        /// </value>
        public Geometry Geometry { get; set; }
    }
}