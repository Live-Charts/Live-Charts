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
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Dimensions;
using Font = LiveCharts.Core.Abstractions.Font;

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
            var baseFont = new Font("Arial", 11, FontStyles.Regular, FontWeight.Regular);

            var sepStyle = new SeparatorStyle(Color.FromArgb(255, 230, 230, 230), Color.FromArgb(50, 245, 245, 245), 1);
            var altStyle =
                new SeparatorStyle(Color.FromArgb(255, 220, 220, 220), Color.FromArgb(50, 220, 220, 220), 1);

            charting
                .UseMaterialDesignColors()
                .UseMaterialDesignColors()
                .SetDefault<ISeries>(series =>
                {
                    series.StrokeThickness = 2;
                    series.DefaultFillOpacity = .3f;
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