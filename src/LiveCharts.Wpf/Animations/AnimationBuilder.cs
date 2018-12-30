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
using System.Windows.Media;
using System.Windows.Media.Animation;

#endregion

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// A storyboard builder.
    /// </summary>
    public class AnimationBuilder<T> : IAnimationBuilder
    {
        private readonly bool _isFrameworkElement;
        private List<Tuple<DependencyProperty, Timeline>> _animations = new List<Tuple<DependencyProperty, Timeline>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationBuilder"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="animationLine">The animation vector.</param>
        /// <param name="isFrameworkElement">if set to <c>true</c> [is framework element].</param>
        public AnimationBuilder(
            DependencyObject[] targets,
            TimeSpan duration,
            LiveCharts.Animations.Ease.IEasingFunction animationLine,
            bool isFrameworkElement)
        {
            Targets = targets;
            Duration = duration;
            _isFrameworkElement = isFrameworkElement;
            EasingFunction = animationLine;
            Storyboard = new Storyboard();
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public DependencyObject[] Targets { get; }

        /// <summary>
        /// Gets or sets the storyboard.
        /// </summary>
        /// <value>
        /// The storyboard.
        /// </value>
        protected Storyboard Storyboard { get; set; }

        public LiveCharts.Animations.Ease.IEasingFunction EasingFunction { get; set; }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Begin()
        {
            if (_isFrameworkElement)
            {
                Storyboard.Begin();
                Storyboard.Completed += OnFinished;
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
                    foreach (var target in Targets)
                    {
                        ((Animatable)target).BeginAnimation(x.Item1, (AnimationTimeline)x.Item2);
                    }
                }
            }
        }

        IAnimationBuilder IAnimationBuilder.Property(string property, double from, double to, double delay)
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
                    p = DependencyPropertyDescriptor.FromName(property, typeof(T), typeof(T))
                        .DependencyProperty;
                    break;
            }

            return Property(p, from, to, delay);
        }

        IAnimationBuilder IAnimationBuilder.Property(string property, PointD from, PointD to, double delay)
        {
            var p = DependencyPropertyDescriptor.FromName(property, typeof(T), typeof(T));
            return Property(p.DependencyProperty, new Point(from.X, from.Y), new Point(to.X, to.Y), delay);
        }

        IAnimationBuilder IAnimationBuilder.Property
            (string property, System.Drawing.Color from, System.Drawing.Color to, double delay)
        {
            var p = DependencyPropertyDescriptor.FromName(property, typeof(T), typeof(T));
            return Property(
                p.DependencyProperty,
                Color.FromArgb(from.A, from.R, from.G, from.B),
                Color.FromArgb(to.A, to.R, to.G, to.B),
                delay);
        }        

        /// <summary>
        /// Animates the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="to">Where the animation finishes.</param>
        /// <param name="from">Where the animation starts.</param>
        /// <param name="delay">The delay in proportion of the time line duration, form 0 to 1.</param>
        /// <returns></returns>
        public AnimationBuilder<T> Property(
        DependencyProperty property, double from, double to, double delay = 0)
        {
            var animation = new DoubleAnimation()
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = Duration,
                EasingFunction = new LiveChartsEasingFunction(EasingFunction)
            };

            SetTarget(animation, property);

            return this;
        }

        /// <summary>
        /// Animates the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// /// <param name="delay">The delay in proportion of the time line duration, form 0 to 1.</param>
        /// <returns></returns>
        public AnimationBuilder<T> Property(
            DependencyProperty property, Point from, Point to, double delay = 0)
        {
            var animation = new PointAnimation
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = Duration,
                EasingFunction = new LiveChartsEasingFunction(EasingFunction)
            };

            SetTarget(animation, property);

            return this;
        }

        /// <summary>
        /// Animates the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// /// <param name="delay">The delay in proportion of the time line duration, form 0 to 1.</param>
        /// <returns></returns>
        public AnimationBuilder<T> Property(
            DependencyProperty property, Color from, Color to, double delay = 0)
        {
            var animation = new ColorAnimation
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = Duration,
                EasingFunction = new LiveChartsEasingFunction(EasingFunction)
            };

            SetTarget(animation, property);

            return this;
        }

        /// <summary>
        /// Runs  the specified callback when the animations are finished.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
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
                Storyboard.Completed += callback;
            }

            return this;
        }

        private void OnFinished(object sender, EventArgs eventArgs)
        {
            Dispose();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Storyboard = null;
            _animations = null;
        }

        private void SetTarget(AnimationTimeline animation, DependencyProperty property)
        {
            if (_isFrameworkElement)
            {
                foreach (var target in Targets)
                {
                    Storyboard.Children.Add(animation);
                    Storyboard.SetTarget(animation, target);
                    Storyboard.SetTargetProperty(animation, new PropertyPath(property));
                }
            }
            else
            {
                _animations.Add(new Tuple<DependencyProperty, Timeline>(property, animation));
            }
        }
    }
}
