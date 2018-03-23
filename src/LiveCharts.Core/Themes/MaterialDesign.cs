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
using LiveCharts.Core.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing.Svg;
using Font = LiveCharts.Core.Abstractions.Font;

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
        /// <returns></returns>
        public static Charting UsingMaterialDesignLightTheme(this Charting charting)
        {
            var baseFont = new Font("Arial", 11, FontStyles.Regular, FontWeight.Regular);

            charting
                .UseMaterialDesignColors()

                // sets a base for all the series
                .SetDefault<ISeries>(series =>
                {
                    series.IsVisible = true;
                    series.Font = baseFont;
                    series.DataLabels = false;
                    series.DefaultFillOpacity = 1f;
                    series.Fill = Color.Empty; // if the color is empty, the DataFactory will assign it.
                    series.Stroke = Color.Empty;
                    series.StrokeThickness = 0;
                    series.Geometry = Geometry.Circle;
                    series.Title = "Unnamed Series";
                    series.DataLabelsPosition = new DataLabelsPosition
                    {
                        HorizontalAlignment = HorizontalAlingment.Centered,
                        VerticalAlignment = VerticalLabelPosition.Top,
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
                .SetDefault<IScatterSeries>(scatterSeries =>
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
                    plane.Font = baseFont;
                    plane.LabelFormatter = Formatters.AsMetricNumber;
                })
                .SetDefault<Axis>(axis =>
                {
                    axis.XSeparatorStyle = SeparatorStyle.Empty;
                    axis.YSeparatorStyle = new SeparatorStyle(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 245, 245, 245), 0);
                    axis.XAlternativeSeparatorStyle = SeparatorStyle.Empty;
                    axis.YAlternativeSeparatorStyle = SeparatorStyle.Empty;
                });

            return charting;
        }
    }
}
