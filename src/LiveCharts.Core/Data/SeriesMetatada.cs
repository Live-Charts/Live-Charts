using System;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// A class containing series information.
    /// </summary>
    public struct SeriesMetatada
    {
        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is referenced type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is referenced type; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueType { get; set; }
    }
}
