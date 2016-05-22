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
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Interaction logic for DefaultTooltip.xaml
    /// </summary>
    public partial class DefaultTooltip : INotifyPropertyChanged
    {
        private TooltipData _data;

        public DefaultTooltip()
        {
            InitializeComponent();

            SetValue(ForegroundProperty, Brushes.White);
            SetValue(CornerRadiusProperty, 4d);
            SetValue(SelectionModeProperty, TooltipSelectionMode.SharedXValues);

            DataContext = this;
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof (double), typeof (DefaultTooltip), new PropertyMetadata(2d));

        public double CornerRadius
        {
            get { return (double) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode", typeof (TooltipSelectionMode), typeof (DefaultTooltip),
            new PropertyMetadata(TooltipSelectionMode.SharedXValues));

        public TooltipSelectionMode SelectionMode
        {
            get { return (TooltipSelectionMode) GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (TooltipData) value;

            if (v.SelectionMode == TooltipSelectionMode.OnlySender) return string.Empty;

            return v.SelectionMode == TooltipSelectionMode.SharedXValues
                ? v.XFormatter(v.SharedValue ?? 0)
                : v.YFormatter(v.SharedValue ?? 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChartPointLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var chartPoint = (ChartPoint) value;

            return chartPoint.SeriesView.LabelPoint(chartPoint);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ParticipationVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (TooltipData) value;

            return v.Points.Any(x => x.ChartPoint.Participation > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SharedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (TooltipData) value;

            if (v.SelectionMode == TooltipSelectionMode.OnlySender) return Visibility.Collapsed;

            return v.SharedValue == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TooltipData
    {
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public double? SharedValue { get; set; }
        public TooltipSelectionMode SelectionMode { get; set; }
        public List<DataPointViewModel> Points { get; set; }
    }

    public class DataPointViewModel 
    {
        public SeriesViewModel Series { get; set; }
        public ChartPoint ChartPoint { get; set; }
    }

    public class SeriesViewModel
    {
        public string Title { get; set; }
        public Brush Stroke { get; set; }
        public double StrokeThickness { get; set; }
        public Brush Fill { get; set; }
    }

}
