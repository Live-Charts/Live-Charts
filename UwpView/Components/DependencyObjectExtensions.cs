using Windows.UI.Xaml;

namespace LiveCharts.Uwp.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Sets if not set.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="dp">The dp.</param>
        /// <param name="value">The value.</param>
        public static void SetIfNotSet(this DependencyObject o, DependencyProperty dp, object value)
        {
            if (o.ReadLocalValue(dp) == DependencyProperty.UnsetValue)
                o.SetValue(dp, value);
        }
    }
}
