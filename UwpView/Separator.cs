﻿//The MIT License(MIT)

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

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// Defines an Axis.Separator, this class customizes the separator of an axis.
    /// </summary>
    public class Separator : Control, ISeparatorView
    {
        /// <summary>
        /// Initializes a new instance of Separator class
        /// </summary>
        public Separator()
        {
            /*Current*/SetValue(IsEnabledProperty, true);
            /*Current*/SetValue(StrokeProperty, new SolidColorBrush(Color.FromArgb(255, 240, 240, 240)));
            /*Current*/SetValue(StrokeThicknessProperty, 1d);
        }

        /// <summary>
        /// Gets the chart the own the separator
        /// </summary>
        public ChartCore Chart { get; set; }

        #region Dependency Properties

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof (Brush), typeof (Separator),
            new PropertyMetadata(default(Brush), CallChartUpdater()));

        /// <summary>
        /// Gets or sets separators color 
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush) GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof (double), typeof (Separator),
            new PropertyMetadata(default(double), CallChartUpdater()));

        /// <summary>
        /// Gets or sets separators thickness
        /// </summary>
        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
            "StrokeDashArray", typeof (DoubleCollection), typeof (Separator),
            new PropertyMetadata(default(DoubleCollection), CallChartUpdater()));

        /// <summary>
        /// Gets or sets the stroke dash array for the current separator.
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection) GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
            "Step", typeof (double?), typeof (Separator),
            new PropertyMetadata(default(double?), CallChartUpdater()));

        /// <summary>
        /// Gets or sets separators step, this means the value between each line, default is null, when null this value is calculated automatically.
        /// </summary>
        public double? Step
        {
            get { return (double?) GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        #endregion

        public SeparatorConfigurationCore AsCoreElement(AxisCore axis)
        {
            return new SeparatorConfigurationCore(axis)
            {
                IsEnabled = IsEnabled,
                Step = Step
            };
        }

        private static PropertyChangedCallback CallChartUpdater(bool animate = false)
        {
            return (o, args) =>
            {
                var wpfSeparator = o as Separator;
                if (wpfSeparator == null) return;

                if (wpfSeparator.Chart != null) wpfSeparator.Chart.Updater.Run(animate);
            };
        }
    }
}
