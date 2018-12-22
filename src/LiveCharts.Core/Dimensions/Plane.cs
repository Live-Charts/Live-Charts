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
using LiveCharts.Core.Drawing.Brushes;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction;
using LiveCharts.Core.Interaction.Dimensions;
using LiveCharts.Core.Interaction.Events;
using Font = LiveCharts.Core.Drawing.Styles.Font;
using FontStyle = LiveCharts.Core.Drawing.Styles.FontStyle;

#endregion

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a Plane.
    /// </summary>
    public class Plane : IResource, INotifyPropertyChanged
    {
        private float[] _pointLength;
        private Func<double, string> _labelFormatter;
        private double _maxValue;
        private double _minValue;
        private string _title;
        private IList<string> _labels;
        private bool _reverse;
        private IEnumerable<Section> _sections;
        private double _labelsRotation;
        private Font _labelsFont;
        private IBrush _labelsForeground;
        private Margin _labelsPadding;
        private double _pointMargin;
        private bool _showLabels;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// </summary>
        public Plane()
        {
            _minValue = double.NaN;
            _maxValue = double.NaN;
            _pointMargin = double.NaN;
            _showLabels = true;
            _labelFormatter = Format.AsMetricNumber;
            _labelsFont = new Font("Arial", 11, FontStyle.Regular, FontWeight.Regular);
            _labelsForeground = Charting.Settings.UiProvider.GetNewSolidColorBrush(255, 30, 30, 30);
            Charting.BuildFromTheme(this);
        }

        /// <summary>
        /// Occurs when the user zooms or pans the plane.
        /// </summary>
        public event RangeChangedEventHandler RangeChanged;

        /// <summary>
        /// Gets the used <see cref="MaxValue"/>, if <see cref="MaxValue"/> is double.NaN, 
        /// the library will calculate this property and will expose the calculated value here,
        /// if <see cref="MaxValue"/> is not double.NaN, <see cref="ActualMaxValue"/> is equals
        /// to <see cref="MaxValue"/> property.
        /// </summary>
        public double ActualMaxValue { get; internal set; }

        internal double InternalMaxValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value to display in this plane, use double.NaN to let the library 
        /// calculate it automatically.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the used <see cref="MinValue"/>, if <see cref="MinValue"/> is double.NaN, 
        /// the library will calculate this property and will expose the calculated value here,
        /// if <see cref="MinValue"/> is not double.NaN, <see cref="ActualMinValue"/> is equals
        /// to <see cref="MinValue"/> property.
        /// </summary>
        public double ActualMinValue { get; internal set; }

        internal double InternalMinValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value to display.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public double MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the actual range, the range of values displayed in the current axis, 
        /// this property is the difference between <see cref="ActualMaxValue"/> and
        /// <see cref="ActualMinValue"/>.
        /// </summary>
        /// <value>
        /// The actual range.
        /// </value>
        public double ActualRange => InternalMaxValue - InternalMinValue;

        /// <summary>
        /// Gets the used <see cref="PointLength"/>, if <see cref="PointLength"/> is null, 
        /// the library will calculate this property and will expose the calculated value here,
        /// if <see cref="PointLength"/> is not null, <see cref="ActualPointLength"/> is equals
        /// to <see cref="PointLength"/> property.
        /// </summary>
        public float[] ActualPointLength { get; internal set; }

        /// <summary>
        /// Gets or sets the <see cref="PointLength"/>, this property represents the space taken by a single point,
        /// In LiveCharts for example a LineSeries point has a width and height of 0, while a BarSeries was a width of 1
        /// and a height of 0, this means that the library requires 1 in the X UI coordinate to display the bar properly, 
        /// PointWidth[0] represents the X in the UI while PointWidth[1] represents Y in the UI, if this property is not 
        /// set, the library will calculate based on the requirements of every series in the plot.
        /// </summary>
        /// <value>
        /// The width of the point.
        /// </value>
        public float[] PointLength
        {
            get => _pointLength;
            set
            {
                _pointLength = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the used <see cref="PointMargin"/>, if <see cref="PointMargin"/> is double.NaN, 
        /// the library will calculate this property and will expose the calculated value here,
        /// if <see cref="PointMargin"/> is not double.NaN, <see cref="ActualPointMargin"/> is equals
        /// to <see cref="PointMargin"/> property.
        /// </summary>
        public double ActualPointMargin { get; internal set; }

        /// <summary>
        /// Gets the point margin, this property represents the space required for a point from
        /// its coordinate to every direction (left, top, right and bottom), the library will
        /// at least add a margin in the plot based on this property.
        /// </summary>
        /// <value>
        /// The point margin.
        /// </value>
        public double PointMargin
        {
            get => _pointMargin;
            set
            {
                _pointMargin = value;
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
        /// Gets or sets a value indicating whether the labels are shown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if labels are visible; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLabels
        {
            get => _showLabels;
            set
            {
                _showLabels = value;
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
        public IBrush LabelsForeground
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
                _labelFormatter = value ?? throw new LiveChartsException(105);
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
            return value >= InternalMinValue && value <= InternalMaxValue;
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
                Math.Abs(chart.ScaleToUi(0f, this) - chart.ScaleToUi(PointLength[0], this)),
                Math.Abs(chart.ScaleToUi(0f, this) - chart.ScaleToUi(PointLength[1], this))
            };
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        protected virtual void OnDispose(IChartView view, bool force)
        {
        }

        /// <summary>
        /// The default separator provider.
        /// </summary>
        /// <returns></returns>
        protected virtual IPlaneViewProvider DefaultViewProvider()
        {
            throw new LiveChartsException(140 ,ToString());
        }

        /// <summary>
        /// Called when [range changed].
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <param name="previousMin">The previousMin.</param>
        /// <param name="previousMax">The previousMax.</param>
        protected virtual void OnRangeChanged(Plane plane, double previousMin, double previousMax)
        {
            RangeChanged?.Invoke(plane, previousMin, previousMax);
        }

        /// <summary>
        /// Sets the range.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public void SetRange(double min, double max)
        {
            double minCopy = _minValue;
            double maxCopy = _maxValue;

            _minValue = min;
            _maxValue = max;

            OnPropertyChanged(nameof(ActualRange));
            OnRangeChanged(this, minCopy, maxCopy);
        }

        #region IResource implementation

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        void IResource.Dispose(IChartView view, bool force)
        {
            OnDispose(view, force);
            Disposed?.Invoke(view, this, force);
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
