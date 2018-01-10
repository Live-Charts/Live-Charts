using System.Collections.Generic;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;

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
        public static ChartingConfig UseColors(this ChartingConfig config, IEnumerable<Color> colors)
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
        public static ChartingConfig UseMaterialDesignColors(this ChartingConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                // Goolge's material design
                // https://material.io/guidelines/style/color.html#color-color-palette

                new Color(255, 8, 98, 185), // blue         800
                new Color(255, 219, 55, 52), // red          700
                new Color(255, 254, 168, 62), // yellow       800
                new Color(255, 82, 109, 120), // blue grey    600
                new Color(255, 141, 24, 78), // pink         900
                new Color(255, 41, 158, 82), // green        600
                new Color(255, 120, 130, 197), // indigo       300
                new Color(255, 0, 147, 135), // teal         500
                new Color(255, 215, 230, 127), // lime         300
                new Color(255, 254, 140, 43), // orange       600
            });

            return config;
        }

        /// <summary>
        /// Uses Microsoft's metro colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static ChartingConfig UseMetroColors(this ChartingConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                // Microsoft's Metro UI colors
                new Color(255, 43, 86, 145), // dark blue
                new Color(255, 191, 39, 72), // red
                new Color(255, 146, 179, 72), // green
                new Color(255, 34, 132, 232), // blue
                new Color(255, 254, 21, 145), // metro magenta
                new Color(255, 254, 196, 60), // yellow
                new Color(255, 102, 58, 178), // dark purple
                new Color(255, 225, 87, 53), // dark orange
                new Color(255, 0, 168, 166)  // teal
            });

            return config;
        }

        /// <summary>
        /// Uses a white scale colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static ChartingConfig UseWhiteScaleColors(this ChartingConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                new Color(255, 245, 245, 245),
                new Color(255, 215, 215, 215),
                new Color(255, 190, 190, 190),
                new Color(255, 165, 165, 165),
                new Color(255, 140, 140, 140)
            });

            return config;
        }

        /// <summary>
        /// Uses a black scale colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static ChartingConfig UseBlackScaleColors(this ChartingConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                new Color(255, 30, 30, 30),
                new Color(255, 65, 65, 65),
                new Color(255, 90, 90, 90),
                new Color(255, 115, 115, 115),
                new Color(255, 140, 140, 140)
            });

            return config;
        }

        /// <summary>
        /// Uses a blue scale colors.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static ChartingConfig UseBlueScaleColors(this ChartingConfig config)
        {
            config.Colors.Clear();
            config.Colors.AddRange(new[]
            {
                // Goolge's material design
                // https://material.io/guidelines/style/color.html#color-color-palette
                // blue

                new Color(255, 16, 70, 154), // blue 900
                new Color(255, 0, 115, 203), // blue 700
                new Color(255, 0, 145, 237), // blue 500
                new Color(255, 89, 177, 241), // blue 300
            });

            return config;
        }
    }
}