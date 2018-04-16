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

using System.Linq;
using System.Windows;

#endregion

namespace LiveCharts.Wpf.Animations
{
    public static class BounceAnimations
    {
        // http://bouncejs.com/
        // type: scale
        // from: 0 to: 1
        // easing: bounce
        // bounces: 6
        // stiffness: 1
        private static readonly double[][] Curve =
        {
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
        };

        /// <summary>
        /// Inverse bounce animation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="property">The property.</param>
        /// <param name="from"></param>
        /// <param name="to">Target top.</param>
        /// <returns></returns>
        public static AnimationBuilder Bounce(
            this AnimationBuilder builder, DependencyProperty property, double from, double to)
        {
            return builder.Property(
                property,
                Curve.Select(x => new Frame(x[0], from + x[1] * (to - from))));
        }
    }
}
