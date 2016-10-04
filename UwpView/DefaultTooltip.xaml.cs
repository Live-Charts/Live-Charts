//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp
{
    public interface IChartTooltip : INotifyPropertyChanged
    {
        TooltipData Data { get; set; }
        TooltipSelectionMode? SelectionMode { get; set; }
        bool IsWrapped { get; set; }
    }

    /// <summary>
    /// The Default Tooltip control, by default any chart that requires a tooltip will create a new instance of this class.
    /// </summary>
    public partial class DefaultTooltip : UserControl ,IChartTooltip
    {
        private TooltipData _data;

        /// <summary>
        /// Initializes a new instance of DefaultTooltip class
        /// </summary>
        public DefaultTooltip()
        {
            InitializeComponent();

            SetValue(ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)));
            SetValue(CornerRadiusProperty, 4d);

            DataContext = this;
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof (double), typeof (DefaultTooltip), new PropertyMetadata(2d));
        /// <summary>
        /// Gets or sets the corner radius of the tooltip
        /// </summary>
        public double CornerRadius
        {
            get { return (double) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

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

        public TooltipData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SharedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = value as TooltipData;

            if (v == null) return null;

            if (v.SelectionMode == TooltipSelectionMode.OnlySender) return string.Empty;

            return v.SelectionMode == TooltipSelectionMode.SharedXValues
                ? v.XFormatter(v.SharedValue ?? 0)
                : v.YFormatter(v.SharedValue ?? 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ChartPointLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var chartPoint = value as ChartPoint;

            if (chartPoint == null) return null;

            return chartPoint.SeriesView.LabelPoint(chartPoint);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ParticipationVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = value as TooltipData;
            if (v == null) return null;

            return v.Points.Any(x => x.ChartPoint.Participation > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class SharedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = value as TooltipData;

            if (v == null) return null;

            if (v.SelectionMode == TooltipSelectionMode.OnlySender) return Visibility.Collapsed;

            return v.SharedValue == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
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
        public Func<double, string> XFormatter { get; set; }
        /// <summary>
        /// The current Y formatter
        /// </summary>
        public Func<double, string> YFormatter { get; set; }
        /// <summary>
        /// Shared coordinate value between points
        /// </summary>
        public double? SharedValue { get; set; }
        /// <summary>
        /// Current selection mode
        /// </summary>
        public TooltipSelectionMode SelectionMode { get; set; }
        /// <summary>
        /// collection of points
        /// </summary>
        public List<DataPointViewModel> Points { get; set; }
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
