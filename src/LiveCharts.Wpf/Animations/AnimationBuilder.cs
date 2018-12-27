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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Core;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Shapes;

#endregion

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// A storyboard builder.
    /// </summary>
    public class AnimationBuilder : IAnimationBuilder
    {
        private readonly bool _isFrameworkElement;
        private readonly IEnumerable<KeyFrame> _animationLine;
        private List<Tuple<DependencyProperty, Timeline>> _animations = new List<Tuple<DependencyProperty, Timeline>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationBuilder"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="animationLine">The animation vector.</param>
        /// <param name="isFrameworkElement">if set to <c>true</c> [is framework element].</param>
        public AnimationBuilder(
            DependencyObject target, 
            TimeSpan duration, 
            IEnumerable<KeyFrame> animationLine, 
            bool isFrameworkElement)
        {
            Target = target;
            Duration = duration;
            _isFrameworkElement = isFrameworkElement;
            _animationLine = animationLine;
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
        public DependencyObject Target { get; }

        /// <summary>
        /// Gets or sets the storyboard.
        /// </summary>
        /// <value>
        /// The storyboard.
        /// </value>
        protected Storyboard Storyboard { get; set; }

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
                // I think this in an error in the WPF framework design
                // this is just a work around.
                // storyboards do not work on all Animatable objects, only in FrameworkElements.
                if (_animations.Count > 0)
                {
                    _animations[0].Item2.Completed += OnFinished;
                }
                foreach (Tuple<DependencyProperty, Timeline> x in _animations)
                {
                    ((Animatable) Target).BeginAnimation(x.Item1, (AnimationTimeline) x.Item2);
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
                    p = DependencyPropertyDescriptor.FromName(property, Target.GetType(), Target.GetType())
                        .DependencyProperty;
                    break;
            }

            return Property(p, from, to, delay);
        }

        IAnimationBuilder IAnimationBuilder.Property(string property, PointD from, PointD to, double delay)
        {
            var p = DependencyPropertyDescriptor.FromName(property, Target.GetType(), Target.GetType());
            return Property(p.DependencyProperty, new Point(from.X, from.Y), new Point(to.X, to.Y), delay);
        }

        IAnimationBuilder IAnimationBuilder.Property
            (string property, System.Drawing.Color from, System.Drawing.Color to, double delay)
        {
            var p = DependencyPropertyDescriptor.FromName(property, Target.GetType(), Target.GetType());
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
        public AnimationBuilder Property(
            DependencyProperty property, double from, double to, double delay = 0)
        {
            var animation = new DoubleAnimationUsingKeyFrames
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = Duration
            };

            IEnumerable<KeyFrame> frames = delay > 0 ? _animationLine.Delay((float) delay) : _animationLine;

            foreach (var frame in frames)
            {
                animation.KeyFrames.Add(
                    new LinearDoubleKeyFrame(
                        from + frame.Value * (to - from),
                        KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(Duration.TotalMilliseconds * frame.Time))));
            }

            if (_isFrameworkElement)
            {
                Storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            }
            else
            {
               _animations.Add(new Tuple<DependencyProperty, Timeline>(property, animation));
            }

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
        public AnimationBuilder Property(
            DependencyProperty property, Point from, Point to, double delay = 0)
        {
            var animation = new PointAnimationUsingKeyFrames
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = Duration
            };

            IEnumerable<KeyFrame> frames = delay > 0 ? _animationLine.Delay((float) delay) : _animationLine;

            foreach (var frame in frames)
            {
                animation.KeyFrames.Add(
                    new LinearPointKeyFrame(
                        new Point(
                            from.X + frame.Value * (to.X - from.X),
                            from.Y + frame.Value * (to.Y - from.Y)),
                        KeyTime.FromPercent(frame.Time)));
            }

            if (_isFrameworkElement)
            {
                Storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            }
            else
            {
                _animations.Add(new Tuple<DependencyProperty, Timeline>(property, animation));
            }

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
        public AnimationBuilder Property(
            DependencyProperty property, Color from, Color to, double delay = 0)
        {
            var animation = new ColorAnimationUsingKeyFrames
            {
                RepeatBehavior = new RepeatBehavior(1),
                Duration = Duration
            };

            IEnumerable<KeyFrame> frames = delay > 0 ? _animationLine.Delay((float) delay) : _animationLine;

            foreach (var frame in frames)
            {
                animation.KeyFrames.Add(
                    new LinearColorKeyFrame(
                        Color.FromArgb(
                            (byte) (from.A + frame.Value * (to.A - from.A)),
                            (byte) (from.R + frame.Value * (to.R - from.R)),
                            (byte) (from.G + frame.Value * (to.G - from.G)),
                            (byte) (from.B + frame.Value * (to.B - from.B))),
                        KeyTime.FromPercent(frame.Time)));
            }

            if (_isFrameworkElement)
            {
                Storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            }
            else
            {
                _animations.Add(new Tuple<DependencyProperty, Timeline>(property, animation));
            }

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
    }
}
