using System.Windows;
using System.Windows.Media.Animation;

namespace LiveCharts.Wpf.Animations
{
    /// <summary>
    /// The animations extensions.
    /// </summary>
    public static class Extensions
    {
        public static AnimationBuilder Animate(this FrameworkElement element)
        {
            return new AnimationBuilder(true).SetTarget(element);
        }

        public static AnimationBuilder Animate(this Animatable animatable)
        {
            return new AnimationBuilder(false).SetTarget(animatable);
        }
    }
}