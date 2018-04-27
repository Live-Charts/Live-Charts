using System.Collections.Generic;

namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// Predefined animation vectors.
    /// </summary>
    public static class TimeLines
    {
        private static Frame[] _ease;
        private static Frame[] _easeIn;
        private static Frame[] _easeOut;
        private static Frame[] _easeInOut;

        /// <summary>
        /// Gets the lineal animation vector.
        /// </summary>
        /// <value>
        /// The lineal animation.
        /// </value>
        public static IEnumerable<Frame> Lineal => new[]
        {
            new Frame(0, 0),
            new Frame(1, 1)
        };

        /// <summary>
        /// Gets the ease animation vector.
        /// </summary>
        /// <value>
        /// The ease.
        /// </value>
        public static IEnumerable<Frame> Ease
        {
            get
            {
                if (_ease != null) return _ease;

                var bezier = new KeySpline(0.25, 0.1, 0.25, 1.0);

                _ease = new[]
                {
                    bezier.GetFrame(0),
                    bezier.GetFrame(0.125),
                    bezier.GetFrame(0.25),
                    bezier.GetFrame(0.5),
                    bezier.GetFrame(0.625),
                    bezier.GetFrame(0.75),
                    bezier.GetFrame(1)
                };

                return _ease;
            }
        }

        /// <summary>
        /// Gets the ease in animation vector.
        /// </summary>
        /// <value>
        /// The ease in.
        /// </value>
        public static IEnumerable<Frame> EaseIn
        {
            get
            {
                if (_easeIn != null) return _easeIn;

                var bezier = new KeySpline(0.42, 0, 1, 1);

                _easeIn = new[]
                {
                    bezier.GetFrame(0),
                    bezier.GetFrame(0.25),
                    bezier.GetFrame(0.42),
                    bezier.GetFrame(0.5),
                    bezier.GetFrame(0.6),
                    bezier.GetFrame(0.7),
                    bezier.GetFrame(0.8),
                    bezier.GetFrame(0.9),
                    bezier.GetFrame(1)
                };

                return _easeIn;
            }
        }

        /// <summary>
        /// Gets the ease out animation vector.
        /// </summary>
        /// <value>
        /// The ease out.
        /// </value>
        public static IEnumerable<Frame> EaseOut
        {
            get
            {
                if (_easeOut != null) return _easeOut;

                var bezier = new KeySpline(0, 0, 0.58, 1);

                _easeOut = new[]
                {
                    bezier.GetFrame(0),
                    bezier.GetFrame(0.3),
                    bezier.GetFrame(0.58),
                    bezier.GetFrame(0.7),
                    bezier.GetFrame(0.8),
                    bezier.GetFrame(0.9),
                    bezier.GetFrame(1)
                };

                return _easeOut;
            }
        }

        /// <summary>
        /// Gets the ease in out. animation vector.
        /// </summary>
        /// <value>
        /// The ease in out.
        /// </value>
        public static IEnumerable<Frame> EaseInOut
        {
            get
            {
                if (_easeInOut != null) return _easeInOut;

                var bezier = new KeySpline(0.42, 0, 0.58, 1);

                _easeInOut = new[]
                {
                    bezier.GetFrame(0),
                    bezier.GetFrame(0.125),
                    bezier.GetFrame(0.25),
                    bezier.GetFrame(0.375),
                    bezier.GetFrame(0.5),
                    bezier.GetFrame(0.75),
                    bezier.GetFrame(0.875),
                    bezier.GetFrame(1)
                };

                return _easeInOut;
            }
        }

        /// <summary>
        /// Gets a small bounce animation vector.
        /// </summary>
        /// <value>
        /// The bounce small.
        /// </value>
        public static IEnumerable<Frame> BounceSmall => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A2%7D%5D%7D
            new Frame(0d, 0d),
            new Frame(0.0761, 0.536),
            new Frame(0.1141, 0.738),
            new Frame(0.1512, 0.881),
            new Frame(0.1892, 0.98),
            new Frame(0.2272, 1.038),
            new Frame(0.3023, 1.072),
            new Frame(0.5025, 1.015),
            new Frame(0.7027, 0.997),
            new Frame(1d, 1d)
        };

        /// <summary>
        /// Gets the medium bounce animation vector.
        /// </summary>
        /// <value>
        /// The bounce medium.
        /// </value>
        public static IEnumerable<Frame> BounceMedium => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A4%7D%5D%7D 
            new Frame(0d, 0d),
            new Frame(0.047d, .45d),
            new Frame(0.0941d, .883d),
            new Frame(0.1411d, 1.141d),
            new Frame(0.1872d, 1.212d),
            new Frame(0.2432d, 1.151d),
            new Frame(0.2993d, 1.048d),
            new Frame(0.3554d, 0.979d),
            new Frame(0.4104d, 0.961d),
            new Frame(0.5215d, 0.991d),
            new Frame(0.6326d, 1.007d),
            new Frame(0.8549d, 0.999d),
            new Frame(1d, 1d)
        };

        /// <summary>
        /// Gets the large bounce animation vector.
        /// </summary>
        /// <value>
        /// The bounce large.
        /// </value>
        public static IEnumerable<Frame> BounceLarge => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A6%7D%5D%7D
            new Frame(0d, 0d),
            new Frame(0.034, .407),
            new Frame(0.0681, .893),
            new Frame(0.1021, 1.226),
            new Frame(0.1361, 1.332),
            new Frame(0.1752, 1.239),
            new Frame(0.2132, 1.069),
            new Frame(0.2523, 0.938),
            new Frame(0.2903, 0.897),
            new Frame(0.3674, 0.979),
            new Frame(0.4444, 1.032),
            new Frame(0.5986, 0.99),
            new Frame(0.7528, 1.003),
            new Frame(0.9069, 0.999),
            new Frame(1d, 1d)
        };

        /// <summary>
        /// Gets the extra large bounce animation vector.
        /// </summary>
        /// <value>
        /// The bounce extra large.
        /// </value>
        public static IEnumerable<Frame> BounceExtraLarge => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A10%7D%5D%7D
            new Frame(0d, 0d),
            new Frame(0.022, 0.368),
            new Frame(0.044, 0.916),
            new Frame(0.0661, 1.346),
            new Frame(0.0881, 1.498),
            new Frame(0.1121, 1.362),
            new Frame(0.1361, 1.078),
            new Frame(0.1602, 0.84),
            new Frame(0.1842, 0.759),
            new Frame(0.2082, 0.829),
            new Frame(0.2322, 0.967),
            new Frame(0.2563, 1.08),
            new Frame(0.2793, 1.117),
            new Frame(0.3273, 1.016),
            new Frame(0.3744, 0.943),
            new Frame(0.4695, 1.028),
            new Frame(0.5656, 0.987),
            new Frame(0.6607, 1.006),
            new Frame(0.7558, 0.997),
            new Frame(0.8509, 1.002),
            new Frame(0.9469, 1)
        };
    }
}