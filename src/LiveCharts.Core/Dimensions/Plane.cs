using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Data;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Events;

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a Plane.
    /// </summary>
    public class Plane : IResource, INotifyPropertyChanged
    {
        private float[] _pointWidth;
        private Font _font;
        private Func<double, string> _labelFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// </summary>
        public Plane()
        {
            MinValue = float.NaN;
            MaxValue = float.NaN;
            DataRange = new RangeF();
            LabelFormatter = Formatters.AsMetricNumber;
            Charting.BuildFromSettings(this);
        }

        /// <summary>
        /// Gets or sets the data range.
        /// </summary>
        /// <value>
        /// The data range.
        /// </value>
        public RangeF DataRange { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum value to display.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public float MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value to display.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public float MinValue { get; set; }

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
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font
        {
            get => _font;
            set
            {
                _font = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the labels.
        /// </summary>
        /// <value>
        /// The labels.
        /// </value>
        public IList<string> Labels { get; set; }

        /// <summary>
        /// Gets or sets the label formatter.
        /// </summary>
        /// <value>
        /// The label formatter.
        /// </value>
        public Func<double, string> LabelFormatter
        {
            get => _labelFormatter;
            set => _labelFormatter = value ?? throw new LiveChartsException(
                                         $"{nameof(LabelFormatter)} can not be null.", 0);
        }

        /// <summary>
        /// Gets or sets the labels rotation.
        /// </summary>
        /// <value>
        /// The labels rotation.
        /// </value>
        public double LabelsRotation { get; set; }

        /// <summary>
        /// Gets the actual labels rotation.
        /// </summary>
        /// <value>
        /// The actual labels rotation.
        /// </value>
        public double ActualLabelsRotation
        {
            get
            {
                // we only allow angles from -90° to 90°
                // see appendix/labels.1.png
                var alpha = LabelsRotation % 360;
                if (alpha < -90) alpha += 360;
                if (alpha > 90) alpha += 180;
                return alpha;
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
        public bool Reverse { get; set; }

        internal bool ActualReverse { get; set; }

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
                    ? Labels[checked((int)value)]
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
        protected virtual IPlaneLabelControl DefaultLabelProvider()
        {
            throw new LiveChartsException(
                $"A {nameof(IPlaneLabelControl)} was not found when trying to draw Plane in the UI", 115);
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
