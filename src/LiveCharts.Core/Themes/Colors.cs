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

#endregion

namespace LiveCharts.Themes
{
    /// <summary>
    /// Colors extensions.
    /// </summary>
    internal static class Colors
    {
        /// <summary>
        /// Uses a custom colors array as the default series color set.
        /// </summary>
        /// <returns></returns>
        public static Settings HasColors(this Settings settings, IEnumerable<Color> colors)
        {
            settings.Colors.Clear();
            settings.Colors.AddRange(colors);
            return settings;
        }

        /// <summary>
        /// Uses Microsoft's metro colors.
        /// </summary>
        /// <returns></returns>
        public static Settings HasMetroColors(this Settings settings)
        {
            settings.HasColors(new[]
            {
                // Microsoft's Metro UI colors
                Color.FromArgb(255, 43, 86, 145), // dark blue
                Color.FromArgb(255, 191, 39, 72), // red
                Color.FromArgb(255, 146, 179, 72), // green
                Color.FromArgb(255, 34, 132, 232), // blue
                Color.FromArgb(255, 254, 21, 145), // metro magenta
                Color.FromArgb(255, 254, 196, 60), // yellow
                Color.FromArgb(255, 102, 58, 178), // dark purple
                Color.FromArgb(255, 225, 87, 53), // dark orange
                Color.FromArgb(255, 0, 168, 166)  // teal
            });

            return settings;
        }

        /// <summary>
        /// Uses a white scale colors.
        /// </summary>
        /// <returns></returns>
        public static Settings HasWhiteScaleColors(this Settings settings)
        {
            settings.HasColors(new[]
            {
                Color.FromArgb(255, 245, 245, 245),
                Color.FromArgb(255, 215, 215, 215),
                Color.FromArgb(255, 190, 190, 190),
                Color.FromArgb(255, 165, 165, 165),
                Color.FromArgb(255, 140, 140, 140)
            });

            return settings;
        }

        /// <summary>
        /// Uses a black scale colors.
        /// </summary>
        /// <returns></returns>
        public static Settings HasGrayScaleColors(this Settings settings)
        {
            settings.HasColors(new[]
            {
                Color.FromArgb(255, 30, 30, 30),
                Color.FromArgb(255, 65, 65, 65),
                Color.FromArgb(255, 90, 90, 90),
                Color.FromArgb(255, 115, 115, 115),
                Color.FromArgb(255, 140, 140, 140)
            });

            return settings;
        }

        /// <summary>
        /// Uses a blue scale colors.
        /// </summary>
        /// <returns></returns>
        public static Settings HasBlueScaleColors(this Settings settings)
        {
            settings.HasColors(new[]
            {
                // Google's material design
                // https://material.io/guidelines/style/color.html#color-color-palette
                // blue

                Color.FromArgb(255, 16, 70, 154), // blue 900
                Color.FromArgb(255, 0, 115, 203), // blue 700
                Color.FromArgb(255, 0, 145, 237), // blue 500
                Color.FromArgb(255, 89, 177, 241), // blue 300
            });

            return settings;
        }
    }
}