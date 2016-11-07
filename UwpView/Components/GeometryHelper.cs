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

    public class GeometryData : Attribute
    {
        public GeometryData(string data)
        {
            Data = data;
        }

        public string Data { get; set; }
    }
}
