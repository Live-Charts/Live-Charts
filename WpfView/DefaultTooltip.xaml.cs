//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// The Default Tooltip control, by default any chart that requires a tooltip will create a new instance of this class.
    /// </summary>
    public partial class DefaultTooltip : IChartTooltip
    {
        private TooltipData _data;

        /// <summary>
        /// Initializes a new instance of DefaultTooltip class
        /// </summary>
        public DefaultTooltip()
        {
            InitializeComponent();
            
            DataContext = this;
        }

        /// <summary>
        /// Initializes the <see cref="DefaultTooltip"/> class.
        /// </summary>
        static DefaultTooltip()
        {
            BackgroundProperty.OverrideMetadata(
                typeof(DefaultTooltip), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(140, 255, 255, 255))));
            PaddingProperty.OverrideMetadata(
                typeof(DefaultTooltip), new FrameworkPropertyMetadata(new Thickness(10, 5, 10, 5)));
            EffectProperty.OverrideMetadata(
                typeof(DefaultTooltip),
                new FrameworkPropertyMetadata(new DropShadowEffect {BlurRadius = 3, Color = Color.FromRgb(50,50,50), Opacity = .2}));
        }

        /// <summary>
        /// The show title property
        /// </summary>
        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
            "ShowTitle", typeof(bool), typeof(DefaultTooltip), new PropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether the tooltip should show the shared coordinate value in the current tooltip data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show title]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowTitle
        {
            get { return (bool) GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, value); }
        }

        /// <summary>
        /// The show series property
        /// </summary>
        public static readonly DependencyProperty ShowSeriesProperty = DependencyProperty.Register(
            "ShowSeries", typeof(bool), typeof(DefaultTooltip), new PropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether should show series name and color.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show series]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSeries
        {
            get { return (bool) GetValue(ShowSeriesProperty); }
            set { SetValue(ShowSeriesProperty, value); }
        }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof (CornerRadius), typeof (DefaultTooltip), new PropertyMetadata(new CornerRadius(4)));
        /// <summary>
        /// Gets or sets the corner radius of the tooltip
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// The selection mode property
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode", typeof (TooltipSelectionMode?), typeof (DefaultTooltip),
            new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the tooltip selection mode, default is null, if this property is null LiveCharts will decide the selection mode based on the series (that fired the tooltip) preferred section mode
        /// </summary>
        public TooltipSelectionMode? SelectionMode
        {
            get { return (TooltipSelectionMode?) GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// The bullet size property
        /// </summary>
        public static readonly DependencyProperty BulletSizeProperty = DependencyProperty.Register(
            "BulletSize", typeof (double), typeof (DefaultTooltip), new PropertyMetadata(15d));
        /// <summary>
        /// Gets or sets the bullet size, the bullet size modifies the drawn shape size.
        /// </summary>
        public double BulletSize
        {
            get { return (double) GetValue(BulletSizeProperty); }
            set { SetValue(BulletSizeProperty, value); }
        }

        /// <summary>
        /// The is wrapped property
        /// </summary>
        public static readonly DependencyProperty IsWrappedProperty = DependencyProperty.Register(
            "IsWrapped", typeof (bool), typeof (DefaultTooltip), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets whether the tooltip is shared in the current view, this property is useful to control
        /// the z index of a tooltip according to a set of controls in a container.
        /// </summary>
        public bool IsWrapped
        {
            get { return (bool) GetValue(IsWrappedProperty); }
            set { SetValue(IsWrappedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public TooltipData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class SharedConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as TooltipData;

            if (v == null) return null;

            if (v.SelectionMode == TooltipSelectionMode.OnlySender) return string.Empty;

            return v.SelectionMode == TooltipSelectionMode.SharedXValues
                ? v.XFormatter(v.SharedValue ?? 0)
                : v.YFormatter(v.SharedValue ?? 0);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class ChartPointLabelConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var chartPoint = value as ChartPoint;

            if (chartPoint == null) return null;

            return chartPoint.SeriesView.LabelPoint(chartPoint);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class ParticipationVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as TooltipData;
            if (v == null) return null;

            return v.Points.Any(x => x.ChartPoint.Participation > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Data.IMultiValueConverter" />
    public class SharedVisibilityConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var v = values[0] as TooltipData;
            var show = values[1] as bool?;

            if (v == null || show == null) return null;

            if (show.Value == false)
                return Visibility.Collapsed;

            if (v.SelectionMode == TooltipSelectionMode.OnlySender)
                return Visibility.Collapsed;

            return v.SharedValue == null
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Contains information about data in a tooltip
    /// </summary>
    public class TooltipData
    {
        /// <summary>
        /// The current X formatter
        /// </summary>
        public Func<double, string> XFormatter { get; internal set; }
        /// <summary>
        /// The current Y formatter
        /// </summary>
        public Func<double, string> YFormatter { get; internal set; }
        /// <summary>
        /// Shared coordinate value between points
        /// </summary>
        public double? SharedValue { get; internal set; }
        /// <summary>
        /// Gets or sets the series that fired the tooltip.
        /// </summary>
        /// <value>
        /// The sender series.
        /// </value>
        public Series SenderSeries { get; internal set; }
        /// <summary>
        /// Current selection mode
        /// </summary>
        public TooltipSelectionMode SelectionMode { get; internal set; }
        /// <summary>
        /// collection of points
        /// </summary>
        public List<DataPointViewModel> Points { get; internal set; }
    }

    /// <summary>
    /// Point Data
    /// </summary>
    public class DataPointViewModel 
    {
        /// <summary>
        /// Gets info about the series that owns the point, like stroke and stroke thickness
        /// </summary>
        public SeriesViewModel Series { get; set; }
        /// <summary>
        /// Gets the ChartPoint instance
        /// </summary>
        public ChartPoint ChartPoint { get; set; }
    }

    /// <summary>
    /// Series Data
    /// </summary>
    public class SeriesViewModel
    {
        /// <summary>
        /// Series Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Series stroke
        /// </summary>
        public Brush Stroke { get; set; }
        /// <summary>
        /// Series Stroke thickness
        /// </summary>
        public double StrokeThickness { get; set; }
        /// <summary>
        /// Series Fill
        /// </summary>
        public Brush Fill { get; set; }
        /// <summary>
        /// Series point Geometry
        /// </summary>
        public Geometry PointGeometry { get; set; }
    }

}
