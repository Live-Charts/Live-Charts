#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

#endregion

namespace LiveCharts.Core.Drawing
{
    /// <summary>
    /// A predefined brushes collection, based on system.windows.media brushes.
    /// </summary>
    public static class Brushes
    {
        private static Dictionary<string, SolidColorBrush> _b;

        private static Dictionary<string, SolidColorBrush> BrushesByName
        {
            get
            {
                if (_b != null) return _b;

                using (var stream = new StreamReader("solidbrushes.txt"))
                {
                    var content = stream.ReadToEnd();
                    _b = content.Split('#')
                        .Where(x => x.Length > 1)
                        .Select(x =>
                        {
                            // AliceBlue,240,248,255
                            var item = x.Split(',');
                            return new
                            {
                                name = item[0],
                                r = int.Parse(item[1]),
                                g = int.Parse(item[2]),
                                b = int.Parse(item[3])
                            };
                        }).ToDictionary(
                            x => x.name,
                            x => new SolidColorBrush(Color.FromArgb(255, x.r, x.g, x.b)));
                }

                return _b;
            }
        }

        public static SolidColorBrush AliceBlue => BrushesByName["AliceBlue"];
        public static SolidColorBrush AntiqueWhite => BrushesByName["AntiqueWhite"];
        public static SolidColorBrush Aqua => BrushesByName["Aqua"];
        public static SolidColorBrush Aquamarine => BrushesByName["Aquamarine"];
        public static SolidColorBrush Azure => BrushesByName["Azure"];
        public static SolidColorBrush Beige => BrushesByName["Beige"];
        public static SolidColorBrush Bisque => BrushesByName["Bisque"];
        public static SolidColorBrush Black => BrushesByName["Black"];
        public static SolidColorBrush BlanchedAlmond => BrushesByName["BlanchedAlmond"];
        public static SolidColorBrush Blue => BrushesByName["Blue"];
        public static SolidColorBrush BlueViolet => BrushesByName["BlueViolet"];
        public static SolidColorBrush Brown => BrushesByName["Brown"];
        public static SolidColorBrush BurlyWood => BrushesByName["BurlyWood"];
        public static SolidColorBrush CadetBlue => BrushesByName["CadetBlue"];
        public static SolidColorBrush Chartreuse => BrushesByName["Chartreuse"];
        public static SolidColorBrush Chocolate => BrushesByName["Chocolate"];
        public static SolidColorBrush Coral => BrushesByName["Coral"];
        public static SolidColorBrush CornflowerBlue => BrushesByName["CornflowerBlue"];
        public static SolidColorBrush Cornsilk => BrushesByName["Cornsilk"];
        public static SolidColorBrush Crimson => BrushesByName["Crimson"];
        public static SolidColorBrush Cyan => BrushesByName["Cyan"];
        public static SolidColorBrush DarkBlue => BrushesByName["DarkBlue"];
        public static SolidColorBrush DarkCyan => BrushesByName["DarkCyan"];
        public static SolidColorBrush DarkGoldenrod => BrushesByName["DarkGoldenrod"];
        public static SolidColorBrush DarkGray => BrushesByName["DarkGray"];
        public static SolidColorBrush DarkGreen => BrushesByName["DarkGreen"];
        public static SolidColorBrush DarkKhaki => BrushesByName["DarkKhaki"];
        public static SolidColorBrush DarkMagenta => BrushesByName["DarkMagenta"];
        public static SolidColorBrush DarkOliveGreen => BrushesByName["DarkOliveGreen"];
        public static SolidColorBrush DarkOrange => BrushesByName["DarkOrange"];
        public static SolidColorBrush DarkOrchid => BrushesByName["DarkOrchid"];
        public static SolidColorBrush DarkRed => BrushesByName["DarkRed"];
        public static SolidColorBrush DarkSalmon => BrushesByName["DarkSalmon"];
        public static SolidColorBrush DarkSeaGreen => BrushesByName["DarkSeaGreen"];
        public static SolidColorBrush DarkSlateBlue => BrushesByName["DarkSlateBlue"];
        public static SolidColorBrush DarkSlateGray => BrushesByName["DarkSlateGray"];
        public static SolidColorBrush DarkTurquoise => BrushesByName["DarkTurquoise"];
        public static SolidColorBrush DarkViolet => BrushesByName["DarkViolet"];
        public static SolidColorBrush DeepPink => BrushesByName["DeepPink"];
        public static SolidColorBrush DeepSkyBlue => BrushesByName["DeepSkyBlue"];
        public static SolidColorBrush DimGray => BrushesByName["DimGray"];
        public static SolidColorBrush DodgerBlue => BrushesByName["DodgerBlue"];
        public static SolidColorBrush Firebrick => BrushesByName["Firebrick"];
        public static SolidColorBrush FloralWhite => BrushesByName["FloralWhite"];
        public static SolidColorBrush ForestGreen => BrushesByName["ForestGreen"];
        public static SolidColorBrush Fuchsia => BrushesByName["Fuchsia"];
        public static SolidColorBrush Gainsboro => BrushesByName["Gainsboro"];
        public static SolidColorBrush GhostWhite => BrushesByName["GhostWhite"];
        public static SolidColorBrush Gold => BrushesByName["Gold"];
        public static SolidColorBrush Goldenrod => BrushesByName["Goldenrod"];
        public static SolidColorBrush Gray => BrushesByName["Gray"];
        public static SolidColorBrush Green => BrushesByName["Green"];
        public static SolidColorBrush GreenYellow => BrushesByName["GreenYellow"];
        public static SolidColorBrush Honeydew => BrushesByName["Honeydew"];
        public static SolidColorBrush HotPink => BrushesByName["HotPink"];
        public static SolidColorBrush IndianRed => BrushesByName["IndianRed"];
        public static SolidColorBrush Indigo => BrushesByName["Indigo"];
        public static SolidColorBrush Ivory => BrushesByName["Ivory"];
        public static SolidColorBrush Khaki => BrushesByName["Khaki"];
        public static SolidColorBrush Lavender => BrushesByName["Lavender"];
        public static SolidColorBrush LavenderBlush => BrushesByName["LavenderBlush"];
        public static SolidColorBrush LawnGreen => BrushesByName["LawnGreen"];
        public static SolidColorBrush LemonChiffon => BrushesByName["LemonChiffon"];
        public static SolidColorBrush LightBlue => BrushesByName["LightBlue"];
        public static SolidColorBrush LightCoral => BrushesByName["LightCoral"];
        public static SolidColorBrush LightCyan => BrushesByName["LightCyan"];
        public static SolidColorBrush LightGoldenrodYellow => BrushesByName["LightGoldenrodYellow"];
        public static SolidColorBrush LightGray => BrushesByName["LightGray"];
        public static SolidColorBrush LightGreen => BrushesByName["LightGreen"];
        public static SolidColorBrush LightPink => BrushesByName["LightPink"];
        public static SolidColorBrush LightSalmon => BrushesByName["LightSalmon"];
        public static SolidColorBrush LightSeaGreen => BrushesByName["LightSeaGreen"];
        public static SolidColorBrush LightSkyBlue => BrushesByName["LightSkyBlue"];
        public static SolidColorBrush LightSlateGray => BrushesByName["LightSlateGray"];
        public static SolidColorBrush LightSteelBlue => BrushesByName["LightSteelBlue"];
        public static SolidColorBrush LightYellow => BrushesByName["LightYellow"];
        public static SolidColorBrush Lime => BrushesByName["Lime"];
        public static SolidColorBrush LimeGreen => BrushesByName["LimeGreen"];
        public static SolidColorBrush Linen => BrushesByName["Linen"];
        public static SolidColorBrush Magenta => BrushesByName["Magenta"];
        public static SolidColorBrush Maroon => BrushesByName["Maroon"];
        public static SolidColorBrush MediumAquamarine => BrushesByName["MediumAquamarine"];
        public static SolidColorBrush MediumBlue => BrushesByName["MediumBlue"];
        public static SolidColorBrush MediumOrchid => BrushesByName["MediumOrchid"];
        public static SolidColorBrush MediumPurple => BrushesByName["MediumPurple"];
        public static SolidColorBrush MediumSeaGreen => BrushesByName["MediumSeaGreen"];
        public static SolidColorBrush MediumSlateBlue => BrushesByName["MediumSlateBlue"];
        public static SolidColorBrush MediumSpringGreen => BrushesByName["MediumSpringGreen"];
        public static SolidColorBrush MediumTurquoise => BrushesByName["MediumTurquoise"];
        public static SolidColorBrush MediumVioletRed => BrushesByName["MediumVioletRed"];
        public static SolidColorBrush MidnightBlue => BrushesByName["MidnightBlue"];
        public static SolidColorBrush MintCream => BrushesByName["MintCream"];
        public static SolidColorBrush MistyRose => BrushesByName["MistyRose"];
        public static SolidColorBrush Moccasin => BrushesByName["Moccasin"];
        public static SolidColorBrush NavajoWhite => BrushesByName["NavajoWhite"];
        public static SolidColorBrush Navy => BrushesByName["Navy"];
        public static SolidColorBrush OldLace => BrushesByName["OldLace"];
        public static SolidColorBrush Olive => BrushesByName["Olive"];
        public static SolidColorBrush OliveDrab => BrushesByName["OliveDrab"];
        public static SolidColorBrush Orange => BrushesByName["Orange"];
        public static SolidColorBrush OrangeRed => BrushesByName["OrangeRed"];
        public static SolidColorBrush Orchid => BrushesByName["Orchid"];
        public static SolidColorBrush PaleGoldenrod => BrushesByName["PaleGoldenrod"];
        public static SolidColorBrush PaleGreen => BrushesByName["PaleGreen"];
        public static SolidColorBrush PaleTurquoise => BrushesByName["PaleTurquoise"];
        public static SolidColorBrush PaleVioletRed => BrushesByName["PaleVioletRed"];
        public static SolidColorBrush PapayaWhip => BrushesByName["PapayaWhip"];
        public static SolidColorBrush PeachPuff => BrushesByName["PeachPuff"];
        public static SolidColorBrush Peru => BrushesByName["Peru"];
        public static SolidColorBrush Pink => BrushesByName["Pink"];
        public static SolidColorBrush Plum => BrushesByName["Plum"];
        public static SolidColorBrush PowderBlue => BrushesByName["PowderBlue"];
        public static SolidColorBrush Purple => BrushesByName["Purple"];
        public static SolidColorBrush Red => BrushesByName["Red"];
        public static SolidColorBrush RosyBrown => BrushesByName["RosyBrown"];
        public static SolidColorBrush RoyalBlue => BrushesByName["RoyalBlue"];
        public static SolidColorBrush SaddleBrown => BrushesByName["SaddleBrown"];
        public static SolidColorBrush Salmon => BrushesByName["Salmon"];
        public static SolidColorBrush SandyBrown => BrushesByName["SandyBrown"];
        public static SolidColorBrush SeaGreen => BrushesByName["SeaGreen"];
        public static SolidColorBrush SeaShell => BrushesByName["SeaShell"];
        public static SolidColorBrush Sienna => BrushesByName["Sienna"];
        public static SolidColorBrush Silver => BrushesByName["Silver"];
        public static SolidColorBrush SkyBlue => BrushesByName["SkyBlue"];
        public static SolidColorBrush SlateBlue => BrushesByName["SlateBlue"];
        public static SolidColorBrush SlateGray => BrushesByName["SlateGray"];
        public static SolidColorBrush Snow => BrushesByName["Snow"];
        public static SolidColorBrush SpringGreen => BrushesByName["SpringGreen"];
        public static SolidColorBrush SteelBlue => BrushesByName["SteelBlue"];
        public static SolidColorBrush Tan => BrushesByName["Tan"];
        public static SolidColorBrush Teal => BrushesByName["Teal"];
        public static SolidColorBrush Thistle => BrushesByName["Thistle"];
        public static SolidColorBrush Tomato => BrushesByName["Tomato"];
        public static SolidColorBrush Transparent => BrushesByName["Transparent"];
        public static SolidColorBrush Turquoise => BrushesByName["Turquoise"];
        public static SolidColorBrush Violet => BrushesByName["Violet"];
        public static SolidColorBrush Wheat => BrushesByName["Wheat"];
        public static SolidColorBrush White => BrushesByName["White"];
        public static SolidColorBrush WhiteSmoke => BrushesByName["WhiteSmoke"];
        public static SolidColorBrush Yellow => BrushesByName["Yellow"];
        public static SolidColorBrush YellowGreen => BrushesByName["YellowGreen"];
    }
}
