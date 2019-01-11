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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#endregion

namespace LiveCharts.Drawing.Brushes
{
    /// <summary>
    /// A predefined brushes collection, based on system.windows.media brushes.
    /// </summary>
    public static class Brushes
    {
        private static Dictionary<string, byte[]> _b;

        private static Dictionary<string, byte[]> BrushesByName
        {
            get
            {
                if (_b != null)
                {
                    return _b;
                }

                var assembly = Assembly.GetExecutingAssembly();
                const string resourceName = "LiveCharts.Core.Assets.solidbrushes.txt";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    string content = reader.ReadToEnd();
                    _b = content.Split('#')
                        .Where(x => x.Length > 1)
                        .Select(x =>
                        {
                            // AliceBlue,240,248,255
                            string[] item = x.Split(',');
                            return new Color
                            {
                                Name = item[0],
                                R = byte.Parse(item[1]),
                                G = byte.Parse(item[2]),
                                B = byte.Parse(item[3])
                            };
                        }).ToDictionary(
                            x => x.Name,
                            x => new[] { x.R, x.G, x.B });
                }

                return _b;
            }
        }

        private struct Color
        {
            public string Name { get; set; }
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
        }

        private static SolidColorBrush GetNewBrushForUi(byte[] components)
        {
            return new SolidColorBrush(255, components[0], components[1], components[2]);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static SolidColorBrush AliceBlue => GetNewBrushForUi(BrushesByName["AliceBlue"]);
        public static SolidColorBrush AntiqueWhite => GetNewBrushForUi(BrushesByName["AntiqueWhite"]);
        public static SolidColorBrush Aqua => GetNewBrushForUi(BrushesByName["Aqua"]);
        public static SolidColorBrush Aquamarine => GetNewBrushForUi(BrushesByName["Aquamarine"]);
        public static SolidColorBrush Azure => GetNewBrushForUi(BrushesByName["Azure"]);
        public static SolidColorBrush Beige => GetNewBrushForUi(BrushesByName["Beige"]);
        public static SolidColorBrush Bisque => GetNewBrushForUi(BrushesByName["Bisque"]);
        public static SolidColorBrush Black => GetNewBrushForUi(BrushesByName["Black"]);
        public static SolidColorBrush BlanchedAlmond => GetNewBrushForUi(BrushesByName["BlanchedAlmond"]);
        public static SolidColorBrush Blue => GetNewBrushForUi(BrushesByName["Blue"]);
        public static SolidColorBrush BlueViolet => GetNewBrushForUi(BrushesByName["BlueViolet"]);
        public static SolidColorBrush Brown => GetNewBrushForUi(BrushesByName["Brown"]);
        public static SolidColorBrush BurlyWood => GetNewBrushForUi(BrushesByName["BurlyWood"]);
        public static SolidColorBrush CadetBlue => GetNewBrushForUi(BrushesByName["CadetBlue"]);
        public static SolidColorBrush Chartreuse => GetNewBrushForUi(BrushesByName["Chartreuse"]);
        public static SolidColorBrush Chocolate => GetNewBrushForUi(BrushesByName["Chocolate"]);
        public static SolidColorBrush Coral => GetNewBrushForUi(BrushesByName["Coral"]);
        public static SolidColorBrush CornflowerBlue => GetNewBrushForUi(BrushesByName["CornflowerBlue"]);
        public static SolidColorBrush Cornsilk => GetNewBrushForUi(BrushesByName["Cornsilk"]);
        public static SolidColorBrush Crimson => GetNewBrushForUi(BrushesByName["Crimson"]);
        public static SolidColorBrush Cyan => GetNewBrushForUi(BrushesByName["Cyan"]);
        public static SolidColorBrush DarkBlue => GetNewBrushForUi(BrushesByName["DarkBlue"]);
        public static SolidColorBrush DarkCyan => GetNewBrushForUi(BrushesByName["DarkCyan"]);
        public static SolidColorBrush DarkGoldenrod => GetNewBrushForUi(BrushesByName["DarkGoldenrod"]);
        public static SolidColorBrush DarkGray => GetNewBrushForUi(BrushesByName["DarkGray"]);
        public static SolidColorBrush DarkGreen => GetNewBrushForUi(BrushesByName["DarkGreen"]);
        public static SolidColorBrush DarkKhaki => GetNewBrushForUi(BrushesByName["DarkKhaki"]);
        public static SolidColorBrush DarkMagenta => GetNewBrushForUi(BrushesByName["DarkMagenta"]);
        public static SolidColorBrush DarkOliveGreen => GetNewBrushForUi(BrushesByName["DarkOliveGreen"]);
        public static SolidColorBrush DarkOrange => GetNewBrushForUi(BrushesByName["DarkOrange"]);
        public static SolidColorBrush DarkOrchid => GetNewBrushForUi(BrushesByName["DarkOrchid"]);
        public static SolidColorBrush DarkRed => GetNewBrushForUi(BrushesByName["DarkRed"]);
        public static SolidColorBrush DarkSalmon => GetNewBrushForUi(BrushesByName["DarkSalmon"]);
        public static SolidColorBrush DarkSeaGreen => GetNewBrushForUi(BrushesByName["DarkSeaGreen"]);
        public static SolidColorBrush DarkSlateBlue => GetNewBrushForUi(BrushesByName["DarkSlateBlue"]);
        public static SolidColorBrush DarkSlateGray => GetNewBrushForUi(BrushesByName["DarkSlateGray"]);
        public static SolidColorBrush DarkTurquoise => GetNewBrushForUi(BrushesByName["DarkTurquoise"]);
        public static SolidColorBrush DarkViolet => GetNewBrushForUi(BrushesByName["DarkViolet"]);
        public static SolidColorBrush DeepPink => GetNewBrushForUi(BrushesByName["DeepPink"]);
        public static SolidColorBrush DeepSkyBlue => GetNewBrushForUi(BrushesByName["DeepSkyBlue"]);
        public static SolidColorBrush DimGray => GetNewBrushForUi(BrushesByName["DimGray"]);
        public static SolidColorBrush DodgerBlue => GetNewBrushForUi(BrushesByName["DodgerBlue"]);
        public static SolidColorBrush Firebrick => GetNewBrushForUi(BrushesByName["Firebrick"]);
        public static SolidColorBrush FloralWhite => GetNewBrushForUi(BrushesByName["FloralWhite"]);
        public static SolidColorBrush ForestGreen => GetNewBrushForUi(BrushesByName["ForestGreen"]);
        public static SolidColorBrush Fuchsia => GetNewBrushForUi(BrushesByName["Fuchsia"]);
        public static SolidColorBrush Gainsboro => GetNewBrushForUi(BrushesByName["Gainsboro"]);
        public static SolidColorBrush GhostWhite => GetNewBrushForUi(BrushesByName["GhostWhite"]);
        public static SolidColorBrush Gold => GetNewBrushForUi(BrushesByName["Gold"]);
        public static SolidColorBrush Goldenrod => GetNewBrushForUi(BrushesByName["Goldenrod"]);
        public static SolidColorBrush Gray => GetNewBrushForUi(BrushesByName["Gray"]);
        public static SolidColorBrush Green => GetNewBrushForUi(BrushesByName["Green"]);
        public static SolidColorBrush GreenYellow => GetNewBrushForUi(BrushesByName["GreenYellow"]);
        public static SolidColorBrush Honeydew => GetNewBrushForUi(BrushesByName["Honeydew"]);
        public static SolidColorBrush HotPink => GetNewBrushForUi(BrushesByName["HotPink"]);
        public static SolidColorBrush IndianRed => GetNewBrushForUi(BrushesByName["IndianRed"]);
        public static SolidColorBrush Indigo => GetNewBrushForUi(BrushesByName["Indigo"]);
        public static SolidColorBrush Ivory => GetNewBrushForUi(BrushesByName["Ivory"]);
        public static SolidColorBrush Khaki => GetNewBrushForUi(BrushesByName["Khaki"]);
        public static SolidColorBrush Lavender => GetNewBrushForUi(BrushesByName["Lavender"]);
        public static SolidColorBrush LavenderBlush => GetNewBrushForUi(BrushesByName["LavenderBlush"]);
        public static SolidColorBrush LawnGreen => GetNewBrushForUi(BrushesByName["LawnGreen"]);
        public static SolidColorBrush LemonChiffon => GetNewBrushForUi(BrushesByName["LemonChiffon"]);
        public static SolidColorBrush LightBlue => GetNewBrushForUi(BrushesByName["LightBlue"]);
        public static SolidColorBrush LightCoral => GetNewBrushForUi(BrushesByName["LightCoral"]);
        public static SolidColorBrush LightCyan => GetNewBrushForUi(BrushesByName["LightCyan"]);
        public static SolidColorBrush LightGoldenrodYellow => GetNewBrushForUi(BrushesByName["LightGoldenrodYellow"]);
        public static SolidColorBrush LightGray => GetNewBrushForUi(BrushesByName["LightGray"]);
        public static SolidColorBrush LightGreen => GetNewBrushForUi(BrushesByName["LightGreen"]);
        public static SolidColorBrush LightPink => GetNewBrushForUi(BrushesByName["LightPink"]);
        public static SolidColorBrush LightSalmon => GetNewBrushForUi(BrushesByName["LightSalmon"]);
        public static SolidColorBrush LightSeaGreen => GetNewBrushForUi(BrushesByName["LightSeaGreen"]);
        public static SolidColorBrush LightSkyBlue => GetNewBrushForUi(BrushesByName["LightSkyBlue"]);
        public static SolidColorBrush LightSlateGray => GetNewBrushForUi(BrushesByName["LightSlateGray"]);
        public static SolidColorBrush LightSteelBlue => GetNewBrushForUi(BrushesByName["LightSteelBlue"]);
        public static SolidColorBrush LightYellow => GetNewBrushForUi(BrushesByName["LightYellow"]);
        public static SolidColorBrush Lime => GetNewBrushForUi(BrushesByName["Lime"]);
        public static SolidColorBrush LimeGreen => GetNewBrushForUi(BrushesByName["LimeGreen"]);
        public static SolidColorBrush Linen => GetNewBrushForUi(BrushesByName["Linen"]);
        public static SolidColorBrush Magenta => GetNewBrushForUi(BrushesByName["Magenta"]);
        public static SolidColorBrush Maroon => GetNewBrushForUi(BrushesByName["Maroon"]);
        public static SolidColorBrush MediumAquamarine => GetNewBrushForUi(BrushesByName["MediumAquamarine"]);
        public static SolidColorBrush MediumBlue => GetNewBrushForUi(BrushesByName["MediumBlue"]);
        public static SolidColorBrush MediumOrchid => GetNewBrushForUi(BrushesByName["MediumOrchid"]);
        public static SolidColorBrush MediumPurple => GetNewBrushForUi(BrushesByName["MediumPurple"]);
        public static SolidColorBrush MediumSeaGreen => GetNewBrushForUi(BrushesByName["MediumSeaGreen"]);
        public static SolidColorBrush MediumSlateBlue => GetNewBrushForUi(BrushesByName["MediumSlateBlue"]);
        public static SolidColorBrush MediumSpringGreen => GetNewBrushForUi(BrushesByName["MediumSpringGreen"]);
        public static SolidColorBrush MediumTurquoise => GetNewBrushForUi(BrushesByName["MediumTurquoise"]);
        public static SolidColorBrush MediumVioletRed => GetNewBrushForUi(BrushesByName["MediumVioletRed"]);
        public static SolidColorBrush MidnightBlue => GetNewBrushForUi(BrushesByName["MidnightBlue"]);
        public static SolidColorBrush MintCream => GetNewBrushForUi(BrushesByName["MintCream"]);
        public static SolidColorBrush MistyRose => GetNewBrushForUi(BrushesByName["MistyRose"]);
        public static SolidColorBrush Moccasin => GetNewBrushForUi(BrushesByName["Moccasin"]);
        public static SolidColorBrush NavajoWhite => GetNewBrushForUi(BrushesByName["NavajoWhite"]);
        public static SolidColorBrush Navy => GetNewBrushForUi(BrushesByName["Navy"]);
        public static SolidColorBrush OldLace => GetNewBrushForUi(BrushesByName["OldLace"]);
        public static SolidColorBrush Olive => GetNewBrushForUi(BrushesByName["Olive"]);
        public static SolidColorBrush OliveDrab => GetNewBrushForUi(BrushesByName["OliveDrab"]);
        public static SolidColorBrush Orange => GetNewBrushForUi(BrushesByName["Orange"]);
        public static SolidColorBrush OrangeRed => GetNewBrushForUi(BrushesByName["OrangeRed"]);
        public static SolidColorBrush Orchid => GetNewBrushForUi(BrushesByName["Orchid"]);
        public static SolidColorBrush PaleGoldenrod => GetNewBrushForUi(BrushesByName["PaleGoldenrod"]);
        public static SolidColorBrush PaleGreen => GetNewBrushForUi(BrushesByName["PaleGreen"]);
        public static SolidColorBrush PaleTurquoise => GetNewBrushForUi(BrushesByName["PaleTurquoise"]);
        public static SolidColorBrush PaleVioletRed => GetNewBrushForUi(BrushesByName["PaleVioletRed"]);
        public static SolidColorBrush PapayaWhip => GetNewBrushForUi(BrushesByName["PapayaWhip"]);
        public static SolidColorBrush PeachPuff => GetNewBrushForUi(BrushesByName["PeachPuff"]);
        public static SolidColorBrush Peru => GetNewBrushForUi(BrushesByName["Peru"]);
        public static SolidColorBrush Pink => GetNewBrushForUi(BrushesByName["Pink"]);
        public static SolidColorBrush Plum => GetNewBrushForUi(BrushesByName["Plum"]);
        public static SolidColorBrush PowderBlue => GetNewBrushForUi(BrushesByName["PowderBlue"]);
        public static SolidColorBrush Purple => GetNewBrushForUi(BrushesByName["Purple"]);
        public static SolidColorBrush Red => GetNewBrushForUi(BrushesByName["Red"]);
        public static SolidColorBrush RosyBrown => GetNewBrushForUi(BrushesByName["RosyBrown"]);
        public static SolidColorBrush RoyalBlue => GetNewBrushForUi(BrushesByName["RoyalBlue"]);
        public static SolidColorBrush SaddleBrown => GetNewBrushForUi(BrushesByName["SaddleBrown"]);
        public static SolidColorBrush Salmon => GetNewBrushForUi(BrushesByName["Salmon"]);
        public static SolidColorBrush SandyBrown => GetNewBrushForUi(BrushesByName["SandyBrown"]);
        public static SolidColorBrush SeaGreen => GetNewBrushForUi(BrushesByName["SeaGreen"]);
        public static SolidColorBrush SeaShell => GetNewBrushForUi(BrushesByName["SeaShell"]);
        public static SolidColorBrush Sienna => GetNewBrushForUi(BrushesByName["Sienna"]);
        public static SolidColorBrush Silver => GetNewBrushForUi(BrushesByName["Silver"]);
        public static SolidColorBrush SkyBlue => GetNewBrushForUi(BrushesByName["SkyBlue"]);
        public static SolidColorBrush SlateBlue => GetNewBrushForUi(BrushesByName["SlateBlue"]);
        public static SolidColorBrush SlateGray => GetNewBrushForUi(BrushesByName["SlateGray"]);
        public static SolidColorBrush Snow => GetNewBrushForUi(BrushesByName["Snow"]);
        public static SolidColorBrush SpringGreen => GetNewBrushForUi(BrushesByName["SpringGreen"]);
        public static SolidColorBrush SteelBlue => GetNewBrushForUi(BrushesByName["SteelBlue"]);
        public static SolidColorBrush Tan => GetNewBrushForUi(BrushesByName["Tan"]);
        public static SolidColorBrush Teal => GetNewBrushForUi(BrushesByName["Teal"]);
        public static SolidColorBrush Thistle => GetNewBrushForUi(BrushesByName["Thistle"]);
        public static SolidColorBrush Tomato => GetNewBrushForUi(BrushesByName["Tomato"]);
        public static SolidColorBrush Transparent => GetNewBrushForUi(BrushesByName["Transparent"]);
        public static SolidColorBrush Turquoise => GetNewBrushForUi(BrushesByName["Turquoise"]);
        public static SolidColorBrush Violet => GetNewBrushForUi(BrushesByName["Violet"]);
        public static SolidColorBrush Wheat => GetNewBrushForUi(BrushesByName["Wheat"]);
        public static SolidColorBrush White => GetNewBrushForUi(BrushesByName["White"]);
        public static SolidColorBrush WhiteSmoke => GetNewBrushForUi(BrushesByName["WhiteSmoke"]);
        public static SolidColorBrush Yellow => GetNewBrushForUi(BrushesByName["Yellow"]);
        public static SolidColorBrush YellowGreen => GetNewBrushForUi(BrushesByName["YellowGreen"]);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
