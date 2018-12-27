using System;
using System.Collections.Generic;

namespace LiveCharts.Core.Animations
{
    /// <summary>
    /// Just an animation.
    /// </summary>
    public class TimeLine
    {
        private static KeyFrame[] _ease;
        private static KeyFrame[] _easeIn;
        private static KeyFrame[] _easeOut;
        private static KeyFrame[] _easeInOut;

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);

        /// <summary>
        /// Gets or sets the time line.
        /// </summary>
        /// <value>
        /// The time line.
        /// </value>
        public IEnumerable<KeyFrame> AnimationLine { get; set; } = Completed;

        /// <summary>
        /// Gets the disable animations vector.
        /// </summary>
        /// <value>
        /// The disable animations.
        /// </value>
        public static IEnumerable<KeyFrame> Completed => new[]
        {
            new KeyFrame(1, 1)
        };

        /// <summary>
        /// Gets the lineal animation vector.
        /// </summary>
        /// <value>
        /// The lineal animation.
        /// </value>
        public static IEnumerable<KeyFrame> Lineal => new[]
        {
            new KeyFrame(0, 0),
            new KeyFrame(1, 1)
        };

        /// <summary>
        /// Gets the ease animation vector.
        /// </summary>
        /// <value>
        /// The ease.
        /// </value>
        public static IEnumerable<KeyFrame> Ease
        {
            get
            {
                if (_ease != null) return _ease;

                var bezier = new KeySpline(0.25f, 0.1f, 0.25f, 1.0f);

                _ease = new[]
                {
                    bezier.GetFrame(0f),
                    bezier.GetFrame(0.125f),
                    bezier.GetFrame(0.25f),
                    bezier.GetFrame(0.5f),
                    bezier.GetFrame(0.625f),
                    bezier.GetFrame(0.75f),
                    bezier.GetFrame(1f)
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
        public static IEnumerable<KeyFrame> EaseIn
        {
            get
            {
                if (_easeIn != null) return _easeIn;

                var bezier = new KeySpline(0.42f, 0f, 1f, 1f);

                _easeIn = new[]
                {
                    bezier.GetFrame(0f),
                    bezier.GetFrame(0.25f),
                    bezier.GetFrame(0.42f),
                    bezier.GetFrame(0.5f),
                    bezier.GetFrame(0.6f),
                    bezier.GetFrame(0.7f),
                    bezier.GetFrame(0.8f),
                    bezier.GetFrame(0.9f),
                    bezier.GetFrame(1f)
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
        public static IEnumerable<KeyFrame> EaseOut
        {
            get
            {
                if (_easeOut != null) return _easeOut;

                var bezier = new KeySpline(0f, 0f, 0.58f, 1f);

                _easeOut = new[]
                {
                    bezier.GetFrame(0f),
                    bezier.GetFrame(0.3f),
                    bezier.GetFrame(0.58f),
                    bezier.GetFrame(0.7f),
                    bezier.GetFrame(0.8f),
                    bezier.GetFrame(0.9f),
                    bezier.GetFrame(1f)
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
        public static IEnumerable<KeyFrame> EaseInOut
        {
            get
            {
                if (_easeInOut != null) return _easeInOut;

                var bezier = new KeySpline(0.42f, 0f, 0.58f, 1f);

                _easeInOut = new[]
                {
                    bezier.GetFrame(0f),
                    bezier.GetFrame(0.125f),
                    bezier.GetFrame(0.25f),
                    bezier.GetFrame(0.375f),
                    bezier.GetFrame(0.5f),
                    bezier.GetFrame(0.75f),
                    bezier.GetFrame(0.875f),
                    bezier.GetFrame(1f)
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
        public static IEnumerable<KeyFrame> BounceSmall => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A2%7D%5D%7D
            new KeyFrame(0f, 0f),
            new KeyFrame(0.0761f, 0.536f),
            new KeyFrame(0.1141f, 0.738f),
            new KeyFrame(0.1512f, 0.881f),
            new KeyFrame(0.1892f, 0.98f),
            new KeyFrame(0.2272f, 1.038f),
            new KeyFrame(0.3023f, 1.072f),
            new KeyFrame(0.5025f, 1.015f),
            new KeyFrame(0.7027f, 0.997f),
            new KeyFrame(1f, 1f)
        };

        /// <summary>
        /// Gets the medium bounce animation vector.
        /// </summary>
        /// <value>
        /// The bounce medium.
        /// </value>
        public static IEnumerable<KeyFrame> BounceMedium => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A4%7D%5D%7D 
            new KeyFrame(0f, 0f),
            new KeyFrame(0.047f, .45f),
            new KeyFrame(0.0941f, .883f),
            new KeyFrame(0.1411f, 1.141f),
            new KeyFrame(0.1872f, 1.212f),
            new KeyFrame(0.2432f, 1.151f),
            new KeyFrame(0.2993f, 1.048f),
            new KeyFrame(0.3554f, 0.979f),
            new KeyFrame(0.4104f, 0.961f),
            new KeyFrame(0.5215f, 0.991f),
            new KeyFrame(0.6326f, 1.007f),
            new KeyFrame(0.8549f, 0.999f),
            new KeyFrame(1f, 1f)
        };

        /// <summary>
        /// Gets the large bounce animation vector.
        /// </summary>
        /// <value>
        /// The bounce large.
        /// </value>
        public static IEnumerable<KeyFrame> BounceLarge => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A6%7D%5D%7D
            new KeyFrame(0f, 0f),
            new KeyFrame(0.034f, .407f),
            new KeyFrame(0.0681f, 0.893f),
            new KeyFrame(0.1021f, 1.226f),
            new KeyFrame(0.1361f, 1.332f),
            new KeyFrame(0.1752f, 1.239f),
            new KeyFrame(0.2132f, 1.069f),
            new KeyFrame(0.2523f, 0.938f),
            new KeyFrame(0.2903f, 0.897f),
            new KeyFrame(0.3674f, 0.979f),
            new KeyFrame(0.4444f, 1.032f),
            new KeyFrame(0.5986f, 0.99f),
            new KeyFrame(0.7528f, 1.003f),
            new KeyFrame(0.9069f, 0.999f),
            new KeyFrame(1f, 1f)
        };

        /// <summary>
        /// Gets the extra large bounce animation vector.
        /// </summary>
        /// <value>
        /// The bounce extra large.
        /// </value>
        public static IEnumerable<KeyFrame> BounceExtraLarge => new[]
        {
            // http://bouncejs.com#%7Bs%3A%5B%7BT%3A%22c%22%2Ce%3A%22b%22%2Cd%3A1000%2CD%3A0%2Cf%3A%7Bx%3A0%2Cy%3A0%7D%2Ct%3A%7Bx%3A1%2Cy%3A1%7D%2Cs%3A1%2Cb%3A10%7D%5D%7D
            new KeyFrame(0f, 0f),
            new KeyFrame(0.022f, 0.368f),
            new KeyFrame(0.044f, 0.916f),
            new KeyFrame(0.0661f, 1.346f),
            new KeyFrame(0.0881f, 1.498f),
            new KeyFrame(0.1121f, 1.362f),
            new KeyFrame(0.1361f, 1.078f),
            new KeyFrame(0.1602f, 0.84f),
            new KeyFrame(0.1842f, 0.759f),
            new KeyFrame(0.2082f, 0.829f),
            new KeyFrame(0.2322f, 0.967f),
            new KeyFrame(0.2563f, 1.08f),
            new KeyFrame(0.2793f, 1.117f),
            new KeyFrame(0.3273f, 1.016f),
            new KeyFrame(0.3744f, 0.943f),
            new KeyFrame(0.4695f, 1.028f),
            new KeyFrame(0.5656f, 0.987f),
            new KeyFrame(0.6607f, 1.006f),
            new KeyFrame(0.7558f, 0.997f),
            new KeyFrame(0.8509f, 1.002f),
            new KeyFrame(0.9469f, 1f)
        };
    }
}