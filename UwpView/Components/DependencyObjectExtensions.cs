using Windows.UI.Xaml;

namespace LiveCharts.Uwp.Components
{
    public static class DependencyObjectExtensions
    {
        public static void SetIfNotSet(this DependencyObject o, DependencyProperty dp, object value)
        {
            if (o.ReadLocalValue(dp) == DependencyProperty.UnsetValue)
                o.SetValue(dp, value);
        }
    }
}
