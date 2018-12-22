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

namespace LiveCharts.Core.Drawing.Brushes
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
                            return new
                            {
                                name = item[0],
                                r = byte.Parse(item[1]),
                                g = byte.Parse(item[2]),
                                b = byte.Parse(item[3])
                            };
                        }).ToDictionary(
                            x => x.name,
                            x => new[] { x.r, x.g, x.b });
                }

                return _b;
            }
        }

        private static ISolidColorBrush GetNewBrushForUi(byte[] components)
        {
            return Charting.Settings.UiProvider.GetNewSolidColorBrush(255, components[0], components[1], components[2]);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static ISolidColorBrush AliceBlue => GetNewBrushForUi(BrushesByName["AliceBlue"]);
        public static ISolidColorBrush AntiqueWhite => GetNewBrushForUi(BrushesByName["AntiqueWhite"]);
        public static ISolidColorBrush Aqua => GetNewBrushForUi(BrushesByName["Aqua"]);
        public static ISolidColorBrush Aquamarine => GetNewBrushForUi(BrushesByName["Aquamarine"]);
        public static ISolidColorBrush Azure => GetNewBrushForUi(BrushesByName["Azure"]);
        public static ISolidColorBrush Beige => GetNewBrushForUi(BrushesByName["Beige"]);
        public static ISolidColorBrush Bisque => GetNewBrushForUi(BrushesByName["Bisque"]);
        public static ISolidColorBrush Black => GetNewBrushForUi(BrushesByName["Black"]);
        public static ISolidColorBrush BlanchedAlmond => GetNewBrushForUi(BrushesByName["BlanchedAlmond"]);
        public static ISolidColorBrush Blue => GetNewBrushForUi(BrushesByName["Blue"]);
        public static ISolidColorBrush BlueViolet => GetNewBrushForUi(BrushesByName["BlueViolet"]);
        public static ISolidColorBrush Brown => GetNewBrushForUi(BrushesByName["Brown"]);
        public static ISolidColorBrush BurlyWood => GetNewBrushForUi(BrushesByName["BurlyWood"]);
        public static ISolidColorBrush CadetBlue => GetNewBrushForUi(BrushesByName["CadetBlue"]);
        public static ISolidColorBrush Chartreuse => GetNewBrushForUi(BrushesByName["Chartreuse"]);
        public static ISolidColorBrush Chocolate => GetNewBrushForUi(BrushesByName["Chocolate"]);
        public static ISolidColorBrush Coral => GetNewBrushForUi(BrushesByName["Coral"]);
        public static ISolidColorBrush CornflowerBlue => GetNewBrushForUi(BrushesByName["CornflowerBlue"]);
        public static ISolidColorBrush Cornsilk => GetNewBrushForUi(BrushesByName["Cornsilk"]);
        public static ISolidColorBrush Crimson => GetNewBrushForUi(BrushesByName["Crimson"]);
        public static ISolidColorBrush Cyan => GetNewBrushForUi(BrushesByName["Cyan"]);
        public static ISolidColorBrush DarkBlue => GetNewBrushForUi(BrushesByName["DarkBlue"]);
        public static ISolidColorBrush DarkCyan => GetNewBrushForUi(BrushesByName["DarkCyan"]);
        public static ISolidColorBrush DarkGoldenrod => GetNewBrushForUi(BrushesByName["DarkGoldenrod"]);
        public static ISolidColorBrush DarkGray => GetNewBrushForUi(BrushesByName["DarkGray"]);
        public static ISolidColorBrush DarkGreen => GetNewBrushForUi(BrushesByName["DarkGreen"]);
        public static ISolidColorBrush DarkKhaki => GetNewBrushForUi(BrushesByName["DarkKhaki"]);
        public static ISolidColorBrush DarkMagenta => GetNewBrushForUi(BrushesByName["DarkMagenta"]);
        public static ISolidColorBrush DarkOliveGreen => GetNewBrushForUi(BrushesByName["DarkOliveGreen"]);
        public static ISolidColorBrush DarkOrange => GetNewBrushForUi(BrushesByName["DarkOrange"]);
        public static ISolidColorBrush DarkOrchid => GetNewBrushForUi(BrushesByName["DarkOrchid"]);
        public static ISolidColorBrush DarkRed => GetNewBrushForUi(BrushesByName["DarkRed"]);
        public static ISolidColorBrush DarkSalmon => GetNewBrushForUi(BrushesByName["DarkSalmon"]);
        public static ISolidColorBrush DarkSeaGreen => GetNewBrushForUi(BrushesByName["DarkSeaGreen"]);
        public static ISolidColorBrush DarkSlateBlue => GetNewBrushForUi(BrushesByName["DarkSlateBlue"]);
        public static ISolidColorBrush DarkSlateGray => GetNewBrushForUi(BrushesByName["DarkSlateGray"]);
        public static ISolidColorBrush DarkTurquoise => GetNewBrushForUi(BrushesByName["DarkTurquoise"]);
        public static ISolidColorBrush DarkViolet => GetNewBrushForUi(BrushesByName["DarkViolet"]);
        public static ISolidColorBrush DeepPink => GetNewBrushForUi(BrushesByName["DeepPink"]);
        public static ISolidColorBrush DeepSkyBlue => GetNewBrushForUi(BrushesByName["DeepSkyBlue"]);
        public static ISolidColorBrush DimGray => GetNewBrushForUi(BrushesByName["DimGray"]);
        public static ISolidColorBrush DodgerBlue => GetNewBrushForUi(BrushesByName["DodgerBlue"]);
        public static ISolidColorBrush Firebrick => GetNewBrushForUi(BrushesByName["Firebrick"]);
        public static ISolidColorBrush FloralWhite => GetNewBrushForUi(BrushesByName["FloralWhite"]);
        public static ISolidColorBrush ForestGreen => GetNewBrushForUi(BrushesByName["ForestGreen"]);
        public static ISolidColorBrush Fuchsia => GetNewBrushForUi(BrushesByName["Fuchsia"]);
        public static ISolidColorBrush Gainsboro => GetNewBrushForUi(BrushesByName["Gainsboro"]);
        public static ISolidColorBrush GhostWhite => GetNewBrushForUi(BrushesByName["GhostWhite"]);
        public static ISolidColorBrush Gold => GetNewBrushForUi(BrushesByName["Gold"]);
        public static ISolidColorBrush Goldenrod => GetNewBrushForUi(BrushesByName["Goldenrod"]);
        public static ISolidColorBrush Gray => GetNewBrushForUi(BrushesByName["Gray"]);
        public static ISolidColorBrush Green => GetNewBrushForUi(BrushesByName["Green"]);
        public static ISolidColorBrush GreenYellow => GetNewBrushForUi(BrushesByName["GreenYellow"]);
        public static ISolidColorBrush Honeydew => GetNewBrushForUi(BrushesByName["Honeydew"]);
        public static ISolidColorBrush HotPink => GetNewBrushForUi(BrushesByName["HotPink"]);
        public static ISolidColorBrush IndianRed => GetNewBrushForUi(BrushesByName["IndianRed"]);
        public static ISolidColorBrush Indigo => GetNewBrushForUi(BrushesByName["Indigo"]);
        public static ISolidColorBrush Ivory => GetNewBrushForUi(BrushesByName["Ivory"]);
        public static ISolidColorBrush Khaki => GetNewBrushForUi(BrushesByName["Khaki"]);
        public static ISolidColorBrush Lavender => GetNewBrushForUi(BrushesByName["Lavender"]);
        public static ISolidColorBrush LavenderBlush => GetNewBrushForUi(BrushesByName["LavenderBlush"]);
        public static ISolidColorBrush LawnGreen => GetNewBrushForUi(BrushesByName["LawnGreen"]);
        public static ISolidColorBrush LemonChiffon => GetNewBrushForUi(BrushesByName["LemonChiffon"]);
        public static ISolidColorBrush LightBlue => GetNewBrushForUi(BrushesByName["LightBlue"]);
        public static ISolidColorBrush LightCoral => GetNewBrushForUi(BrushesByName["LightCoral"]);
        public static ISolidColorBrush LightCyan => GetNewBrushForUi(BrushesByName["LightCyan"]);
        public static ISolidColorBrush LightGoldenrodYellow => GetNewBrushForUi(BrushesByName["LightGoldenrodYellow"]);
        public static ISolidColorBrush LightGray => GetNewBrushForUi(BrushesByName["LightGray"]);
        public static ISolidColorBrush LightGreen => GetNewBrushForUi(BrushesByName["LightGreen"]);
        public static ISolidColorBrush LightPink => GetNewBrushForUi(BrushesByName["LightPink"]);
        public static ISolidColorBrush LightSalmon => GetNewBrushForUi(BrushesByName["LightSalmon"]);
        public static ISolidColorBrush LightSeaGreen => GetNewBrushForUi(BrushesByName["LightSeaGreen"]);
        public static ISolidColorBrush LightSkyBlue => GetNewBrushForUi(BrushesByName["LightSkyBlue"]);
        public static ISolidColorBrush LightSlateGray => GetNewBrushForUi(BrushesByName["LightSlateGray"]);
        public static ISolidColorBrush LightSteelBlue => GetNewBrushForUi(BrushesByName["LightSteelBlue"]);
        public static ISolidColorBrush LightYellow => GetNewBrushForUi(BrushesByName["LightYellow"]);
        public static ISolidColorBrush Lime => GetNewBrushForUi(BrushesByName["Lime"]);
        public static ISolidColorBrush LimeGreen => GetNewBrushForUi(BrushesByName["LimeGreen"]);
        public static ISolidColorBrush Linen => GetNewBrushForUi(BrushesByName["Linen"]);
        public static ISolidColorBrush Magenta => GetNewBrushForUi(BrushesByName["Magenta"]);
        public static ISolidColorBrush Maroon => GetNewBrushForUi(BrushesByName["Maroon"]);
        public static ISolidColorBrush MediumAquamarine => GetNewBrushForUi(BrushesByName["MediumAquamarine"]);
        public static ISolidColorBrush MediumBlue => GetNewBrushForUi(BrushesByName["MediumBlue"]);
        public static ISolidColorBrush MediumOrchid => GetNewBrushForUi(BrushesByName["MediumOrchid"]);
        public static ISolidColorBrush MediumPurple => GetNewBrushForUi(BrushesByName["MediumPurple"]);
        public static ISolidColorBrush MediumSeaGreen => GetNewBrushForUi(BrushesByName["MediumSeaGreen"]);
        public static ISolidColorBrush MediumSlateBlue => GetNewBrushForUi(BrushesByName["MediumSlateBlue"]);
        public static ISolidColorBrush MediumSpringGreen => GetNewBrushForUi(BrushesByName["MediumSpringGreen"]);
        public static ISolidColorBrush MediumTurquoise => GetNewBrushForUi(BrushesByName["MediumTurquoise"]);
        public static ISolidColorBrush MediumVioletRed => GetNewBrushForUi(BrushesByName["MediumVioletRed"]);
        public static ISolidColorBrush MidnightBlue => GetNewBrushForUi(BrushesByName["MidnightBlue"]);
        public static ISolidColorBrush MintCream => GetNewBrushForUi(BrushesByName["MintCream"]);
        public static ISolidColorBrush MistyRose => GetNewBrushForUi(BrushesByName["MistyRose"]);
        public static ISolidColorBrush Moccasin => GetNewBrushForUi(BrushesByName["Moccasin"]);
        public static ISolidColorBrush NavajoWhite => GetNewBrushForUi(BrushesByName["NavajoWhite"]);
        public static ISolidColorBrush Navy => GetNewBrushForUi(BrushesByName["Navy"]);
        public static ISolidColorBrush OldLace => GetNewBrushForUi(BrushesByName["OldLace"]);
        public static ISolidColorBrush Olive => GetNewBrushForUi(BrushesByName["Olive"]);
        public static ISolidColorBrush OliveDrab => GetNewBrushForUi(BrushesByName["OliveDrab"]);
        public static ISolidColorBrush Orange => GetNewBrushForUi(BrushesByName["Orange"]);
        public static ISolidColorBrush OrangeRed => GetNewBrushForUi(BrushesByName["OrangeRed"]);
        public static ISolidColorBrush Orchid => GetNewBrushForUi(BrushesByName["Orchid"]);
        public static ISolidColorBrush PaleGoldenrod => GetNewBrushForUi(BrushesByName["PaleGoldenrod"]);
        public static ISolidColorBrush PaleGreen => GetNewBrushForUi(BrushesByName["PaleGreen"]);
        public static ISolidColorBrush PaleTurquoise => GetNewBrushForUi(BrushesByName["PaleTurquoise"]);
        public static ISolidColorBrush PaleVioletRed => GetNewBrushForUi(BrushesByName["PaleVioletRed"]);
        public static ISolidColorBrush PapayaWhip => GetNewBrushForUi(BrushesByName["PapayaWhip"]);
        public static ISolidColorBrush PeachPuff => GetNewBrushForUi(BrushesByName["PeachPuff"]);
        public static ISolidColorBrush Peru => GetNewBrushForUi(BrushesByName["Peru"]);
        public static ISolidColorBrush Pink => GetNewBrushForUi(BrushesByName["Pink"]);
        public static ISolidColorBrush Plum => GetNewBrushForUi(BrushesByName["Plum"]);
        public static ISolidColorBrush PowderBlue => GetNewBrushForUi(BrushesByName["PowderBlue"]);
        public static ISolidColorBrush Purple => GetNewBrushForUi(BrushesByName["Purple"]);
        public static ISolidColorBrush Red => GetNewBrushForUi(BrushesByName["Red"]);
        public static ISolidColorBrush RosyBrown => GetNewBrushForUi(BrushesByName["RosyBrown"]);
        public static ISolidColorBrush RoyalBlue => GetNewBrushForUi(BrushesByName["RoyalBlue"]);
        public static ISolidColorBrush SaddleBrown => GetNewBrushForUi(BrushesByName["SaddleBrown"]);
        public static ISolidColorBrush Salmon => GetNewBrushForUi(BrushesByName["Salmon"]);
        public static ISolidColorBrush SandyBrown => GetNewBrushForUi(BrushesByName["SandyBrown"]);
        public static ISolidColorBrush SeaGreen => GetNewBrushForUi(BrushesByName["SeaGreen"]);
        public static ISolidColorBrush SeaShell => GetNewBrushForUi(BrushesByName["SeaShell"]);
        public static ISolidColorBrush Sienna => GetNewBrushForUi(BrushesByName["Sienna"]);
        public static ISolidColorBrush Silver => GetNewBrushForUi(BrushesByName["Silver"]);
        public static ISolidColorBrush SkyBlue => GetNewBrushForUi(BrushesByName["SkyBlue"]);
        public static ISolidColorBrush SlateBlue => GetNewBrushForUi(BrushesByName["SlateBlue"]);
        public static ISolidColorBrush SlateGray => GetNewBrushForUi(BrushesByName["SlateGray"]);
        public static ISolidColorBrush Snow => GetNewBrushForUi(BrushesByName["Snow"]);
        public static ISolidColorBrush SpringGreen => GetNewBrushForUi(BrushesByName["SpringGreen"]);
        public static ISolidColorBrush SteelBlue => GetNewBrushForUi(BrushesByName["SteelBlue"]);
        public static ISolidColorBrush Tan => GetNewBrushForUi(BrushesByName["Tan"]);
        public static ISolidColorBrush Teal => GetNewBrushForUi(BrushesByName["Teal"]);
        public static ISolidColorBrush Thistle => GetNewBrushForUi(BrushesByName["Thistle"]);
        public static ISolidColorBrush Tomato => GetNewBrushForUi(BrushesByName["Tomato"]);
        public static ISolidColorBrush Transparent => GetNewBrushForUi(BrushesByName["Transparent"]);
        public static ISolidColorBrush Turquoise => GetNewBrushForUi(BrushesByName["Turquoise"]);
        public static ISolidColorBrush Violet => GetNewBrushForUi(BrushesByName["Violet"]);
        public static ISolidColorBrush Wheat => GetNewBrushForUi(BrushesByName["Wheat"]);
        public static ISolidColorBrush White => GetNewBrushForUi(BrushesByName["White"]);
        public static ISolidColorBrush WhiteSmoke => GetNewBrushForUi(BrushesByName["WhiteSmoke"]);
        public static ISolidColorBrush Yellow => GetNewBrushForUi(BrushesByName["Yellow"]);
        public static ISolidColorBrush YellowGreen => GetNewBrushForUi(BrushesByName["YellowGreen"]);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
