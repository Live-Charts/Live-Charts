using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// A storyboard builder... I REALLY hate the current way.
    /// </summary>
    public class Animation<T>
        where T : DependencyObject
    {
        private readonly TimeSpan _speed;
        private readonly Storyboard _storyboard;
        private readonly T _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation{T}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="speed">The speed.</param>
        public Animation(T target, TimeSpan speed)
        {
            _speed = speed;
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

        /// <summary>
        /// To the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="to">To.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public Animation<T> Property(Expression<Func<T, double>> expression, double to, TimeSpan? speed = null)
        {
            var animation = new DoubleAnimation(to, speed ?? _speed);
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(GetPropertyInfo(expression)));
            return this;
        }

        /// <summary>
        /// Properties the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public Animation<T> Property(Expression<Func<T, double>> expression, double from, double to, TimeSpan? speed = null)
        {
            var animation = new DoubleAnimation(from, to, speed ?? _speed);
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(GetPropertyInfo(expression).Name));
            return this;
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

            var member = expression.Body as MemberExpression;
            if (member == null)
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
