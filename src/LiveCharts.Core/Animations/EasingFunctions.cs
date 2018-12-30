using LiveCharts.Animations.Ease;

namespace LiveCharts.Animations
{
    /// <summary>
    /// Defines a property value transition.
    /// </summary>
    public static class EasingFunctions
    {
        /// <summary>
        /// Gets a transition that completes in one step.
        /// </summary>
        /// <value>
        /// The disable animations.
        /// </value>
        public static IEasingFunction Completed => new CompletedFunction();

        /// <summary>
        /// Gets the lineal transition.
        /// </summary>
        /// <value>
        /// The lineal animation.
        /// </value>
        public static IEasingFunction Lineal => new LinealFunction();

        /// <summary>
        /// Gets the ease transition.
        /// </summary>
        /// <value>
        /// The ease.
        /// </value>
        public static IEasingFunction Ease => new CubicBezierFunction(.25, 0.1, .25, 1);

        /// <summary>
        /// Gets the ease-in transition.
        /// </summary>
        /// <value>
        /// The ease in.
        /// </value>
        public static IEasingFunction EaseIn => new CubicBezierFunction(0.42, 0, 1, 1);

        /// <summary>
        /// Gets the ease out animation vector.
        /// </summary>
        /// <value>
        /// The ease out.
        /// </value>
        public static IEasingFunction EaseOut => new CubicBezierFunction(0, 0, .58, 1);

        /// <summary>
        /// Gets the ease in out transition.
        /// </summary>
        /// <value>
        /// The ease in out.
        /// </value>
        public static IEasingFunction EaseInOut => new CubicBezierFunction(.42, 0, .58, 1);

        /// <summary>
        /// Gets a rubber band like animation at the begining.
        /// </summary>
        public static IEasingFunction ElasticIn => new ElasticInFunction();

        /// <summary>
        /// Gets a rubber band like animation at the end.
        /// </summary>
        public static IEasingFunction ElasticOut => new ElasticOutFunction();

        /// <summary>
        /// Gets a rubber band like animation at the begining and end.
        /// </summary>
        public static IEasingFunction ElasticInOut => new ElasticInOutFunction();

        /// <summary>
        /// Gets a bounce in function.
        /// </summary>
        public static IEasingFunction BounceIn => new BounceInFunction();

        /// <summary>
        /// Gets a bounce out function.
        /// </summary>
        public static IEasingFunction BounceOut => new BounceOutFunction();

        /// <summary>
        /// Gets a bounce in out function.
        /// </summary>
        public static IEasingFunction BounceInOut => new BounceInOutFunction();
    }
}