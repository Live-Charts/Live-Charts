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

        public StoryboardBuilder AtSpeed(TimeSpan speedSpan)
        {
            _speed = speedSpan;
            return this;
        }

        /// <summary>
        /// Animates the specified property.
        /// </summary>
        /// <param name="dependencyProperty">Name of the property.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public StoryboardBuilder Property(DependencyProperty dependencyProperty, double to, double? from = null, TimeSpan? speed = null)
        {
            var animation = from == null
                ? new DoubleAnimation(to, speed ?? _speed)
                : new DoubleAnimation(from.Value, to, speed ?? _speed);
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(dependencyProperty));
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
