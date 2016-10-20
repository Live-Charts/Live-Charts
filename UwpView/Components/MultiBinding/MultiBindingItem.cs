using Windows.UI.Xaml;

namespace LiveCharts.Uwp.Components.MultiBinding
{
    /// <summary>
    /// A multiple binding item.
    /// </summary>
    public class MultiBindingItem : DependencyObject
    {
        /// <summary>
        /// Gets or sets the binding value.
        /// </summary>
        /// <value>The binding value.</value>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="Value" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(MultiBindingItem), new PropertyMetadata(null, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var multiBindingItem = (MultiBindingItem)d;

            multiBindingItem.Update();
        }

        internal MultiBindingItemCollection Parent { get; set; }

        private void Update()
        {
            var parent = Parent;

            parent?.Update();
        }
    }
}
