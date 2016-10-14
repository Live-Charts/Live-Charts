using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp.Components
{
    public static class GeometryHelper
    {
        public static Geometry Parse(string data)
        {
            var parser = new GeometryParser();
            return parser.Convert(data);
            //var sb = new StringBuilder();
            //sb.Append("<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");
            //sb.Append(data);
            //sb.Append("</Geometry>");
            //var g = (Geometry) XamlReader.Load(sb.ToString());
            // Todo: Find out if this type could be frozen
            //g.Freeze(); ????
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
