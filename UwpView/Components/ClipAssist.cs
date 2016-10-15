using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp.Components
{
    /// <summary>
    /// from https://github.com/xyzzer/WinRTXamlToolkit/blob/master/WinRTXamlToolkit/Controls/Extensions/FrameworkElementExtensions.cs
    /// </summary>
    public class ClipAssist
    {
        /// <summary>
        /// ClipToBounds Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.RegisterAttached(
                "ClipToBounds",
                typeof(bool),
                typeof(ClipAssist),
                new PropertyMetadata(false, OnClipToBoundsChanged));

        /// <summary>
        /// Gets the ClipToBounds property. This dependency property 
        /// indicates whether the element should be clipped to its bounds.
        /// </summary>
        public static bool GetClipToBounds(DependencyObject d)
        {
            return (bool)d.GetValue(ClipToBoundsProperty);
        }

        /// <summary>
        /// Sets the ClipToBounds property. This dependency property 
        /// indicates whether the element should be clipped to its bounds.
        /// </summary>
        public static void SetClipToBounds(DependencyObject d, bool value)
        {
            d.SetValue(ClipToBoundsProperty, value);
        }

        /// <summary>
        /// Handles changes to the ClipToBounds property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnClipToBoundsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldClipToBounds = (bool)e.OldValue;
            bool newClipToBounds = (bool)d.GetValue(ClipToBoundsProperty);

            if (newClipToBounds)
                SetClipToBoundsHandler(d, new ClipToBoundsHandler());
            else
                SetClipToBoundsHandler(d, null);
        }

        /// <summary>
        /// ClipToBoundsHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ClipToBoundsHandlerProperty =
            DependencyProperty.RegisterAttached(
                "ClipToBoundsHandler",
                typeof(ClipToBoundsHandler),
                typeof(ClipAssist),
                new PropertyMetadata(null, OnClipToBoundsHandlerChanged));

        /// <summary>
        /// Gets the ClipToBoundsHandler property. This dependency property 
        /// indicates the handler that handles the updates to the clipping geometry when ClipToBounds is set to true.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ClipToBoundsHandler GetClipToBoundsHandler(DependencyObject d)
        {
            return (ClipToBoundsHandler)d.GetValue(ClipToBoundsHandlerProperty);
        }

        /// <summary>
        /// Sets the ClipToBoundsHandler property. This dependency property 
        /// indicates the handler that handles the updates to the clipping geometry when ClipToBounds is set to true.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetClipToBoundsHandler(DependencyObject d, ClipToBoundsHandler value)
        {
            d.SetValue(ClipToBoundsHandlerProperty, value);
        }

        /// <summary>
        /// Handles changes to the ClipToBoundsHandler property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnClipToBoundsHandlerChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ClipToBoundsHandler oldClipToBoundsHandler = (ClipToBoundsHandler)e.OldValue;
            ClipToBoundsHandler newClipToBoundsHandler = (ClipToBoundsHandler)d.GetValue(ClipToBoundsHandlerProperty);

            if (oldClipToBoundsHandler != null)
                oldClipToBoundsHandler.Detach();
            if (newClipToBoundsHandler != null)
                newClipToBoundsHandler.Attach((FrameworkElement)d);
        }
    }

    #region ClipToBoundsHandler
    /// <summary>
    /// Handles the ClipToBounds attached behavior defined by the attached property
    /// of the <see cref="FrameworkElementExtensions"/> class.
    /// </summary>
    public class ClipToBoundsHandler
    {
        private FrameworkElement _fe;

        private long heightSign;
        private long widthSign;

        /// <summary>
        /// Attaches to the specified framework element.
        /// </summary>
        /// <param name="fe">The fe.</param>
        public void Attach(FrameworkElement fe)
        {
            _fe = fe;
            UpdateClipGeometry();
            fe.Loaded += Loaded;
            fe.SizeChanged += OnSizeChanged;
            this.heightSign = fe.RegisterPropertyChangedCallback(FrameworkElement.HeightProperty, (obj, dp) => UpdateClipGeometry());
            this.widthSign = fe.RegisterPropertyChangedCallback(FrameworkElement.WidthProperty, (obj, dp) => UpdateClipGeometry());
        }

        private void Loaded(Object sender, RoutedEventArgs e)
        {
            UpdateClipGeometry();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdateClipGeometry();
        }

        private void UpdateClipGeometry()
        {
            if (_fe == null)
                return;

            _fe.Clip =
                new RectangleGeometry
                {
                    Rect = new Rect(0, 0, _fe.ActualWidth, _fe.ActualHeight)
                };
        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        public void Detach()
        {
            if (_fe == null)
                return;

            _fe.Loaded -= Loaded;
            _fe.SizeChanged -= OnSizeChanged;
            _fe.UnregisterPropertyChangedCallback(FrameworkElement.HeightProperty, this.heightSign);
            _fe.UnregisterPropertyChangedCallback(FrameworkElement.WidthProperty, this.widthSign);
            _fe = null;
        }
    }
    #endregion
}
