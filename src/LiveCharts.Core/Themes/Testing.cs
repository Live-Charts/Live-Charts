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

using System.Drawing;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Styles;
using Font = LiveCharts.Core.Interaction.Styles.Font;

#endregion

namespace LiveCharts.Core.Themes
{
    /// <summary>
    /// Testing theme.
    /// </summary>
    public static class Testing
    {
        /// <summary>
        /// Uses the testing theme.
        /// </summary>
        /// <param name="charting">The charting.</param>
        /// <returns></returns>
        public static Charting UsingTestingTheme(this Charting charting)
        {
            var sepStyle = new SeparatorStyle(
                new SolidColorBrush(Color.FromArgb(0, 230, 230, 230)),
                new SolidColorBrush(Color.FromArgb(150, 245, 245, 245)),
                1);
            var altStyle = new SeparatorStyle(
                new SolidColorBrush(Color.FromArgb(0, 220, 220, 220)),
                new SolidColorBrush(Color.FromArgb(150, 220, 220, 220)),
                1);

            charting
                .HasColors(new []
                {
                    // Google's material design
                    // https://material.io/guidelines/style/color.html#color-color-palette

                    Color.FromArgb(255, 8, 98, 185), // blue         800
                    Color.FromArgb(255, 219, 55, 52), // red          700
                    Color.FromArgb(255, 254, 168, 62), // yellow       800
                    Color.FromArgb(255, 82, 109, 120), // blue grey    600
                    Color.FromArgb(255, 141, 24, 78), // pink         900
                    Color.FromArgb(255, 41, 158, 82), // green        600
                    Color.FromArgb(255, 120, 130, 197), // indigo       300
                    Color.FromArgb(255, 0, 147, 135), // teal         500
                    Color.FromArgb(255, 215, 230, 127), // lime         300
                    Color.FromArgb(255, 254, 140, 43), // orange       600)
                })
                .SetDefault<IStrokeSeries>(series =>
                {
                    series.StrokeThickness = 3f;
                    series.DefaultFillOpacity = .3f;
                })
                .SetDefault<IBubbleSeries>(scatter =>
                {
                    scatter.MinGeometrySize = 35;
                    scatter.MaxGeometrySize = 40;
                })
                .SetDefault<Axis>(axis =>
                {
                    axis.XSeparatorStyle = sepStyle;
                    axis.YSeparatorStyle = sepStyle;
                    axis.XAlternativeSeparatorStyle = altStyle;
                    axis.YAlternativeSeparatorStyle = altStyle;
                });

            return charting;
        }
    }
}