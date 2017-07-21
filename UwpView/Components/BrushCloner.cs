//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp.Components
{
    internal static class BrushCloner
    {
        public static Brush Clone(this Brush brush)
        {
            var colorBrush = brush as SolidColorBrush;
            if (colorBrush != null)
            {
                return new SolidColorBrush
                {
                    Color = colorBrush.Color,
                    Opacity = colorBrush.Opacity,
                    Transform = colorBrush.Transform,
                    RelativeTransform = colorBrush.RelativeTransform
                };
            }

            var gradientBrush = brush as LinearGradientBrush;
            if (gradientBrush != null)
            {
                return new LinearGradientBrush
                {
                    ColorInterpolationMode = gradientBrush.ColorInterpolationMode,
                    EndPoint = gradientBrush.EndPoint,
                    GradientStops = gradientBrush.GradientStops,
                    MappingMode = gradientBrush.MappingMode,
                    Opacity = gradientBrush.Opacity,
                    RelativeTransform = gradientBrush.RelativeTransform,
                    StartPoint = gradientBrush.StartPoint,
                    SpreadMethod = gradientBrush.SpreadMethod,
                    Transform = gradientBrush.Transform
                };
            }

            var imageBrush = brush as ImageBrush;
            if (imageBrush != null)
            {
                return new ImageBrush
                {
                    AlignmentX = imageBrush.AlignmentX,
                    AlignmentY = imageBrush.AlignmentY,
                    ImageSource = imageBrush.ImageSource,
                    Opacity = imageBrush.Opacity,
                    RelativeTransform = imageBrush.RelativeTransform,
                    Stretch = imageBrush.Stretch,
                    Transform = imageBrush.Transform
                };
            }

            return null;
        }
    }
}
