namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// The key Spline class
    /// </summary>
    public class KeySpline
    {
        // special thanks to 
        // Gaetan Renaudeau
        // http://greweb.me/2012/02/bezier-curve-based-easing-functions-from-concept-to-implementation/

        private readonly double _mx1;
        private readonly double _mx2;
        private readonly double _my1;
        private readonly double _my2;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeySpline"/> class.
        /// </summary>
        /// <param name="mx1">The MX1.</param>
        /// <param name="mx2">The MX2.</param>
        /// <param name="my1">The my1.</param>
        /// <param name="my2">The my2.</param>
        public KeySpline(double mx1, double mx2, double my1, double my2)
        {
            _mx1 = mx1;
            _mx2 = mx2;
            _my1 = my1;
            _my2 = my2;
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public Frame GetFrame(double x)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (_mx1 == _my1 && _mx2 == _my2)
            {
                return new Frame(x, x);
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            return new Frame(x, CalcBezier(GetTForX(x), _my1, _my2));
        }

        private double GetTForX(double aX)
        {
            // Newton raphson iteration
            var aGuessT = aX;
            for (var i = 0; i < 4; ++i)
            {
                var currentSlope = GetSlope(aGuessT, _mx1, _mx2);
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (currentSlope == 0.0) return aGuessT;
                var currentX = CalcBezier(aGuessT, _mx1, _mx2) - aX;
                aGuessT -= currentX / currentSlope;
            }
            return aGuessT;
        }

        // Returns dx/dt given t, x1, and x2, or dy/dt given t, y1, and y2.
        private static double GetSlope(double aT, double aA1, double aA2)
        {
            return 3.0 * A(aA1, aA2) * aT * aT + 2.0 * B(aA1, aA2) * aT + C(aA1);
        }

        private static double CalcBezier(double aT, double aA1, double aA2)
        {
            return ((A(aA1, aA2) * aT + B(aA1, aA2)) * aT + C(aA1)) * aT;
        }

        private static double A(double aA1, double aA2)
        {
            return 1.0 - 3.0 * aA2 + 3.0 * aA1;
        }

        private static double B(double aA1, double aA2)
        {
            return 3.0 * aA2 - 6.0 * aA1;
        }

        private static double C(double aA1)
        {
            return 3.0 * aA1;
        }
    }
}