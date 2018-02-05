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
        public static StoryboardBuilder AsStoryboardTarget(this UIElement element)
        {
            return new StoryboardBuilder()
                .SetTarget(element);
        }
    }

    /// <summary>
    /// A storyboard builder.
    /// </summary>
    public class StoryboardBuilder
    {
        private readonly Storyboard _storyboard;
        private TimeSpan _speed;
        private UIElement _target;

        public StoryboardBuilder()
        {
            _storyboard = new Storyboard();
            _storyboard.Begin();
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
        public StoryboardBuilder AtSpeed(TimeSpan speedSpan)
        {
            _speed = speedSpan;
            return this;
        }

        /// <summary>
        /// Animates the specified property using a defined .
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="frames">The frames.</param>
        /// <returns></returns>
        public StoryboardBuilder Property(DependencyProperty property, params AnimationFrame[] frames)
        {
            var animation = new DoubleAnimationUsingKeyFrames {RepeatBehavior = new RepeatBehavior(1)};
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

        /// <summary>
        /// Animates the specified property linearly.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public StoryboardBuilder Property(DependencyProperty property, double to, double? from = null, TimeSpan? speed = null)
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
        /// Animates the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public StoryboardBuilder Property(string propertyName, double to, double? from = null, TimeSpan? speed = null)
        {
            var animation = from == null 
                ? new DoubleAnimation(to, speed ?? _speed)
                : new DoubleAnimation(from.Value, to, speed ?? _speed);
            animation.RepeatBehavior = new RepeatBehavior(1);
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(propertyName));
            return this;
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <returns></returns>
        public StoryboardBuilder SetTarget(UIElement target)
        {
            _target = target;
            return this;
        }

        /// <summary>
        /// Runs  the specified callback when the animations are finished.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public StoryboardBuilder Then(EventHandler callback)
        {
            _storyboard.Completed += callback;
            return this;
        }
    }
}
