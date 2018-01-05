using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.Data.Builders;

namespace LiveCharts.Core.Config
{
    /// <summary>
    /// Colors extensions.
    /// </summary>
    public static class ColorsConfig
    {
        /// <summary>
        /// Uses a custom colors array as the default series color set.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static LiveChartsConfig UseColors(this LiveChartsConfig config, IEnumerable<Color> colors)
        {
            config.Colors.Clear();
            config.Colors.AddRange(colors);
            return config;
        }

        /// <summary>
        /// Uses Google's material design colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig UseMaterialDesignColors(this LiveChartsConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                // Goolge's material design
                // https://material.io/guidelines/style/color.html#color-color-palette

                Color.FromArgb(255,8,98,185),       // blue         800
                Color.FromArgb(255,219,55,52),      // red          700
                Color.FromArgb(255,254,168,62),     // yellow       800
                Color.FromArgb(255,82,109,120),     // blue grey    600
                Color.FromArgb(255,141,24,78),      // pink         900
                Color.FromArgb(255,41,158,82),      // green        600
                Color.FromArgb(255,120,130,197),    // indigo       300
                Color.FromArgb(255,0,147,135),      // teal         500
                Color.FromArgb(255,215,230,127),    // lime         300
                Color.FromArgb(255,254,140,43),     // orange       600
            });

            return config;
        }

        /// <summary>
        /// Uses Microsoft's metro colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig UseMetroColors(this LiveChartsConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                // Microsoft's Metro UI colors
                Color.FromArgb(255,43,86,145),      // dark blue
                Color.FromArgb(255,191,39,72),      // red
                Color.FromArgb(255,146,179,72),     // green
                Color.FromArgb(255,34,132,232),     // blue
                Color.FromArgb(255,254,21,145),     // metro magenta
                Color.FromArgb(255,254,196,60),     // yellow
                Color.FromArgb(255,102,58,178),     // dark purple
                Color.FromArgb(255,225,87,53),      // dark orange
                Color.FromArgb(255,0,168,166),      // teal
            });

            return config;
        }

        /// <summary>
        /// Uses a white scale colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig UseWhiteScaleColors(this LiveChartsConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                Color.FromArgb(245, 245, 245),
                Color.FromArgb(215, 215, 215),
                Color.FromArgb(190, 190, 190),
                Color.FromArgb(165, 165, 165),
                Color.FromArgb(140, 140, 140)
            });

            return config;
        }

        /// <summary>
        /// Uses a black scale colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig UseBlackScaleColors(this LiveChartsConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                Color.FromArgb(30, 30, 30),
                Color.FromArgb(65, 65, 65),
                Color.FromArgb(90, 90, 90),
                Color.FromArgb(115, 115, 115),
                Color.FromArgb(140, 140, 140)
            });

            return config;
        }

        /// <summary>
        /// Uses a blue scale colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig UseBlueScaleColors(this LiveChartsConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                // Goolge's material design
                // https://material.io/guidelines/style/color.html#color-color-palette
                // blue

                Color.FromArgb(16, 70, 154),        // blue 900
                Color.FromArgb(0, 115, 203),        // blue 700
                Color.FromArgb(0, 145, 237),         // blue 500
                Color.FromArgb(89, 177, 241),      // blue 300
            });

            return config;
        }
    }
}
