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

using System.Collections.Generic;
using System.Linq;
using System.Windows;

#endregion

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// 
    /// </summary>
    public static class BounceAnimations
    {
        private static readonly double[][][] Curves =
        {
            // small
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A2%7D%5D%7D
            new []
            {
                new []{0d,0d},
                new []{0.0761,0.536},
                new []{0.1141,0.738},
                new []{0.1512,0.881},
                new []{0.1892,0.98},
                new []{0.2272,1.038},
                new []{0.3023,1.072},
                new []{0.5025,1.015},
                new []{0.7027,0.997},
                new []{1d,1d}
            },

            // medium
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A4%7D%5D%7D 
            new []
            {
                new[] {0d, 0d},
                new[] {0.047d, .45d},
                new[] {0.0941d, .883d},
                new[] {0.1411d, 1.141d},
                new[] {0.1872d, 1.212d},
                new[] {0.2432d, 1.151d},
                new[] {0.2993d, 1.048d},
                new[] {0.3554d, 0.979d},
                new[] {0.4104d, 0.961d},
                new[] {0.5215d, 0.991d},
                new[] {0.6326d, 1.007d},
                new[] {0.8549d, 0.999d},
                new[] {1d, 1d}
            },

            // large
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A6%7D%5D%7D
            new []
            {
                new[] {0d, 0d},
                new[] {0.034, .407},
                new[] {0.0681, .893},
                new[] {0.1021, 1.226},
                new[] {0.1361, 1.332},
                new[] {0.1752, 1.239},
                new[] {0.2132, 1.069},
                new[] {0.2523, 0.938},
                new[] {0.2903, 0.897},
                new[] {0.3674, 0.979},
                new[] {0.4444, 1.032},
                new[] {0.5986, 0.99},
                new[] {0.7528, 1.003},
                new[] {0.9069, 0.999},
                new[] {1d, 1d}
            },

            // extra large
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A10%7D%5D%7D
            new []
            {
                new []{0d,0d},
                new []{0.022, 0.368},
                new []{0.044, 0.916},
                new []{0.0661, 1.346},
                new []{0.0881, 1.498},
                new []{0.1121, 1.362},
                new []{0.1361, 1.078},
                new []{0.1602, 0.84},
                new []{0.1842, 0.759},
                new []{0.2082, 0.829},
                new []{0.2322, 0.967},
                new []{0.2563, 1.08},
                new []{0.2793, 1.117},
                new []{0.3273, 1.016},
                new []{0.3744, 0.943},
                new []{0.4695, 1.028},
                new []{0.5656, 0.987},
                new []{0.6607, 1.006},
                new []{0.7558, 0.997},
                new []{0.8509, 1.002},
                new []{0.9469, 0.999},
                new []{1d,1d}
            }
        };

        /// <summary>
        /// Inverse bounce animation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="property">The property.</param>
        /// <param name="from"></param>
        /// <param name="to">Target top.</param>
        /// <param name="magnitude">The curve.</param>
        /// <param name="scale">The bounce scale.</param>
        /// <param name="delay">The delay in proportion to the total animation speed, form 0 to 1.</param>
        /// <returns></returns>
        public static AnimationBuilder Bounce(
            this AnimationBuilder builder, DependencyProperty property, double from, double to,
            BounceMagnitude magnitude = BounceMagnitude.Medium, double delay = 0, int scale = 1)
        {
            return builder.Property(
                property,
                GetCurve(magnitude, delay)
                    .Select(x => 
                        new Frame(x[0], from + x[1] * (to - from) + (1 - x[1]) * scale)));
        }

        private static IEnumerable<double[]> GetCurve(BounceMagnitude magnitude, double delay)
        {
            if (delay > 0)
            {
                yield return new []{0d,0d};
                yield return new []{delay, 0d};
            }

            var remaining = 1 - delay;

            foreach (var curve in Curves[(int) magnitude])
            {
                yield return new[] {delay + remaining * curve[0], curve[1]};
            }
        }
    }
}
