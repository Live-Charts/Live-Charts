#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodr�guez Orozco & LiveCharts contributors
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using LiveCharts.Core.Animations;

#endregion

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// The animations extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Animates the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="timeLine">The animation.</param>
        /// <returns></returns>
        public static AnimationBuilder Animate(
            this FrameworkElement element, TimeLine timeLine)
        {
            return new AnimationBuilder(
                element, timeLine.Duration, timeLine.AnimationLine, true);
        }

        /// <summary>
        /// Animates the specified element.
        /// </summary>
        /// <param name="animatable">The animatable.</param>
        /// <param name="timeLine">The animation vector.</param>
        /// <returns></returns>
        public static AnimationBuilder Animate(
            this Animatable animatable, TimeLine timeLine)
        {
            return new AnimationBuilder(
                animatable, timeLine.Duration, timeLine.AnimationLine, false);
        }
    }
}