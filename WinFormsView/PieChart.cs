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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using LiveCharts.Events;
using LiveCharts.Wpf;

namespace LiveCharts.WinForms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Integration.ElementHost" />
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]
    public class PieChart : ElementHost
    {
        /// <summary>
        /// The WPF base
        /// </summary>
        protected readonly Wpf.PieChart WpfBase = new Wpf.PieChart();

        /// <summary>
        /// Initializes a new instance of the <see cref="PieChart"/> class.
        /// </summary>
        public PieChart()
        {
            Child = WpfBase;
            WpfBase.DataClick += (o, point) =>
            {
                if (DataClick != null) DataClick.Invoke(o, point);
            };
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                WpfBase.Series = WpfBase.GetDesignerModeCollection();
            }
        }

        /// <summary>
        /// Occurs when [data click].
        /// </summary>
        public event DataClickHandler DataClick;

        #region ChartProperties

        /// <summary>
        /// Gets or sets the axis y.
        /// </summary>
        /// <value>
        /// The axis y.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public AxesCollection AxisY
        {
            get { return WpfBase.AxisY; }
            set { WpfBase.AxisY = value; }
        }

        /// <summary>
        /// Gets or sets the axis x.
        /// </summary>
        /// <value>
        /// The axis x.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public AxesCollection AxisX
        {
            get { return WpfBase.AxisX; }
            set { WpfBase.AxisX = value; }
        }

        /// <summary>
        /// Gets or sets the default legend.
        /// </summary>
        /// <value>
        /// The default legend.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public UserControl DefaultLegend
        {
            get { return WpfBase.ChartLegend; }
            set { WpfBase.ChartLegend = value; }
        }

        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public ZoomingOptions Zoom
        {
            get { return WpfBase.Zoom; }
            set { WpfBase.Zoom = value; }
        }

        /// <summary>
        /// Gets or sets the legend location.
        /// </summary>
        /// <value>
        /// The legend location.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public LegendLocation LegendLocation
        {
            get { return WpfBase.LegendLocation; }
            set { WpfBase.LegendLocation = value; }
        }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>
        /// The series.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SeriesCollection Series
        {
            get { return WpfBase.Series; }
            set { WpfBase.Series = value; }
        }

        /// <summary>
        /// Gets or sets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AnimationsSpeed
        {
            get { return WpfBase.AnimationsSpeed; }
            set { WpfBase.AnimationsSpeed = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [disable animations].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable animations]; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DisableAnimations
        {
            get { return WpfBase.DisableAnimations; }
            set { WpfBase.DisableAnimations = value; }
        }

        /// <summary>
        /// Gets or sets the data tooltip.
        /// </summary>
        /// <value>
        /// The data tooltip.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UserControl DataTooltip
        {
            get { return WpfBase.DataTooltip; }
            set { WpfBase.DataTooltip = value; }
        }
        #endregion

        #region ThisChartProperties

        /// <summary>
        /// Gets or sets the inner radius.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double InnerRadius
        {
            get { return WpfBase.InnerRadius; }
            set { WpfBase.InnerRadius = value; }
        }

        /// <summary>
        /// Gets or sets the starting rotation angle.
        /// </summary>
        /// <value>
        /// The starting rotation angle.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double StartingRotationAngle
        {
            get { return WpfBase.StartingRotationAngle; }
            set { WpfBase.StartingRotationAngle = value; }
        }

        /// <summary>
        /// Gets or sets the state of the updater.
        /// </summary>
        /// <value>
        /// The state of the updater.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdaterState UpdaterState
        {
            get { return WpfBase.UpdaterState; }
            set { WpfBase.UpdaterState = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Updates the specified restart view.
        /// </summary>
        /// <param name="restartView">if set to <c>true</c> [restart view].</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public void Update(bool restartView, bool force)
        {
            WpfBase.Update(restartView, force);
        }
        #endregion
    }
}