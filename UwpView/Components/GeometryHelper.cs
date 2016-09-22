using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp.Components
{
    public class GeometryHelper
    {
        public static Geometry Parse(string data)
        {
            var sb = new StringBuilder();
            sb.Append("<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");
            sb.Append(data);
            sb.Append("</Geometry>");
            return (Geometry)XamlReader.Load(sb.ToString());
        }
    }
}
