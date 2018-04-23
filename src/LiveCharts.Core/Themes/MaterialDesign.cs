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
using LiveCharts.Core.Drawing.Svg;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Styles;
#if NET45 || NET46
using Font = LiveCharts.Core.Interaction.Styles.Font;
using FontStyle= LiveCharts.Core.Interaction.Styles.FontStyle;
#endif

#endregion

namespace LiveCharts.Core.Themes
{
    /// <summary>
    /// material design theme.
    /// </summary>
    public static class MaterialDesign
    {
        /// <summary>
        /// Uses the material design light theme.
        /// </summary>
        /// <param name="charting">The settings.</param>
        internal static Charting SetMaterialDesignDefaults(this Charting charting)
        {
            var baseFont = new Font("Arial", 13, FontStyle.Regular, FontWeight.Regular);

            charting
                .HasColors(new[]
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

                // sets a base for all the series
                .SetDefault<IStrokeSeries>(series =>
                {
                    series.IsVisible = true;
                    series.DataLabelsFont = baseFont;
                    series.DataLabels = false;
                    series.StrokeThickness = 3f;
                    series.DefaultFillOpacity = 1f;
                    series.Fill = null; // if the color is empty, the series will assign it.
                    series.Stroke = null;
                    series.StrokeThickness = 0;
                    series.Geometry = Geometry.Circle;
                    series.Title = "Unnamed Series";
                    series.DataLabelsPosition = new DataLabelsPosition
                    {
                        HorizontalAlignment = HorizontalAlignment.Centered,
                        VerticalAlignment = VerticalAlignment.Top,
                        Rotation = 0
                    };
                })
                .SetDefault<IBarSeries>(columnSeries =>
                {
                    columnSeries.StrokeThickness = 0f;
                    columnSeries.Geometry = Geometry.Square;
                    columnSeries.MaxColumnWidth = 20f;
                    columnSeries.DefaultFillOpacity = 1f;
                })
                .SetDefault<IBubbleSeries>(scatterSeries =>
                {
                    scatterSeries.DefaultFillOpacity = .6f;
                    scatterSeries.StrokeThickness = 3f;
                    scatterSeries.MinGeometrySize = 25f;
                    scatterSeries.MaxGeometrySize = 45f;
                })
                .SetDefault<ILineSeries>(lineSeries =>
                {
                    lineSeries.Geometry = Geometry.Circle;
                    lineSeries.GeometrySize = 15f;
                    lineSeries.StrokeThickness = 3.5f;
                    lineSeries.DefaultFillOpacity = .25f;
                    lineSeries.LineSmoothness = .6f;
                })

                // sets a base for all the planes
                .SetDefault<Plane>(plane =>
                {
                    plane.LabelsFont = baseFont;
                    plane.LabelsForeground = new SolidColorBrush(Color.FromArgb(30, 30, 30));
                    plane.LabelsRotation = 0;
                    plane.LabelFormatter = Format.AsMetricNumber;
                })
                .SetDefault<Axis>(axis =>
                {
                    axis.XSeparatorStyle = null;
                    axis.YSeparatorStyle = new ShapeStyle(
                        new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                        new SolidColorBrush(Color.FromArgb(255, 225, 225, 225)),
                        0,
                        null);
                    axis.XAlternativeSeparatorStyle = null;
                    axis.YAlternativeSeparatorStyle = null;
                });

            return charting;
        }
    }
}
