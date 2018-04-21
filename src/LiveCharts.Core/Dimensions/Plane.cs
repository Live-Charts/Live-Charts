#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Dimensions;
using LiveCharts.Core.Interaction.Styles;
using Brush = LiveCharts.Core.Drawing.Brush;
using Font = LiveCharts.Core.Interaction.Styles.Font;
using FontStyle = LiveCharts.Core.Interaction.Styles.FontStyle;

#endregion

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a Plane.
    /// </summary>
    public class Plane : IResource, INotifyPropertyChanged
    {
        private float[] _pointWidth;
        private Func<double, string> _labelFormatter;
        private float _maxValue;
        private float _minValue;
        private string _title;
        private IList<string> _labels;
        private bool _reverse;
        private IEnumerable<Section> _sections;
        private double _labelsRotation;
        private Font _labelsFont;
        private Brush _labelsForeground;
        private Margin _labelsPadding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// </summary>
        public Plane()
        {
            MinValue = float.NaN;
            MaxValue = float.NaN;
            LabelFormatter = Format.AsMetricNumber;
            LabelsFont = new Font("Arial", 11, FontStyle.Regular, FontWeight.Regular);
            LabelsForeground = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
            Charting.BuildFromSettings(this);
        }

        /// <summary>
        /// Gets the point margin.
        /// </summary>
        /// <value>
        /// The point margin.
        /// </value>
        public float PointMargin { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum value to display.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the minimum value to display.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public float MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the actual maximum value.
        /// </summary>
        /// <value>
        /// The actual maximum value.
        /// </value>
        public float ActualMaxValue { get; internal set; }

        /// <summary>
        /// Gets the actual minimum value.
        /// </summary>
        /// <value>
        /// The actual minimum value.
        /// </value>
        public float ActualMinValue { get; internal set; }

        /// <summary>
        /// Gets the actual point unit.
        /// </summary>
        /// <value>
        /// The actual point unit.
        /// </value>
        public float[] ActualPointWidth { get; internal set; }

        /// <summary>
        /// Gets or sets the width of the point.
        /// </summary>
        /// <value>
        /// The width of the point.
        /// </value>
        public float[] PointWidth
        {
            get => _pointWidth;
            set
            {
                _pointWidth = value;
                ActualPointWidth = new [] {0f, 0f};
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the labels.
        /// </summary>
        /// <value>
        /// The labels.
        /// </value>
        public IList<string> Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the labels font.
        /// </summary>
        /// <value>
        /// The labels font.
        /// </value>
        public Font LabelsFont
        {
            get => _labelsFont;
            set
            {
                _labelsFont = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the labels foreground.
        /// </summary>
        /// <value>
        /// The labels foreground.
        /// </value>
        public Brush LabelsForeground
        {
            get => _labelsForeground;
            set
            {
                _labelsForeground = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the labels rotation.
        /// </summary>
        /// <value>
        /// The labels rotation.
        /// </value>
        public double LabelsRotation
        {
            get => _labelsRotation;
            set
            {
                _labelsRotation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the labels padding.
        /// </summary>
        /// <value>
        /// The labels padding.
        /// </value>
        public Margin LabelsPadding
        {
            get => _labelsPadding;
            set
            {
                _labelsPadding = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the label formatter.
        /// </summary>
        /// <value>
        /// The label formatter.
        /// </value>
        public Func<double, string> LabelFormatter
        {
            get => _labelFormatter;
            set
            {
                _labelFormatter = value ?? throw new LiveChartsException(
                                      $"{nameof(Axis)}.{nameof(LabelFormatter)} property can not be null.", 0);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public IEnumerable<Section> Sections
        {
            get => _sections;
            set
            {
                _sections = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the dimension affected, for a cartesian chart -> 0: X, 1: Y.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Dimension { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Plane"/> is drawn in reverse.
        /// </summary>
        /// <value>
        ///   <c>true</c> if reverse; otherwise, <c>false</c>.
        /// </value>
        public bool Reverse
        {
            get => _reverse;
            set
            {
                _reverse = value;
                OnPropertyChanged();
            }
        }

        internal bool ActualReverse { get; set; }

        internal Margin ByStackMargin { get; set; }

        /// <summary>
        /// Formats a given value according to the axis, <see cref="LabelFormatter"/> and <see cref="Labels"/> properties.
        /// If <see cref="Labels"/> property is null, <see cref="LabelFormatter"/> delegate will be used to get the formatted value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string FormatValue(double value)
        {
            if (Labels != null)
            {
                return Labels.Count > value && value >= 0
                    ? Labels[checked((int) value)]
                    : "";
            }

            return LabelFormatter(value);
        }

        /// <summary>
        /// Determines whether a given value is between the plane range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is in range] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInRange(double value)
        {
            return value >= ActualMinValue && value <= ActualMaxValue;
        }

        /// <summary>
        /// Gets the UI unit width.
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <returns></returns>
        public float[] Get2DUiUnitWidth(ChartModel chart)
        {
            return new[]
            {
                chart.ScaleToUi(0f, this) - chart.ScaleToUi(PointWidth[0], this),
                chart.ScaleToUi(0f, this) - chart.ScaleToUi(PointWidth[1], this)
            };
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        protected virtual void OnDispose(IChartView view)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The default separator provider.
        /// </summary>
        /// <returns></returns>
        protected virtual IPlaneViewProvider DefaultViewProvider()
        {
            throw new LiveChartsException(
                $"A {nameof(IMeasurableLabel)} was not found when trying to draw Plane in the UI", 115);
        }

        #region IResource implementation

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        void IResource.Dispose(IChartView view)
        {
            OnDispose(view);
            Disposed?.Invoke(view, this);
        }

        #endregion

        #region INPC implementation

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
