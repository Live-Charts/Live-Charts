using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The animations extensions.
    /// </summary>
    public static class AnimationsExtensions
    {
        public static Animation<T> Animate<T>(this T element)
        where T : FrameworkElement
        {
            return new Animation<T>(element);
        }
    }

    /// <summary>
    /// A storyboard builder... I REALLY hate the current way.
    /// </summary>
    public class Animation<T>
        where T : DependencyObject
    {
        private TimeSpan _speed;
        private readonly Storyboard _storyboard;
        private readonly T _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation{T}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public Animation(T target)
        {
            _storyboard = new Storyboard();
            _storyboard.Begin();
            _target = target;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            _storyboard.Begin();
        }

        public Animation<T> AtSpeed(TimeSpan speedSpan)
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
        public Animation<T> Property(DependencyProperty dependencyProperty, double to, double? from = null, TimeSpan? speed = null)
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
        public Animation<T> Property(string propertyName, double to, double? from = null, TimeSpan? speed = null)
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
        /// Animates the specified property.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public Animation<T> Property(Expression<Func<T, double>> expression, double to, double? from = null, TimeSpan? speed = null)
        {
            return Property(GetPropertyInfo(expression).Name, to, from, speed);
        }

        /// <summary>
        /// Runs  the specified callback when the animations are finished.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public Animation<T> Then(EventHandler callback)
        {
            _storyboard.Completed += callback;
            return this;
        }

        // based on => https://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            var type = typeof(TSource);

            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException(
                    $"Expression '{expression}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

            if (propInfo.ReflectedType != null && 
                type != propInfo.ReflectedType && 
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(
                    $"Expression '{expression}' refers to a property that is not from type {type}.");

            return propInfo;
        }
    }
}
