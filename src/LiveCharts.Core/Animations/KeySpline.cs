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

        private readonly float _mx1;
        private readonly float _mx2;
        private readonly float _my1;
        private readonly float _my2;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeySpline"/> class.
        /// </summary>
        /// <param name="mx1">The MX1.</param>
        /// <param name="mx2">The MX2.</param>
        /// <param name="my1">The my1.</param>
        /// <param name="my2">The my2.</param>
        public KeySpline(float mx1, float mx2, float my1, float my2)
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
        public KeyFrame GetFrame(float x)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (_mx1 == _my1 && _mx2 == _my2)
            {
                return new KeyFrame(x, x);
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            return new KeyFrame(x, CalcBezier(GetTForX(x), _my1, _my2));
        }

        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public float GetY(float x)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (_mx1 == _my1 && _mx2 == _my2)
            {
                return x;
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            return CalcBezier(GetTForX(x), _my1, _my2);
        }

        private float GetTForX(float aX)
        {
            // Newton-Raphson iteration
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
        private static float GetSlope(float aT, float aA1, float aA2)
        {
            return 3f * A(aA1, aA2) * aT * aT + 2f * B(aA1, aA2) * aT + C(aA1);
        }

        private static float CalcBezier(float aT, float aA1, float aA2)
        {
            return ((A(aA1, aA2) * aT + B(aA1, aA2)) * aT + C(aA1)) * aT;
        }

        private static float A(float aA1, float aA2)
        {
            return 1f - 3f* aA2 + 3f * aA1;
        }

        private static float B(float aA1, float aA2)
        {
            return 3f * aA2 - 6f * aA1;
        }

        private static float C(float aA1)
        {
            return 3f * aA1;
        }
    }
}
