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
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

#endregion

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// A storyboard builder.
    /// </summary>
    public class AnimationBuilder
    {
        private Storyboard _storyboard;
        private TimeSpan _speed;
        private DependencyObject _target;
        private readonly bool _isFe;
        private List<Tuple<DependencyProperty, Timeline>> _animations = new List<Tuple<DependencyProperty, Timeline>>();

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
            if (_isFe)
            {
                _storyboard.Begin();
            }
            else
            {
                foreach (var tuple in _animations)
                {
                    ((Animatable) _target).BeginAnimation(tuple.Item1, (AnimationTimeline) tuple.Item2);
                }
            }
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
        public AnimationBuilder Property(DependencyProperty property, params Frame[] frames)
        {
            var animation = new DoubleAnimationUsingKeyFrames { RepeatBehavior = new RepeatBehavior(1) };
            foreach (var frame in frames)
            {
                animation.KeyFrames.Add(
                    new SplineDoubleKeyFrame(
                        frame.To,
                        TimeSpan.FromMilliseconds(_speed.TotalMilliseconds * frame.Proportion),
                        new KeySpline(new Point(0.25, 0.5), new Point(0.75, 1))));
            }

            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            return this;
        }

        /// <summary>
        /// Animates the specified property using a defined .
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="frames">The frames.</param>
        /// <returns></returns>
        public AnimationBuilder Properties(IEnumerable<DependencyProperty> properties, params Frame[] frames)
        {
            foreach (var dependencyProperty in properties)
            {
                Property(dependencyProperty, frames);
            }
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

            _animations.Add(new Tuple<DependencyProperty, Timeline>(property, animation));

            return this;
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <returns></returns>
        public AnimationBuilder ChangeTarget(DependencyObject target)
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
            if (!_isFe && _animations.Any())
            {
                _animations[0].Item2.Completed += callback;
            }
            return this;
        }

        public void Dispose()
        {
            _storyboard = null;
            _animations = null;
        }
    }
}
