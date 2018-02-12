using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The animations extensions.
    /// </summary>
    public static class AnimationsExtensions
    {
        public static AnimationBuilder Animate(this FrameworkElement element)
        {
            return new AnimationBuilder(true).SetTarget(element);
        }

        public static AnimationBuilder Animate(this Animatable animatable)
        {
            return new AnimationBuilder(false).SetTarget(animatable);
        }
    }

    /// <summary>
    /// A storyboard builder.
    /// </summary>
    public class AnimationBuilder
    {
        private readonly Storyboard _storyboard;
        private TimeSpan _speed;
        private DependencyObject _target;
        private readonly bool _isFe;

        public AnimationBuilder(bool isFe)
        {
            _storyboard = new Storyboard();
            _isFe = isFe;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Begin()
        {
            _storyboard.Begin();
        }

        /// <summary>
        /// Specifies the storyboard speed.
        /// </summary>
        /// <param name="speedSpan">The speed span.</param>
        /// <returns></returns>
        public AnimationBuilder AtSpeed(TimeSpan speedSpan)
        {
            _speed = speedSpan;
            return this;
        }

        /// <summary>
        /// Animates the specified property linearly.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public AnimationBuilder Property(DependencyProperty property, double to, double? from = null,
            TimeSpan? speed = null)
        {
            var animation = from == null
                ? new DoubleAnimation(to, speed ?? _speed)
                : new DoubleAnimation(from.Value, to, speed ?? _speed);
            animation.RepeatBehavior = new RepeatBehavior(1);
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            return this;
        }

        /// <summary>
        /// Animates the specified property using a defined .
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="frames">The frames.</param>
        /// <returns></returns>
        public AnimationBuilder Property(DependencyProperty property, params AnimationFrame[] frames)
        {
            var animation = new DoubleAnimationUsingKeyFrames { RepeatBehavior = new RepeatBehavior(1) };
            foreach (var frame in frames)
            {
                animation.KeyFrames.Add(
                    new SplineDoubleKeyFrame(
                        frame.Value,
                        TimeSpan.FromMilliseconds(_speed.TotalMilliseconds * frame.Proportion),
                        new KeySpline(new Point(0.25, 0.5), new Point(0.75, 1))));
            }

            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            return this;
        }

        public AnimationBuilder Property(
            DependencyProperty property, Point to, Point? from = null, TimeSpan? speed = null)
        {
            if (_isFe)
            {
                var feAnim = from == null
                    ? new PointAnimation(to, speed ?? _speed)
                    : new PointAnimation(from.Value, to, speed ?? _speed);
                feAnim.RepeatBehavior = new RepeatBehavior(1);
                _storyboard.Children.Add(feAnim);
                Storyboard.SetTarget(feAnim, _target);
                Storyboard.SetTargetProperty(feAnim, new PropertyPath(property));
                return this;
            }

            // storyboard for some reason only works with FrameworkElement, 
            // because <- insert reason here???? ->

            var animation = from == null
                ? new PointAnimation(to, speed ?? _speed)
                : new PointAnimation(from.Value, to, speed ?? _speed);
            ((Animatable) _target).BeginAnimation(property, animation);
            return this;
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <returns></returns>
        public AnimationBuilder SetTarget(DependencyObject target)
        {
            _target = target;
            return this;
        }

        /// <summary>
        /// Runs  the specified callback when the animations are finished.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public AnimationBuilder Then(EventHandler callback)
        {
            _storyboard.Completed += callback;
            return this;
        }
    }
}
