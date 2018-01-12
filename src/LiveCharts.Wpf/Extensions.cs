using System;
using System.Windows.Controls;
using System.Windows.Media;
using Color = LiveCharts.Core.Drawing.Color;

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
        public static Color AsLiveChartsColors(this System.Windows.Media.Color color)
        {
            return new Color(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// converts a color to a solid color brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static SolidColorBrush AsSolidColorBrush(this Color color)
        {
            return new SolidColorBrush(
                System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        /// <summary>
        /// Converts Orientation to WPF.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">orientation - null</exception>
        public static Orientation AsWpfOrientation(this Core.Abstractions.Orientation orientation)
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
