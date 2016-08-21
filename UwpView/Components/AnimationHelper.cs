using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace LiveCharts.Uwp.Components
{
    public static class AnimationHelper
    {
        public static DoubleAnimation CreateDouble(double? to, Duration duration, string targetProperty)
        {
            var animation = new DoubleAnimation()
            {
                To = to,
                Duration = duration
            };
            Storyboard.SetTargetProperty(animation, targetProperty);
            return animation;
        }

        public static void CreateStoryBoardAndBegin(DependencyObject target, params Timeline[] animation)
        {
            var sb = new Storyboard();
            foreach (var timeline in animation)
            {
                Storyboard.SetTarget(timeline, target);
                sb.Children.Add(timeline);
            }
            sb.Begin();
        }

        public static Storyboard CreateStoryBoard(DependencyObject target,params Timeline[] animation)
        {
            var sb = new Storyboard();
            foreach (var timeline in animation)
            {
                Storyboard.SetTarget(timeline, target);
                sb.Children.Add(timeline);
            }
            return sb;
        }
    }
}
