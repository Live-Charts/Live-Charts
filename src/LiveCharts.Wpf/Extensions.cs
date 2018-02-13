using System;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Font = LiveCharts.Core.Abstractions.Font;
using FontStyle = System.Windows.FontStyle;
using FontStyles = LiveCharts.Core.Abstractions.FontStyles;
using FontWeight = LiveCharts.Core.Abstractions.FontWeight;
using Orientation = System.Windows.Controls.Orientation;
using Point = System.Windows.Point;

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
                case FontStyles.Regular:
                    s = System.Windows.FontStyles.Normal;
                    break;
                case FontStyles.Italic:
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
        public static Point AsWpf(this Core.Drawing.Point point)
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
