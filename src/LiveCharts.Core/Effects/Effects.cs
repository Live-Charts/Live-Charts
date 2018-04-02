using System.Collections.Generic;

namespace LiveCharts.Core.Effects
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
        public static IEnumerable<double> GetAnimatedFloatDashArray(IEnumerable<double> strokeDashArray, float length)
        {
            var stack = 0d;

            if (strokeDashArray == null)
            {
                yield return length;
                yield return length;
                yield break;
            }

            var e = strokeDashArray.GetEnumerator();
            var isStroked = true;

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
        public static IEnumerable<float> GetAnimatedFloatDashArray(IEnumerable<float> strokeDashArray, float length)
        {
            var stack = 0f;

            if (strokeDashArray == null)
            {
                yield return length;
                yield return length;
                yield break;
            }

            var e = strokeDashArray.GetEnumerator();
            var isStroked = true;

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
