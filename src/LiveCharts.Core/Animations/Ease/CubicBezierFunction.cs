namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// A cubic bezier function based on Gaetan Renaudeau article http://greweb.me/2012/02/bezier-curve-based-easing-functions-from-concept-to-implementation/.
    /// </summary>
    public class CubicBezierFunction : IEasingFunction
    {
        // special thanks to 
        // Gaetan Renaudeau
        // http://greweb.me/2012/02/bezier-curve-based-easing-functions-from-concept-to-implementation/

        private readonly double _mx1;
        private readonly double _mx2;
        private readonly double _my1;
        private readonly double _my2;

        /// <summary>
        /// Initializes a new instance of the <see cref="CubicBezierFunction"/> class.
        /// If you need help creating a bezier, there is a visual web site that explains that this function is, take a look at
        /// http://cubic-bezier.com/#.42,0,.58,1 in that link mx1 = 0.42, mx2 = 0, my1 = 0.58 and my2 = 1
        /// </summary>
        /// <param name="mx1">The MX1.</param>
        /// <param name="mx2">The MX2.</param>
        /// <param name="my1">The my1.</param>
        /// <param name="my2">The my2.</param>
        public CubicBezierFunction(double mx1, double mx2, double my1, double my2)
        {
            _mx1 = mx1;
            _mx2 = mx2;
            _my1 = my1;
            _my2 = my2;
        }

        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public double GetProgress(double x)
        {
            // ReSharper disable CompareOfdoublesByEqualityOperator
            if (_mx1 == _my1 && _mx2 == _my2)
            {
                return x;
            }
            // ReSharper restore CompareOfdoublesByEqualityOperator

            return CalcBezier(GetTForX(x), _my1, _my2);
        }

        private double GetTForX(double aX)
        {
            // Newton-Raphson iteration
            double aGuessT = aX;
            for (int i = 0; i < 4; ++i)
            {
                double currentSlope = GetSlope(aGuessT, _mx1, _mx2);
                // ReSharper disable once CompareOfdoublesByEqualityOperator
                if (currentSlope == 0) return aGuessT;
                double currentX = CalcBezier(aGuessT, _mx1, _mx2) - aX;
                aGuessT -= currentX / currentSlope;
            }
            return aGuessT;
        }

        // Returns dx/dt given t, x1, and x2, or dy/dt given t, y1, and y2.
        private static double GetSlope(double aT, double aA1, double aA2)
        {
            return 3f * A(aA1, aA2) * aT * aT + 2f * B(aA1, aA2) * aT + C(aA1);
        }

        private static double CalcBezier(double aT, double aA1, double aA2)
        {
            return ((A(aA1, aA2) * aT + B(aA1, aA2)) * aT + C(aA1)) * aT;
        }

        private static double A(double aA1, double aA2)
        {
            return 1f - 3f * aA2 + 3f * aA1;
        }

        private static double B(double aA1, double aA2)
        {
            return 3f * aA2 - 6f * aA1;
        }

        private static double C(double aA1)
        {
            return 3f * aA1;
        }
    }
}
