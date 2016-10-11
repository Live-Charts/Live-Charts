using System;

namespace UWP
{
    using System.Reflection;

    using Windows.UI.Xaml.Controls;

    public static class XamlUtils
    {
        public static void UpdateBindings(Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Update", new Type[] { });
            update?.Invoke(bindings, null);
        }
    }
}
