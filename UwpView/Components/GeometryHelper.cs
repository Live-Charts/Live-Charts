using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp.Components
{
    public class GeometryHelper
    {
        public static Geometry Resolve(DefaultGeometries geometry)
        {
            var type = typeof(DefaultGeometries);
            var memInfo = type.GetMember(geometry.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(GeometryData), false).ToArray();
            if (!attributes.Any()) return null;
            var data = ((GeometryData) attributes[0]).Data;
            return Parse(data);
        }

        public static Geometry Parse(string data)
        {
            var sb = new StringBuilder();
            sb.Append("<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");
            sb.Append(data);
            sb.Append("</Geometry>");
            var g = (Geometry) XamlReader.Load(sb.ToString());
            // Todo: Find out if this type could be frozen
            //g.Freeze(); ????
            return g;
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
