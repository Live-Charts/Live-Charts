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

using System.Windows;

#endregion

namespace LiveCharts.Wpf.Framework.Animations
{
    public static class BounceAnimations
    {
        /// <summary>
        /// Bounce animation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="property">The property.</param>
        /// <param name="to">To.</param>
        /// /// <param name="maxBounce">The max bounce limit.</param>
        /// <returns></returns>
        public static AnimationBuilder Bounce(
            this AnimationBuilder builder, DependencyProperty property, double to, double maxBounce = double.NaN)
        {
            double b;
            if (double.IsNaN(maxBounce))
            {
                var l = double.IsNaN(maxBounce) ? .25 : maxBounce;
                 b = to * l;
            }
            else
            {
                b = maxBounce;
            }
            
            return builder.Property(
                property,
                new Frame(0.8, to + b),
                new Frame(0.9, to - b * .6),
                new Frame(1, to));
        }

        /// <summary>
        /// Inverse bounce animation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="property">The property.</param>
        /// <param name="to">To.</param>
        /// <param name="maxBounce">The max bounce limit.</param>
        /// <returns></returns>
        public static AnimationBuilder InverseBounce(
            this AnimationBuilder builder, DependencyProperty property, double to, double maxBounce = double.NaN)
        {
            double b;
            if (double.IsNaN(maxBounce))
            {
                var l = double.IsNaN(maxBounce) ? .25 : maxBounce;
                b = to * l;
            }
            else
            {
                b = maxBounce;
            }
            return builder.Property(
                property,
                new Frame(0.8, to - b),
                new Frame(0.9, to + b * .6),
                new Frame(1, to));
        }
    }
}
