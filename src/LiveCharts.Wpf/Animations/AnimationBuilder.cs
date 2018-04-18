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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LiveCharts.Core;

#endregion

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// A storyboard builder.
    /// </summary>
    public class AnimationBuilder
    {
        private Storyboard _storyboard;
        private DependencyObject _target;
        private readonly bool _isFe;
        private List<Tuple<DependencyProperty, Timeline>> _animations = new List<Tuple<DependencyProperty, Timeline>>();

        public AnimationBuilder(bool isFe)
        {
            _storyboard = new Storyboard();
            _isFe = isFe;
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Begin()
        {
            if (_isFe)
            {
                _storyboard.Begin();
                _storyboard.Completed += OnFinished;
            }
            else
            {
                if (_animations.Count > 0) _animations[0].Item2.Completed += OnFinished;
                foreach (var x in _animations)
                {
                    ((Animatable) _target).BeginAnimation(x.Item1, (AnimationTimeline) x.Item2);
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
            Duration = speedSpan;
            return this;
        }

        /// <summary>
        /// Animates the specified double property.
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
                ? new DoubleAnimation(to, speed ?? Duration)
                : new DoubleAnimation(from.Value, to, speed ?? Duration);
            animation.RepeatBehavior = new RepeatBehavior(1);
            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            return this;
        }

        /// <summary>
        /// Animates the specified property using a key frames.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="frames">The frames.</param>
        /// <returns></returns>
        public AnimationBuilder Property(DependencyProperty property, IEnumerable<Frame> frames)
        {
            var animation = new DoubleAnimationUsingKeyFrames { RepeatBehavior = new RepeatBehavior(1) };
            foreach (var frame in frames)
            {
                animation.KeyFrames.Add(
                    new LinearDoubleKeyFrame(
                        frame.ToValue,
                        TimeSpan.FromMilliseconds(Duration.TotalMilliseconds * frame.ElapsedTime)));
            }

            _storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            return this;
        }

        /// <summary>
        /// Animates to the specifies point property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public AnimationBuilder Property(
            DependencyProperty property, Point to, Point? from = null, TimeSpan? speed = null)
        {
            if (_isFe)
            {
                var feAnim = from == null
                    ? new PointAnimation(to, speed ?? Duration)
                    : new PointAnimation(from.Value, to, speed ?? Duration);
                feAnim.RepeatBehavior = new RepeatBehavior(1);
                _storyboard.Children.Add(feAnim);
                Storyboard.SetTarget(feAnim, _target);
                Storyboard.SetTargetProperty(feAnim, new PropertyPath(property));
                return this;
            }

            // storyboard for some reason only works with FrameworkElement, 
            // because <- insert reason here???? ->

            var animation = from == null
                ? new PointAnimation(to, speed ?? Duration)
                : new PointAnimation(from.Value, to, speed ?? Duration);

            _animations.Add(new Tuple<DependencyProperty, Timeline>(property, animation));

            return this;
        }

        /// <summary>
        /// Animates to the specified color property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public AnimationBuilder Property(
            DependencyProperty property, Color to, Color? from = null, TimeSpan? speed = null)
        {
            if (_isFe)
            {
                var feAnim = from == null
                    ? new ColorAnimation(to, speed ?? Duration)
                    : new ColorAnimation(from.Value, to, speed ?? Duration);
                feAnim.RepeatBehavior = new RepeatBehavior(1);
                _storyboard.Children.Add(feAnim);
                Storyboard.SetTarget(feAnim, _target);
                Storyboard.SetTargetProperty(feAnim, new PropertyPath(property));
                return this;
            }

            var animation = from == null
                ? new ColorAnimation(to, speed ?? Duration)
                : new ColorAnimation(from.Value, to, speed ?? Duration);

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
            if (!_isFe)
            {
                if (_animations.Count < 1)
                {
                    throw new LiveChartsException(
                        "No animation was found, therefore it is not possible to listen for animation completion.",
                        155);
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

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _storyboard = null;
            _animations = null;
        }
    }
}
