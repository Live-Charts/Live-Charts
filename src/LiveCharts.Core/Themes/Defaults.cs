using System;

namespace LiveCharts.Core.Themes
{
    /// <summary>
    /// The default themes.
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// Sets a default theme defined by LiveCharts, you can always define your own theme take a look at 'docs/getting started/settings' for more information.
        /// </summary>
        /// <param name="charting">The charting.</param>
        /// <param name="theme">The theme.</param>
        /// <returns></returns>
        public static Charting SetTheme(this Charting charting, Themes theme)
        {
            switch (theme)
            {
                case Themes.MaterialDesign:
                    charting
                        .SetMaterialDesignDefaults();
                    break;
                case Themes.Metro:
                    charting
                        .SetMaterialDesignDefaults()
                        .HasMetroColors();
                    break;
                case Themes.WhiteScale:
                    charting
                        .SetMaterialDesignDefaults()
                        .HasWhiteScaleColors();
                    break;
                case Themes.GrayScale:
                    charting
                        .SetMaterialDesignDefaults()
                        .HasGrayScaleColors();
                    break;
                case Themes.BlueScale:
                    charting
                        .SetMaterialDesignDefaults();
                    break;
                case Themes.Testing:
                    charting.UsingTestingTheme();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }

            return charting;
        }
    }
}