using System;
using System.Windows;
using System.Windows.Media;

namespace Desktop
{
    public class Separator : FrameworkElement, ISeparatorView
    {
        public Separator()
        {
            Model = new LiveChartsCore.Separator(this);
        }
        public ISeparatorModel Model { get; private set; }

        public new static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled", typeof (bool), typeof (Separator), 
            new PropertyMetadata(default(bool), OnPropertyChanged((v,m) => m.IsEnabled = v.IsEnabled)));
        /// <summary>
        /// Gets or sets if separators are enabled (will be drawn)
        /// </summary>
        public new bool IsEnabled
        {
            get { return (bool) GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof (Color), typeof (Separator), 
            new PropertyMetadata(default(Color)));
        /// <summary>
        /// Gets or sets separators color 
        /// </summary>
        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (int), typeof (Separator), 
            new PropertyMetadata(default(int), OnPropertyChanged((v, m)=> m.StrokeThickness = v.StrokeThickness)));
        /// <summary>
        /// Gets or sets separatos thickness
        /// </summary>
        public int StrokeThickness
        {
            get { return (int) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
            "Step", typeof (double?), typeof (Separator),
            new PropertyMetadata(default(double?), OnPropertyChanged((v, m) => m.Step = v.Step)));
        /// <summary>
        /// Gets or sets sepator step, this means the value between each line, use null for auto.
        /// </summary>
        public double? Step
        {
            get { return (double?) GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        private static PropertyChangedCallback OnPropertyChanged(bool animate = false)
        {
            return (o, args) =>
            {
                var wpfSeries = o as Separator;
                if (wpfSeries == null) return;
                wpfSeries.Model.Chart.Update(animate);
            };
        }

        private static PropertyChangedCallback OnPropertyChanged(Action<Separator, ISeparatorModel> map, bool animate = false)
        {
            return (o, args) =>
            {
                var wpfSeparator = o as Separator;
                if (wpfSeparator == null) return;

                map(wpfSeparator, wpfSeparator.Model);

                wpfSeparator.Model.Chart.Update(animate);
            };
        }
    }
}
