using System;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp.Components
{
    internal static class GeometryHelper
    {
        public static Geometry Parse(string data)
        {
            var parser = new GeometryParser();
            return parser.Convert(data);
        }

        public static Geometry Parse(this PointGeometry geometry)
        {
            return geometry == null ? null : Parse(geometry.Data);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class GeometryData : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryData"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public GeometryData(string data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }
    }
}
