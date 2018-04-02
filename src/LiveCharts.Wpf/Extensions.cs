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
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Font = LiveCharts.Core.Abstractions.Font;
using FontFamily = System.Windows.Media.FontFamily;
using FontStyle = System.Windows.FontStyle;
using FontWeight = LiveCharts.Core.Abstractions.FontWeight;
using Orientation = System.Windows.Controls.Orientation;
using Point = System.Windows.Point;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Windows presentation foundation Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts a WPF color to LiveCharts color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Color AsLiveCharts(this Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts to a type face.
        /// </summary>
        /// <param name="font">The style.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">style - null</exception>
        public static Typeface AsTypeface(this Font font)
        {
            FontStyle s;

            switch (font.Style)
            {
                case Core.Abstractions.FontStyle.Regular:
                    s = System.Windows.FontStyles.Normal;
                    break;
                case Core.Abstractions.FontStyle.Italic:
                    s = System.Windows.FontStyles.Italic;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            System.Windows.FontWeight w;

            switch (font.Weight)
            {
                case FontWeight.Regular:
                    w = FontWeights.Regular;
                    break;
                case FontWeight.Bold:
                    w = FontWeights.Bold;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Typeface(
                new FontFamily(font.FamilyName), s, w, FontStretches.Normal);
        }

        /// <summary>
        /// converts a color to a solid color brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static SolidColorBrush AsWpf(this Color color)
        {
            if (color == Color.Empty) return null;
            return new SolidColorBrush(
                System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        /// <summary>
        /// converts a LiveCharts point to a WPF point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static Point AsWpf(this PointF point)
        {
            return new Point(point.X, point.Y);
        }

        /// <summary>
        /// Converts Orientation to WPF.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">orientation - null</exception>
        public static Orientation AsWpf(this Core.Abstractions.Orientation orientation)
        {
            switch (orientation)
            {
                case Core.Abstractions.Orientation.Vertical:
                    return Orientation.Vertical;
                case Core.Abstractions.Orientation.Horizontal:
                    return Orientation.Horizontal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }
    }
}
