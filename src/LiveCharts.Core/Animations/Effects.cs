﻿#region License
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

#endregion

namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// Effects class.
    /// </summary>
    public static class Effects
    {
        /// <summary>
        /// Gets the animated float dash array.
        /// </summary>
        /// <param name="strokeDashArray">The stroke dash array.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static IEnumerable<double> GetAnimatedDashArray(IEnumerable<double> strokeDashArray, float length)
        {
            double stack = 0d;

            if (strokeDashArray == null)
            {
                yield return length;
                yield return length;
                yield break;
            }

            IEnumerator<double> e = strokeDashArray.GetEnumerator();
            bool isStroked = true;

            while (stack < length)
            {
                if (!e.MoveNext())
                {
                    e.Reset();
                    e.MoveNext();
                }
                isStroked = !isStroked;
                yield return e.Current;
                stack += e.Current;
            }

            if (isStroked)
            {
                yield return 0;
            }
            yield return length;
            e.Dispose();
        }

        /// <summary>
        /// Gets the animated stroke dash array.
        /// </summary>
        /// <param name="strokeDashArray">The stroke dash array.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static IEnumerable<float> GetAnimatedDashArray(IEnumerable<float> strokeDashArray, float length)
        {
            float stack = 0f;

            if (strokeDashArray == null)
            {
                yield return length;
                yield return length;
                yield break;
            }

            IEnumerator<float> e = strokeDashArray.GetEnumerator();
            bool isStroked = true;

            while (stack < length)
            {
                if (!e.MoveNext())
                {
                    e.Reset();
                    e.MoveNext();
                }
                isStroked = !isStroked;
                yield return e.Current;
                stack += e.Current;
            }

            if (isStroked)
            {
                yield return 0;
            }
            yield return length;
            e.Dispose();
        }
    }
}
