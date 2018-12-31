#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using LiveCharts.Animations;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#endregion

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// An animation builder for WPF.
    /// </summary>
    public class AnimationBuilder<T> : IAnimationBuilder
    {
        private static readonly bool _isFrameworkElement;
        private List<Tuple<DependencyProperty, Timeline>> _animations = new List<Tuple<DependencyProperty, Timeline>>();
        private readonly T _target;
        private readonly Storyboard _storyboard;
        private readonly AnimatableArguments _animatableArguments;

        static AnimationBuilder()
        {
            _isFrameworkElement = typeof(FrameworkElement).IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IAnimationBuilder"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="animationLine">The animation vector.</param>
        /// <param name="isFrameworkElement">if set to <c>true</c> [is framework element].</param>
        public AnimationBuilder(
            T target, AnimatableArguments arguments)
        {
            _target = target;
            _animatableArguments = arguments;
            _storyboard = new Storyboard();
        }

        /// <inheritdoc></inheritdoc>
        public void Begin()
        {
            if (_isFrameworkElement)
            {
                _storyboard.Begin();
                _storyboard.Completed += OnFinished;
            }
            else
            {
                // this is just a work around.
                // storyboards do not work on all Animatable objects, only in FrameworkElements.
                // I think this in an error in the WPF framework design
                if (_animations.Count > 0)
                {
                    _animations[0].Item2.Completed += OnFinished;
                }
                foreach (Tuple<DependencyProperty, Timeline> x in _animations)
                {
                    Animatable animatable = _target as Animatable;
                    if (animatable == null) throw new LiveChartsException(107, typeof(T).Name);
                    animatable.BeginAnimation(x.Item1, (AnimationTimeline)x.Item2);
                }
            }
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Property(
            string property, double from, double to, double delay)
        {
            DependencyProperty p;
            switch (property)
            {
                case nameof(IShape.Left):
                    p = Canvas.LeftProperty;
                    break;
                case nameof(IShape.Top):
                    p = Canvas.TopProperty;
                    break;
                default:
                    p = DependencyPropertyDescriptor.FromName(property, typeof(T), typeof(T)).DependencyProperty;
                    break;
            }

            var animation = new DoubleAnimation
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = _animatableArguments.Duration,
                EasingFunction = new LiveChartsEasingFunction(_animatableArguments.EasingFunction),
                From = from,
                To = to
            };

            SetTarget(animation, p);

            return this;
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Property(
            string property, PointD from, PointD to, double delay)
        {
            var p = DependencyPropertyDescriptor.FromName(property, typeof(T), typeof(T)).DependencyProperty;

            var animation = new PointAnimation
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = _animatableArguments.Duration,
                EasingFunction = new LiveChartsEasingFunction(_animatableArguments.EasingFunction),
                From = new Point(from.X, from.Y),
                To = new Point(to.X, to.Y)
            };

            SetTarget(animation, p);

            return this;
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Property(
            string property, System.Drawing.Color from, System.Drawing.Color to, double delay)
        {
            var p = DependencyPropertyDescriptor.FromName(property, typeof(T), typeof(T)).DependencyProperty;

            var animation = new ColorAnimation
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = _animatableArguments.Duration,
                EasingFunction = new LiveChartsEasingFunction(_animatableArguments.EasingFunction),
                From = System.Windows.Media.Color.FromArgb(from.A, from.R, from.G, from.B),
                To = System.Windows.Media.Color.FromArgb(to.A, to.R, to.G, to.B)
            };

            SetTarget(animation, p);

            return this;
        }

        /// <inheritdoc></inheritdoc>
        public IAnimationBuilder Then(EventHandler callback)
        {
            if (!_isFrameworkElement)
            {
                if (_animations.Count < 1)
                {
                    throw new LiveChartsException(141);
                }
                _animations[0].Item2.Completed += callback;
            }
            else
            {
                _storyboard.Completed += callback;
            }

            return this;
        }

        private void OnFinished(object sender, EventArgs eventArgs)
        {
            Dispose();
        }

        /// <inheritdoc></inheritdoc>
        public void Dispose()
        {
            _storyboard.Completed -= OnFinished;
            if (_animations.Count > 0)
            {
                _animations[0].Item2.Completed -= OnFinished;
            }
            _animations.Clear();
            _animations = null;
        }

        private void SetTarget(AnimationTimeline animation, DependencyProperty property)
        {
            if (_isFrameworkElement)
            {
                _storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, _target as DependencyObject);
                Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            }
            else
            {
                _animations.Add(new Tuple<DependencyProperty, Timeline>(property, animation));
            }
        }
    }
}
