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

#endregion

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
        public static Settings SetTheme(this Settings charting, Themes theme)
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