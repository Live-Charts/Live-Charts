//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using LiveCharts.Helpers;
using LiveCharts.Maps;

namespace LiveCharts.Wpf.Maps
{
    public static class MapResolver
    {
        public static SvgMap Get(LiveCharts.Maps.Maps map)
        {
            string s;
            switch (map)
            {
                case LiveCharts.Maps.Maps.World:
                    s = "world.xml";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("map");
            }

            if (s == null) throw new LiveChartsException("Map not found");
        
            var svgMap = new SvgMap
            {
                DesiredHeight = 600,
                DesiredWidth = 800,
                Data = new List<MapData>()
            };

            using (var reader = XmlReader.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Maps\" + s)))
            {
                while (reader.Read())
                {
                    if (reader.Name == "livecharts:params")
                    {
                        var @params = XNode.ReadFrom(reader) as XElement;
                        if (@params == null) continue;
                        svgMap.DesiredHeight = double.Parse((string) @params.Attribute("desiredheight"));
                        svgMap.DesiredWidth = double.Parse((string) @params.Attribute("desiredwidth"));
                    }

                    if (reader.NodeType != XmlNodeType.Element || reader.Name != "path") continue;

                    var el = XNode.ReadFrom(reader) as XElement;
                    if (el == null) continue;
                    var p = new MapData
                    {
                        Id = (string) el.Attribute("id"),
                        Name = (string) el.Attribute("title"),
                        Data = (string) el.Attribute("d"),
                        SvgMap = svgMap
                    };
                    svgMap.Data.Add(p);
                }
            }
            return svgMap;
        }
    }
}
