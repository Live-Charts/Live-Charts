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
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Interaction.Controls;
using Brushes = System.Windows.Media.Brushes;
using Font = LiveCharts.Core.Interaction.Styles.Font;

#endregion

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// a default label for a point.
    /// </summary>
    /// <seealso cref="TextBlock" />
    public class AxisLabel : Label, IPlaneLabelControl
    {
        static AxisLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AxisLabel),
                new FrameworkPropertyMetadata(typeof(AxisLabel)));
        }

        SizeF IPlaneLabelControl.MeasureAndUpdate(IChartContent content, Font font, string label)
        {
            Content = label;
            FontFamily = new System.Windows.Media.FontFamily(font.FamilyName);
            FontSize = font.Size;
            FontWeight = font.Weight == Core.Interaction.Styles.FontWeight.Bold ? FontWeights.Bold : FontWeights.Normal;
            FontStyle = font.Style == Core.Interaction.Styles.FontStyle.Italic ? FontStyles.Italic : FontStyles.Normal;
            Foreground = Brushes.Black;

            Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new SizeF((float) DesiredSize.Width, (float) DesiredSize.Height);
        }
    }
}
